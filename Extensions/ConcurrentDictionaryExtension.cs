using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackLib
{
    public static class ConcurrentDictionaryExtension
    {
        public const int ErrorTryCountMax = 5;

        public static void Add<K, V>(this ConcurrentDictionary<K, V> dic, K key, V value)
        {
            if (dic == null) throw new ArgumentNullException(nameof(dic));

            if (!dic.TryAdd(key, value)) throw new ArgumentException($"已存在具有相同索引鍵的元素({key})。");
        }

        public static V Remove<K, V>(this ConcurrentDictionary<K, V> dic, K key)
        {
            if (dic == null) throw new ArgumentNullException(nameof(dic));

            int errorCount = 0; 
            while (true)
            {
                V value;
                if(dic.TryRemove(key, out value))
                {
                    return value;
                }
                else
                {
                    errorCount++;
                    if(errorCount >= ErrorTryCountMax)
                    {
                        throw new ArgumentException($"移除該索引鍵失敗({key})。");
                    }
                    else
                    {
                        SpinWait.SpinUntil(() => false, 20);
                    }
                }
            }
        }
    }
}
