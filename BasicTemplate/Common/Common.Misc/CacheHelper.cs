using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public static class CacheHelper
    {
        //默认缓存过期时间单位秒 
        public static void Configure()
        {
            int maxWritePoolSize = FeatureHelper.MaxWritePoolSize;
            int maxReadPoolSize = FeatureHelper.MaxReadPoolSize;
            string redisConfig = SystemConfig.RedisConfig;
            if (string.IsNullOrEmpty(redisConfig)) return;

            if (redisConfig.Contains("#"))
            {
                var arr = redisConfig.Split('#');
                if (arr.Length == 2)
                {
                    string[] readWriteHosts = arr[0].Split('|', ',');
                    string[] readOnlyHosts = arr[1].Split('|', ',');
                    RedisClientManager.InitRedis(readWriteHosts, readOnlyHosts, maxWritePoolSize, maxReadPoolSize);
                }
            }
            else
            {
                string[] readWriteHosts = redisConfig.Split('|', ',');
                string[] readOnlyHosts = readWriteHosts;
                RedisClientManager.InitRedis(readWriteHosts, readOnlyHosts, maxWritePoolSize, maxReadPoolSize);
            }
        }

        /// <summary>
        /// redis手动初始化
        /// </summary>
        /// <param name="maxWritePoolSize"></param>
        /// <param name="maxReadPoolSize"></param>
        /// <param name="RedisConfig"></param>
        public static void Configure(int maxWritePoolSize, int maxReadPoolSize, string RedisConfig)
        {
            RedisClientManager.InitRedis(new string[] { RedisConfig }, new string[] { RedisConfig }, maxWritePoolSize, maxReadPoolSize);
        }

        /// <summary>
        /// 清空所有Cache
        /// </summary>
        public static void ClearAll()
        {
            RedisClientManager.FlushAll();
        }

        #region Key/Value存储
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存建</param>
        /// <param name="t">缓存值</param>
        /// <param name="timeout">过期时间，单位小时,-1：不过期，0：默认过期时间</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T t, int hours = -1)
        {
            bool flag = false;
            if (CacheKeys.HasNeedCompress(key))
            {
                string value = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                string str = GzipString.Compress(value);
                flag = RedisClientManager.Set<string>(key, str, hours);
            }
            else
            {
                flag = RedisClientManager.Set<T>(key, t, hours);
            }
            return flag;
        }

        public static bool Set<T>(string key, T t, TimeSpan timeOut)
        {
            bool flag = false;
            if (CacheKeys.HasNeedCompress(key))
            {
                string value = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                string str = GzipString.Compress(value);
                flag = RedisClientManager.Set<string>(key, str, timeOut);
            }
            else
            {
                flag = RedisClientManager.Set<T>(key, t, timeOut);
            }
            return flag;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            T t = default(T);
            if (CacheKeys.HasNeedCompress(key))
            {
                string value = RedisClientManager.Get<string>(key);
                string str = GzipString.Decompress(value);
                if (string.IsNullOrWhiteSpace(str))
                    return default(T);
                t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
                t = RedisClientManager.Get<T>(key);
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            }
            return t;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return RedisClientManager.Remove(key);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RemoveAll(IEnumerable<string> keys)
        {
            RedisClientManager.RemoveAll(keys);
        }

        public static bool Add<T>(string key, T t, int hours = -1)
        {
            bool flag = false;
            if (CacheKeys.HasNeedCompress(key))
            {
                string value = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                string str = GzipString.Compress(value);
                flag = RedisClientManager.Add<string>(key, str, hours);
            }
            else
            {
                flag = RedisClientManager.Add<T>(key, t, hours);
            }
            return flag;
        }
        #endregion

        #region 有序集合操作

        /// <summary>
        /// 获取有序集合中的所有元素KEY.
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static List<string> GetAllItemsFromSortedSet(string setId)
        {
            return RedisClientManager.GetAllItemsFromSortedSet(setId);
        }

        ///// <summary>
        ///// 根据KEY获取值
        ///// </summary>
        ///// <param name="setId"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static double GetItemValueByKey(string setId, string key)
        //{
        //    return RedisClientManager.GetItemVauleByKey(setId, key);
        //}

        /// <summary>
        /// 获取集合数据.(正序)
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetByAsc(string setId, int minValue, int maxValue)
        {
            return RedisClientManager.GetRangeWithScoresFromSortedSetByAsc(setId, minValue, maxValue);
        }

        /// <summary>
        /// 获取集合数据。（倒叙）
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetByDesc(string setId, int minValue, int maxValue)
        {
            return RedisClientManager.GetRangeWithScoresFromSortedSetByDesc(setId, minValue, maxValue);
        }

        /// <summary>
        /// 将元素加入有序集合
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddItemToSortedSet(string setId, string key, double value)
        {
            return RedisClientManager.AddItemToSortedSet(setId, key, value);
        }

        /// <summary>
        /// 获取有序集合中的数量
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string setId)
        {
            return RedisClientManager.GetSortedSetCount(setId);
        }

        /// <summary>
        /// 获取有序集合中的数量(根据值范围)
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string setId, double minValue, double maxValue)
        {
            return RedisClientManager.GetSortedSetCount(setId, minValue, maxValue);
        }

        /// <summary>
        /// 移除有序集合的元素 (根据值范围)
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static long RemoveSortedSetByRange(string setId, double minValue, double maxValue)
        {
            return RedisClientManager.RemoveSortedSetByRange(setId, minValue, maxValue);
        }

        /// <summary>
        /// 移除有序集合的元素.
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveItemFromSortedSet(string setId, string key)
        {
            return RedisClientManager.RemoveItemFromSortedSet(setId, key);
        }

        #endregion

        #region 链表操作
        /// <summary>
        /// 根据IEnumerable数据添加链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="values"></param>
        /// <param name="timeout"></param>
        public static void AddList<T>(string listId, IEnumerable<T> values, int hours = -1)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                List<string> list = new List<string>();
                foreach (var item in values)
                {
                    string value = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                    string str = GzipString.Compress(value);
                    list.Add(str);
                }
                RedisClientManager.AddList<string>(listId, list, hours);
            }
            else
            {
                RedisClientManager.AddList<T>(listId, values, hours);
            }
        }
        public static long GetListCount(string listId)
        {
            return RedisClientManager.GetListCount(listId);
        }
        /// <summary>
        /// 根据List数据添加链表
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        public static void AddRangeToList(string listId, List<string> values, int hours = -1)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                List<string> list = new List<string>();
                foreach (var item in values)
                {
                    string str = GzipString.Compress(item);
                    list.Add(str);
                }
                RedisClientManager.AddRangeToList(listId, list, hours);
            }
            else
            {
                RedisClientManager.AddRangeToList(listId, values, hours);
            }
        }
        /// <summary>
        /// 添加单个对象到List
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        public static void AddItemToList(string listId, string value, int hours = -1)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                string str = GzipString.Compress(value);
                RedisClientManager.AddItemToList(listId, str, hours);
            }
            else
            {
                RedisClientManager.AddItemToList(listId, value, hours);
            }
        }
        /// <summary>
        /// 获取链表集合
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static List<string> GetAllItemsFromList(string listId)
        {
            return RedisClientManager.GetAllItemsFromList(listId);
        }
        /// <summary>
        /// 获取链表中指定的对象
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="listIndex"></param>
        /// <returns></returns>
        public static string GetItemFromList(string listId, int listIndex)
        {
            return RedisClientManager.GetItemFromList(listId, listIndex);
        }
        /// <summary>
        /// 清除链表
        /// </summary>
        /// <param name="listId"></param>
        public static void RemoveList<T>(string listId)
        {
            RedisClientManager.RemoveList<T>(listId);
        }
        /// <summary>
        /// 添加单个实体到链表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="item"></param>
        /// <param name="timeout"></param>
        public static void AddEntityToList<T>(string listId, T item, int hours = -1)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                string value = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                string str = GzipString.Compress(value);
                RedisClientManager.AddEntityToList<string>(listId, str, hours);
            }
            else
            {
                RedisClientManager.AddEntityToList<T>(listId, item, hours);
            }
        }

        /// <summary>
        /// 获取链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetList<T>(string listId)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                List<T> list = new List<T>();
                var values = RedisClientManager.GetList<string>(listId);
                foreach (var item in values)
                {
                    string str = GzipString.Decompress(item);
                    if (string.IsNullOrWhiteSpace(str))
                        continue;
                    var t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
                    list.Add(t);
                }
                return list;
            }
            else
            {
                return RedisClientManager.GetList<T>(listId);
            }
        }
        /// <summary>
        /// 在链表中删除单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="t"></param>
        public static void RemoveEntityFromList<T>(string listId, T t)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                string value = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                string str = GzipString.Compress(value);
                RedisClientManager.RemoveEntityFromList(listId, str);
            }
            else
            {
                RedisClientManager.RemoveEntityFromList(listId, t);
            }
        }
        /// <summary>
        /// 删除并返回链表中第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        public static T RemoveStartFromList<T>(string listId)
        {
            if (CacheKeys.HasNeedCompress(listId))
            {
                var item = RedisClientManager.RemoveStartFromList<string>(listId);
                string str = GzipString.Decompress(item);
                if (string.IsNullOrWhiteSpace(str))
                    return default(T);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
                return RedisClientManager.RemoveStartFromList<T>(listId);
            }
        }
        #endregion

        #region <HashSet>
        /// <summary>
        /// 添加单个对象到HashSet
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        public static void AddItemToSet(string setId, string value, int hours = -1)
        {
            if (CacheKeys.HasNeedCompress(setId))
            {
                string str = GzipString.Compress(value);
                RedisClientManager.AddItemToSet(setId, str, hours);
            }
            else
            {
                RedisClientManager.AddItemToSet(setId, value, hours);
            }
        }

        /// <summary>
        /// 批量添加集合到HashSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setId"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        public static void AddRangeToSet(string setId, List<string> values, int hours = -1)
        {
            if (CacheKeys.HasNeedCompress(setId))
            {
                List<string> list = new List<string>();
                foreach (var item in values)
                {
                    string str = GzipString.Compress(item);
                    list.Add(str);
                }
                RedisClientManager.AddRangeToSet(setId, list, hours);
            }
            else
            {
                RedisClientManager.AddRangeToSet(setId, values, hours);
            }
        }

        /// <summary>
        /// 获取HashSet集合
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllItemsFromSet(string setId)
        {
            if (CacheKeys.HasNeedCompress(setId))
            {
                HashSet<string> hashSet = new HashSet<string>();
                var values = RedisClientManager.GetAllItemsFromSet(setId);
                foreach (var item in values)
                {
                    string str = GzipString.Decompress(item);
                    if (string.IsNullOrWhiteSpace(str))
                        continue;
                    hashSet.Add(str);
                }
                return hashSet;
            }
            else
            {
                return RedisClientManager.GetAllItemsFromSet(setId);
            }
        }
        /// <summary>
        /// 在HashSet中删除单个实体
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="item"></param>
        public static void RemoveItemFromSet(string setId, string item)
        {
            RedisClientManager.RemoveItemFromSet(setId, item);
        }
        #endregion

        /// <summary>
        /// 入列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Data"></param>
        public static void WriteRedis(string key, string Data)
        {
            RedisClientManager.WriteRedis(key, Data);
        }
        /// <summary>
        /// 出列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadRedis(string key)
        {
            return RedisClientManager.ReadRedis(key);
        }
        public static IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            IDictionary<string, T> result = new Dictionary<string, T>();
            bool flag = false;
            if (keys.Any())
                flag = CacheKeys.HasNeedCompress(keys.First());
            if (flag)
            {
                IDictionary<string, string> values = RedisClientManager.GetAll<string>(keys);
                foreach (var item in values)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        string str = GzipString.Decompress(item.Value);
                        var value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
                        result.Add(item.Key, value);
                    }
                }
            }
            else
            {
                return RedisClientManager.GetAll<T>(keys);
            }
            return result;
        }
        /// <summary>
        /// 批量存入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="expiresIn"></param>
        public static void SetAll<T>(IDictionary<string, T> values, TimeSpan? expiresIn = null)
        {
            bool flag = CacheKeys.HasNeedCompress(values.Keys.First());
            if (flag)
            {
                IDictionary<string, string> result = new Dictionary<string, string>();
                foreach (var item in values)
                {
                    if (item.Value != null)
                    {
                        string value = Newtonsoft.Json.JsonConvert.SerializeObject(item.Value);
                        string str = GzipString.Compress(value);
                        result.Add(item.Key, str);
                    }
                }
                RedisClientManager.SetAll(result);
            }
            else
            {
                RedisClientManager.SetAll<T>(values);
            }
        }
        /// <summary>
        /// 查找匹配的队列id
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static List<string> SearchKeys(string pattern)
        {
            return RedisClientManager.SearchKeys(pattern);
        }
        /// <summary>
        /// 获取所有队列key
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllKeys()
        {
            return RedisClientManager.GetAllKeys();
        }
        /// <summary>
        /// 是否包含某个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            return RedisClientManager.ContainsKey(key);
        }
        /// <summary>        
        /// 判断key是否存在，如果不存在则插入，返回true；如果存在，则返回false；
        /// 利用AcquireLock全局锁处理并发。
        /// </summary>
        /// <typeparam name="T">设置的值的类型</typeparam>
        /// <param name="lockKey">全局锁的名称</param>
        /// <param name="lockTimeOut">获取锁的超时时间</param>
        /// <param name="key">设置的值得key</param>
        /// <param name="t">设置的值</param>
        /// <param name="cacheTimeOut">设置的值，过期时间</param>
        /// <returns></returns>
        public static bool MySetNX<T>(string lockKey, TimeSpan lockTimeOut, string key, T t, TimeSpan cacheTimeOut)
        {
            return RedisClientManager.MySetNX<T>(lockKey, lockTimeOut, key, t, cacheTimeOut);
        }

        public static object LockObj = new object();
    }

    /// <summary>
    /// 缓存过去时间
    /// </summary>
    public static class CacheTimeOut
    {
        /// <summary>
        /// 三秒
        /// </summary>
        public static TimeSpan ThreeSecond = new TimeSpan(0, 0, 3);
        /// <summary>
        /// 五秒
        /// </summary>
        public static TimeSpan FiveSecond = new TimeSpan(0, 0, 5);
        /// <summary>
        /// 十秒
        /// </summary>
        public static TimeSpan TenSecond = new TimeSpan(0, 0, 10);
        /// <summary>
        /// 二十秒
        /// </summary>
        public static TimeSpan TwentySecond = new TimeSpan(0, 0, 20);
        /// <summary>
        /// 三十秒
        /// </summary>
        public static TimeSpan ThirtySecond = new TimeSpan(0, 0, 30);
        /// <summary>
        /// 一分钟
        /// </summary>
        public static TimeSpan OneMinute = new TimeSpan(0, 1, 0);
        /// <summary>
        /// 两分钟
        /// </summary>
        public static TimeSpan TwoMinute = new TimeSpan(0, 2, 0);
        /// <summary>
        /// 三分钟
        /// </summary>
        public static TimeSpan ThreeMinute = new TimeSpan(0, 3, 0);
        /// <summary>
        /// 五分钟
        /// </summary>
        public static TimeSpan FiveMinute = new TimeSpan(0, 5, 0);
        /// <summary>
        /// 十分钟
        /// </summary>
        public static TimeSpan TenMinute = new TimeSpan(0, 10, 0);
        /// <summary>
        /// 二十分钟
        /// </summary>
        public static TimeSpan TwentyMinute = new TimeSpan(0, 20, 0);
        /// <summary>
        /// 三十分钟
        /// </summary>
        public static TimeSpan ThirtyMinute = new TimeSpan(0, 30, 0);
        /// <summary>
        /// 一小时
        /// </summary>
        public static TimeSpan OneHour = new TimeSpan(1, 0, 0);
        /// <summary>
        /// 两小时
        /// </summary>
        public static TimeSpan TwoHour = new TimeSpan(2, 0, 0);
        /// <summary>
        /// 三小时
        /// </summary>
        public static TimeSpan ThreeHour = new TimeSpan(3, 0, 0);
        /// <summary>
        /// 五小时
        /// </summary>
        public static TimeSpan FiveHour = new TimeSpan(5, 0, 0);
        /// <summary>
        /// 十小时
        /// </summary>
        public static TimeSpan TenHour = new TimeSpan(10, 0, 0);
        /// <summary>
        /// 二十小时
        /// </summary>
        public static TimeSpan TwentyHour = new TimeSpan(20, 0, 0);
    }
}
