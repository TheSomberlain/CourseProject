using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCaching.Core;

namespace CourseProjectMVC.Interfaces
{
    public interface IRedisService
    {
        Task Put(string key, string value);
        Task<CacheValue<string>> Consume(string key);
        Task Update(string key, string value);
        Task Delete(string key);
    }
}
