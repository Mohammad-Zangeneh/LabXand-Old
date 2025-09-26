using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IRedisStore _redisStore;
        public RedisCacheService(IRedisStore redisStore)
        {
            _redisStore = redisStore;
        }

        public T GetObject<T>(string key, int dbNumber)
        {
            var value = _redisStore.GetRedisDatabase(dbNumber).StringGet(key);
            if (!value.IsNull)
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        /// <summary>
        /// Get All Objects Of Specific Db 
        /// </summary>
        /// <param name="dbNumber"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetAllObjects(int dbNumber)
        {
            var keys = GetAllKeys("*", dbNumber);
            var objects = new Dictionary<string, object>();
            foreach (var item in keys)
            {
                var obj = _redisStore.GetRedisDatabase(dbNumber).StringGet(item);
                objects.Add(item, obj);
            }

            return objects ?? default;
        }

        /// <summary>
        /// Get All Keys Of Sepcific Database Based On Pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="dbNumber"></param>
        /// <returns></returns>
        public IList<string> GetAllKeys(string pattern, int dbNumber)
        {
            var keys = _redisStore.RedisServer().Keys(dbNumber, pattern).Select(key => (string)key).ToList();
            return keys ?? default;
        }

        public bool SetObject<T>(string key, T value, int dbNumber, TimeSpan? expirationTime = null, JsonSerializerSettings jsonSetting = null)
        {
            if (jsonSetting == null)
                jsonSetting = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Serialize };
            var isSet = _redisStore.GetRedisDatabase(dbNumber).StringSet(key, JsonConvert.SerializeObject(value, Formatting.None, jsonSetting), expirationTime);
            return isSet;
        }

        public bool RemoveKey(string key, int dbNumber)
        {
            var database = _redisStore.GetRedisDatabase(dbNumber);
            var isKeyExist = database.KeyExists(key);
            if (!isKeyExist)
                return false;

            return database.KeyDelete(key);
        }

        /// <summary>
        /// Clear The Specified Database 
        /// </summary>
        /// <param name="dbNumber"></param>
        /// <returns></returns>
        public bool RemoveDatabase(int dbNumber)
        {
            try
            {
                _redisStore.RedisServer().FlushDatabase(dbNumber);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Clear All Redis Cache Database
        /// </summary>
        /// <returns></returns>
        public bool RemoveDatabase()
        {
            try
            {
                _redisStore.RedisServer().FlushDatabase();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        ////------------- Lists  ----------------------
        /// <summary>
        /// Insert to end of list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool InsertToEndOfList<T>(string key, T value, int dbNumber)
        {
            try
            {
                _redisStore.GetRedisDatabase(dbNumber).ListRightPush(key, JsonConvert.SerializeObject(value));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get all elements of specified list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string key, int dbNumber)
        {
            var result = _redisStore.GetRedisDatabase(dbNumber).ListRange(key, 0, _redisStore.GetRedisDatabase(dbNumber).ListLength(key));

            var list = new List<T>();
            foreach (var item in result)
                list.Add(JsonConvert.DeserializeObject<T>(item));

            return list ?? null;
        }
        public bool SetList<T>(string key, List<T> value, int dbNumber)
        {
            try
            {
                foreach (var item in value)
                    _redisStore.GetRedisDatabase(dbNumber).ListRightPush(key, JsonConvert.SerializeObject(item));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
