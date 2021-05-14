using RestSharpSample.Interfaces;
using System;
using System.Runtime.Caching;

namespace RestSharpSample.Concrete
{
    public class CacheService : ICacheService
    {
        public T Get<T>(string key) where T : class
        {
            return MemoryCache.Default.Get(key) as T;
        }

        public void Set<T>(string key, object data, int minutes )
        {
            if(data != null)
            {
                MemoryCache.Default.Add(key,data,DateTime.Now.AddMinutes(minutes));
            }
        }
    }
}
