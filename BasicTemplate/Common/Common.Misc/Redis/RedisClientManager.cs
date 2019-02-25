using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public class RedisClientManager
    {
        private static int hoursTimeOut = 12;
        private static int _maxWritePoolSize = 5;
        private static int _maxReadPoolSize = 5;
        private static PooledRedisClientManager Client;

        private static PooledRedisClientManager CreateManager(
          string[] readWriteHosts,
          string[] readOnlyHosts)
        {
            return new PooledRedisClientManager((IEnumerable<string>)readWriteHosts, (IEnumerable<string>)readOnlyHosts, new RedisClientManagerConfig()
            {
                MaxWritePoolSize = RedisClientManager._maxWritePoolSize,
                MaxReadPoolSize = RedisClientManager._maxReadPoolSize,
                AutoStart = true
            });
        }

        /// <summary>初始化Redis</summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        public static bool InitRedis(
          string[] readWriteHosts,
          string[] readOnlyHosts,
          int maxWritePoolSize,
          int maxReadPoolSize)
        {
            try
            {
                if (maxWritePoolSize > 0)
                    RedisClientManager._maxWritePoolSize = maxWritePoolSize;
                if (maxReadPoolSize > 0)
                    RedisClientManager._maxReadPoolSize = maxReadPoolSize;
                RedisClientManager.Client = RedisClientManager.CreateManager(readWriteHosts, readOnlyHosts);
                RedisClientManager.Client.PoolTimeout = new int?(5000);
            }
            catch (Exception ex)
            {
                ex.Error("缓存服务器错误!");
                return false;
            }
            return true;
        }

        /// <summary>出队</summary>
        /// <returns></returns>
        public static string ReadRedis(string key)
        {
            try
            {
                using (IRedisClient client = RedisClientManager.Client.GetClient())
                    return client.Lists[key].Dequeue();
            }
            catch (Exception ex)
            {
                ex.Error("缓存服务器错误!");
                return "Error:" + ex.Message;
            }
        }

        /// <summary>入队</summary>
        /// <param name="Data"></param>
        public static void WriteRedis(string key, string Data)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                client.Lists[key].Enqueue(Data);
        }

        /// <summary>队列元素个数</summary>
        /// <returns></returns>
        public static long Count(string key)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetListCount(key);
        }

        public static void FlushAll()
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                client.FlushAll();
        }

        /// <summary>设置缓存</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存建</param>
        /// <param name="t">缓存值</param>
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T t, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                if (hours < 0)
                    return client.Set<T>(key, t);
                if (hours == 0)
                    hours = RedisClientManager.hoursTimeOut;
                return client.Set<T>(key, t, new TimeSpan(hours, 0, 0));
            }
        }

        public static bool Set<T>(string key, T t, TimeSpan timeOut)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.Set<T>(key, t, timeOut);
        }

        /// <summary>获取</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            T obj = default(T);
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
            {
                if (readOnlyClient.ContainsKey(key))
                    obj = readOnlyClient.Get<T>(key);
            }
            return obj;
        }

        /// <summary>删除</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.Remove(key);
        }

        /// <summary>批量删除</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RemoveAll(IEnumerable<string> keys)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                client.RemoveAll(keys);
        }

        public static bool Add<T>(string key, T t, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                if (hours < 0)
                    return client.Add<T>(key, t);
                if (hours == 0)
                    hours = RedisClientManager.hoursTimeOut;
                return client.Add<T>(key, t, new TimeSpan(hours, 0, 0));
            }
        }

        /// <summary>获取有序集合中的所有元素KEY.</summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static List<string> GetAllItemsFromSortedSet(string setId)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.GetAllItemsFromSortedSet(setId);
        }

        public static double GetItemVauleByKey(string setId, string key)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.GetItemScoreInSortedSet(setId, key);
        }

        /// <summary>获取有序集合中的数量</summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string setId)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.GetSortedSetCount(setId);
        }

        /// <summary>获取集合数据.(正序)</summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetByAsc(
          string setId,
          int minValue,
          int maxValue)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.GetRangeWithScoresFromSortedSetByLowestScore(setId, (long)minValue, (long)maxValue);
        }

        /// <summary>获取集合数据。（倒叙）</summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetByDesc(
          string setId,
          int minValue,
          int maxValue)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.GetRangeWithScoresFromSortedSetByHighestScore(setId, (long)minValue, (long)maxValue);
        }

        /// <summary>将元素加入有序集合</summary>
        /// <param name="setId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddItemToSortedSet(string setId, string key, double value)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.AddItemToSortedSet(setId, key, value);
        }

        /// <summary>获取有序集合中的数量 (根据值范围)</summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string setId, double minValue, double maxValue)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.GetSortedSetCount(setId, minValue, maxValue);
        }

        /// <summary>移除有序集合的元素.</summary>
        /// <param name="setId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveItemFromSortedSet(string setId, string key)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.RemoveItemFromSortedSet(setId, key);
        }

        /// <summary>移除有序集合的元素 (根据值范围)</summary>
        /// <param name="setId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static long RemoveSortedSetByRange(string setId, double minValue, double maxValue)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                return client.RemoveRangeFromSortedSetByScore(setId, minValue, maxValue);
        }

        /// <summary>根据IEnumerable数据添加链表</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        public static void AddList<T>(string listId, IEnumerable<T> values, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = client.As<T>();
                if (hours >= 0)
                {
                    if (hours == 0)
                        hours = RedisClientManager.hoursTimeOut;
                    client.ExpireEntryIn(listId, new TimeSpan(hours, 0, 0));
                }
                redisTypedClient.Lists[listId].AddRange(values);
                redisTypedClient.Save();
            }
        }

        /// <summary>根据List数据添加链表</summary>
        /// <param name="listId"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        public static void AddRangeToList(string listId, List<string> values, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                if (hours >= 0)
                {
                    if (hours == 0)
                        hours = RedisClientManager.hoursTimeOut;
                    client.ExpireEntryIn(listId, new TimeSpan(hours, 0, 0));
                }
                client.AddRangeToList(listId, values);
            }
        }

        /// <summary>添加单个对象到List</summary>
        /// <param name="listId"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        public static void AddItemToList(string listId, string value, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                if (hours >= 0)
                {
                    if (hours == 0)
                        hours = RedisClientManager.hoursTimeOut;
                    client.ExpireEntryIn(listId, new TimeSpan(hours, 0, 0));
                }
                client.AddItemToList(listId, value);
            }
        }

        /// <summary>获取链表集合</summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static List<string> GetAllItemsFromList(string listId)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetAllItemsFromList(listId);
        }

        /// <summary>获取链表中指定的对象</summary>
        /// <param name="listId"></param>
        /// <param name="listIndex"></param>
        /// <returns></returns>
        public static string GetItemFromList(string listId, int listIndex)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetItemFromList(listId, listIndex);
        }

        public static long GetListCount(string listId)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetListCount(listId);
        }

        /// <summary>清除链表</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        public static void RemoveList<T>(string listId)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = client.As<T>();
                IRedisList<T> list = redisTypedClient.Lists[listId];
                redisTypedClient.RemoveAllFromList(list);
                redisTypedClient.Save();
            }
        }

        /// <summary>添加单个实体到链表中</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="Item"></param>
        /// <param name="hours"></param>
        public static void AddEntityToList<T>(string listId, T Item, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = client.As<T>();
                if (hours >= 0)
                {
                    if (hours == 0)
                        hours = RedisClientManager.hoursTimeOut;
                    client.ExpireEntryIn(listId, new TimeSpan(hours, 0, 0));
                }
                redisTypedClient.Lists[listId].Add(Item);
                redisTypedClient.Save();
            }
        }

        /// <summary>获取链表</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetList<T>(string listId)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
            {
                IRedisTypedClient<T> redisTypedClient = readOnlyClient.As<T>();
                {

                    return (IEnumerable<T>)redisTypedClient.Lists[listId].GetAll();
                }
            }
        }

        /// <summary>在链表中删除单个实体</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="t"></param>
        public static void RemoveEntityFromList<T>(string listId, T t)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = client.As<T>();
                {
                    redisTypedClient.Lists[listId].RemoveValue(t);
                    redisTypedClient.Save();
                }
            }
        }

        /// <summary>根据lambada表达式删除符合条件的实体</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> SearchFromList<T>(string listId, Func<T, bool> func)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
            {
                IRedisTypedClient<T> redisTypedClient = readOnlyClient.As<T>();
                return redisTypedClient.Lists[listId].Where<T>(func);
            }
        }

        /// <summary>根据lambada表达式删除符合条件的实体</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="func"></param>
        public static void RemoveEntityFromList<T>(string listId, Func<T, bool> func)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = client.As<T>();
                {
                    IRedisList<T> list = redisTypedClient.Lists[listId];
                    T obj = list.Where<T>(func).FirstOrDefault<T>();
                    list.RemoveValue(obj);
                    redisTypedClient.Save();
                }
            }
        }

        /// <summary>删除并返回链表中第一个元素</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        public static T RemoveStartFromList<T>(string listId)
        {
            T obj = default(T);
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                IRedisTypedClient<T> redisTypedClient = client.As<T>();
                {
                    IRedisList<T> list = redisTypedClient.Lists[listId];
                    obj = redisTypedClient.RemoveStartFromList(list);
                    redisTypedClient.Save();
                }
            }
            return obj;
        }

        /// <summary>添加单个对象到HashSet</summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        public static void AddItemToSet(string setId, string value, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                if (hours >= 0)
                {
                    if (hours == 0)
                        hours = RedisClientManager.hoursTimeOut;
                    client.ExpireEntryIn(setId, new TimeSpan(hours, 0, 0));
                }
                client.AddItemToSet(setId, value);
            }
        }

        /// <summary>批量添加集合到HashSet</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setId"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        public static void AddRangeToSet(string setId, List<string> values, int hours = -1)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                if (hours >= 0)
                {
                    if (hours == 0)
                        hours = RedisClientManager.hoursTimeOut;
                    client.ExpireEntryIn(setId, new TimeSpan(hours, 0, 0));
                }
                client.AddRangeToSet(setId, values);
            }
        }

        /// <summary>获取HashSet集合</summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllItemsFromSet(string setId)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetAllItemsFromSet(setId);
        }

        /// <summary>在HashSet中删除单个实体</summary>
        /// <param name="setId"></param>
        /// <param name="item"></param>
        public static void RemoveItemFromSet(string setId, string item)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
                client.RemoveItemFromSet(setId, item);
        }

        /// <summary>获取所有列表</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetAll<T>(keys);
        }

        /// <summary>批量存入</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="expiresIn"></param>
        public static void SetAll<T>(IDictionary<string, T> values, TimeSpan? expiresIn = null)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                using (IRedisPipeline pipeline = client.CreatePipeline())
                {
                    foreach (KeyValuePair<string, T> keyValuePair in (IEnumerable<KeyValuePair<string, T>>)values)
                    {
                        KeyValuePair<string, T> item = keyValuePair;
                        if (!expiresIn.HasValue)
                            pipeline.QueueCommand((Func<IRedisClient, bool>)(r => r.Set<T>(item.Key, item.Value)));
                        else
                            pipeline.QueueCommand((Func<IRedisClient, bool>)(r => r.Set<T>(item.Key, item.Value, expiresIn.Value)));
                    }
                    pipeline.Flush();
                }
            }
        }

        /// <summary>查找匹配的队列id</summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static List<string> SearchKeys(string pattern)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.SearchKeys(pattern);
        }

        /// <summary>获取所有队列key</summary>
        /// <returns></returns>
        public static List<string> GetAllKeys()
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.GetAllKeys();
        }

        /// <summary>是否包含某个key</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            using (IRedisClient readOnlyClient = RedisClientManager.Client.GetReadOnlyClient())
                return readOnlyClient.ContainsKey(key);
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
        public static bool MySetNX<T>(
          string lockKey,
          TimeSpan lockTimeOut,
          string key,
          T t,
          TimeSpan cacheTimeOut)
        {
            using (IRedisClient client = RedisClientManager.Client.GetClient())
            {
                using (client.AcquireLock(lockKey, lockTimeOut))
                {
                    if (!client.ContainsKey(key))
                        return client.Set<T>(key, t, cacheTimeOut);
                    return false;
                }
            }
        }
    }
}
