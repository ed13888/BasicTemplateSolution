using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>运行时缓存工具类</summary>
    public class CacheUtil : IDisposable
    {
        private static bool _isDispose = false;
        private static readonly object lockObj = new object();
        private static ObjectCache _cache;

        private static ObjectCache Cache
        {
            get
            {
                if (CacheUtil._cache == null)
                {
                    lock (CacheUtil.lockObj)
                    {
                        if (CacheUtil._cache == null)
                            CacheUtil._cache = (ObjectCache)MemoryCache.Default;
                    }
                }
                return CacheUtil._cache;
            }
        }

        /// <summary>获取缓存数据</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (CacheUtil.Cache.Contains(key, (string)null))
                return (T)CacheUtil.Cache.Get(key, (string)null);
            return default(T);
        }

        /// <summary>永久缓存</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public static void Set<T>(string key, T item)
        {
            CacheUtil.SetCache<T>(key, item, new CacheItemPolicy()
            {
                Priority = CacheItemPriority.NotRemovable
            });
        }

        /// <summary>设置过期时间</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="hours"></param>
        public static void Set<T>(string key, T item, double hours = 12.0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            DateTime dateTime = DateTime.Now.AddHours(hours);
            policy.AbsoluteExpiration = new DateTimeOffset(dateTime);
            CacheUtil.SetCache<T>(key, item, policy);
        }

        /// <summary>设置过期时间</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="hours"></param>
        public static void Set<T>(string key, T item, TimeSpan timeOut)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            DateTime dateTime = DateTime.Now.Add(timeOut);
            policy.AbsoluteExpiration = new DateTimeOffset(dateTime);
            CacheUtil.SetCache<T>(key, item, policy);
        }

        /// <summary>设置过期时间</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="minutes"></param>
        public static void SetMinutes<T>(string key, T item, double minutes = 10.0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            DateTime dateTime = DateTime.Now.AddMinutes(minutes);
            policy.AbsoluteExpiration = new DateTimeOffset(dateTime);
            CacheUtil.SetCache<T>(key, item, policy);
        }

        /// <summary>设置过期时间</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="seconds"></param>
        public static void SetSeconds<T>(string key, T item, double seconds = 10.0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            DateTime dateTime = DateTime.Now.AddSeconds(seconds);
            policy.AbsoluteExpiration = new DateTimeOffset(dateTime);
            CacheUtil.SetCache<T>(key, item, policy);
        }

        /// <summary>写入缓存</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        private static void SetCache<T>(string key, T item, CacheItemPolicy policy)
        {
            if (CacheUtil.Cache.Contains(key, (string)null))
                CacheUtil.Cache.Set(key, (object)item, policy, (string)null);
            else
                CacheUtil.Cache.Add(key, (object)item, policy, (string)null);
        }

        public static bool ContainsKey(string key)
        {
            return CacheUtil.Cache.Contains(key, (string)null);
        }

        public static void Remove(string key)
        {
            CacheUtil.Cache.Remove(key, (string)null);
        }

        public static void RemoveAll(IEnumerable<string> keys)
        {
            foreach (string key in keys)
                CacheUtil.Cache.Remove(key, (string)null);
        }

        public static IEnumerable<string> SearchKeys(string pattern)
        {
            pattern = string.Format("^{0}$", (object)pattern.Replace("*", ".+?"));
            return CacheUtil.Cache.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>)(a => Regex.Match(a.Key, pattern, RegexOptions.IgnoreCase).Success)).Select<KeyValuePair<string, object>, string>((Func<KeyValuePair<string, object>, string>)(a => a.Key)) ?? (IEnumerable<string>)new List<string>();
        }

        public static IEnumerable<string> GetAllKeys()
        {
            return CacheUtil.Cache.Select<KeyValuePair<string, object>, string>((Func<KeyValuePair<string, object>, string>)(a => a.Key));
        }

        public static IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            IDictionary<string, T> dictionary = (IDictionary<string, T>)new Dictionary<string, T>();
            if (keys != null && keys.Any<string>())
            {
                IDictionary<string, object> values = CacheUtil.Cache.GetValues(keys, (string)null);
                if (values != null)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>)values)
                        dictionary.Add(keyValuePair.Key, (T)keyValuePair.Value);
                }
            }
            return dictionary;
        }

        public static void Clear()
        {
            foreach (string allKey in CacheUtil.GetAllKeys())
                CacheUtil.Cache.Remove(allKey, (string)null);
        }

        public void Dispose()
        {
            CacheUtil._isDispose = true;
            CacheUtil.Clear();
        }
    }
}
