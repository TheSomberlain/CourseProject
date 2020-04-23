using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Interfaces;
using EasyCaching.Core;

namespace CourseProjectMVC.Services
{
    public class RedisService : IRedisService
    {
        private readonly IEasyCachingProvider _provider;
        private IEasyCachingProviderFactory _providerFactory;

        public RedisService(IEasyCachingProviderFactory p)
        {
            _providerFactory = p;
            _provider = p.GetCachingProvider("redis1");
        }
        public async Task<CacheValue<string>> Consume(string key)
        {
            var item = await _provider.GetAsync<string>(key);
            return item;
        }

        public async Task Delete(string key)
        {
            await _provider.RemoveAsync(key);
        }

        public async Task Put(string key, string item)
        {
            await _provider.SetAsync(key,item, TimeSpan.FromMinutes(20));
        }

        public Task Update(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
