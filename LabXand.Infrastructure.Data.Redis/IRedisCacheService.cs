using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.Redis
{
    public interface IRedisCacheService
    {
        T GetObject<T>(string key, int dbNumber);
        IList<string> GetAllKeys(string pattern, int dbNumber);
        Dictionary<string, object> GetAllObjects(int dbNumber);
        bool SetObject<T>(string key, T value, int dbNumber, TimeSpan? expireTime = null, JsonSerializerSettings jsonSetting = null);
        bool RemoveKey(string key, int dbNumber);
        bool RemoveDatabase();
        bool RemoveDatabase(int dbNumber);

        List<T> GetList<T>(string key, int dbNumber);
        bool InsertToEndOfList<T>(string key, T value, int dbNumber);
        bool SetList<T>(string key, List<T> value, int dbNumber);
    }
}
