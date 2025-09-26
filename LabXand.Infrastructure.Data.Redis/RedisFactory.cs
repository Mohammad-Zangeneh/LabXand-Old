using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.Redis
{
    public class RedisFactory
    {
        private RedisFactory() { }
        private static IRedisCacheService redisCacheService;
        public static IRedisCacheService CreateInstance()
        {
            if (redisCacheService == null)
                redisCacheService = new RedisCacheService(new RedisStore());

            return redisCacheService;
        }
    }
}
