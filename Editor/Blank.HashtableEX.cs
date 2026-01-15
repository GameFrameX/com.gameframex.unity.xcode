#if UNITY_IOS
using UnityEngine;
using System.Collections;

namespace GameFrameX.Xcode.Editor
{
    public static class HashtableEX
    {
        /// <summary>
        /// 获取值
        /// </summary>
        public static object Get(this Hashtable inst, object key)
        {
            if (inst == null)
            {
                Debug.Log("hashtable is null");
                return null;
            }

            if (key == null || ReferenceEquals(key, ""))
            {
                return null;
            }
            else if (inst.ContainsKey(key))
                return inst[key];
            else
                return null;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        public static void SSet(this Hashtable inst, object key, object value)
        {
            if (inst == null)
            {
                //Debug.Log("hashtable is null");
                return;
            }

            if (key == null || ReferenceEquals(key, ""))
            {
                return;
            }
            else if (inst.ContainsKey(key))
                inst[key] = value;
            else
                inst.Add(key, value);
        }

        /// <summary>
        /// 获取泛型值
        /// </summary>
        public static T Get<T>(this Hashtable inst, object key)
        {
            if (inst == null)
            {
                //Debug.Log("hashtable is null");
                return default(T);
            }

            if (key == null || ReferenceEquals(key, ""))
            {
                return default(T);
            }
            else if (inst.ContainsKey(key) && inst[key] != null)
            {
                if (inst[key] is T)
                {
                    return (T)inst[key];
                }
                else
                {
                    return default(T);
                }
            }
            else
                return default(T);
        }

        /// <summary>
        /// 构造Hashtable
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Hashtable Construct(params object[] p)
        {
            var inst = new Hashtable();
            if (p != null)
            {
                for (int i = 0; i < p.Length;)
                {
                    inst.Add(p[i], p[i + 1]);
                    i = i + 2;
                }
            }

            return inst;
        }

        public static bool UpdateByKey<T>(this Hashtable inst, object key, ref T v)
        {
            if (inst == null)
            {
                return false;
            }

            if (key == null || ReferenceEquals(key, ""))
            {
                return false;
            }
            else if (inst.ContainsKey(key) && inst[key] != null)
            {
                if (inst[key].GetType() == typeof(T))
                {
                    v = (T)inst[key];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 深度合并 Hashtable
        /// </summary>
        /// <param name="target">目标 Hashtable</param>
        /// <param name="source">源 Hashtable</param>
        public static void Merge(this Hashtable target, Hashtable source)
        {
            if (target == null || source == null)
            {
                return;
            }

            foreach (DictionaryEntry entry in source)
            {
                var key = entry.Key;
                var value = entry.Value;

                if (target.ContainsKey(key))
                {
                    var targetValue = target[key];

                    if (targetValue is Hashtable targetTable && value is Hashtable sourceTable)
                    {
                        // 递归合并 Hashtable
                        targetTable.Merge(sourceTable);
                    }
                    else if (targetValue is ArrayList targetList && value is ArrayList sourceList)
                    {
                        // 合并 ArrayList，追加并去重
                        foreach (var item in sourceList)
                        {
                            if (!targetList.Contains(item))
                            {
                                targetList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        // 其他类型直接覆盖
                        target[key] = value;
                    }
                }
                else
                {
                    // 如果目标不存在该键，根据类型决定是否需要克隆
                    if (value is Hashtable sourceTable)
                    {
                        // 创建新的 Hashtable 并深度复制内容，避免引用污染
                        // 这里为了简化，我们调用 Clone。虽然 Hashtable.Clone 是浅拷贝，但对于我们的一层结构合并足够。
                        // 如果需要完全深度克隆，需要另外实现。但在合并配置的场景下，
                        // 通常我们是将多个配置合并到一个新的空配置中，或者合并到一个已有的配置中。
                        // 如果直接赋值引用，后续修改 target[key] 会影响 sourceTable。
                        // 在当前场景下，source 是从 JSON 解析出来的临时对象，所以直接赋值引用通常是安全的。
                        // 但为了保险起见，可以手动复制。
                        // 考虑到 JSON 解析出的 Hashtable 包含的也是基本类型或 ArrayList/Hashtable。
                        
                        // 简单处理：直接赋值。因为 source 通常是一次性的。
                        target[key] = value; 
                    }
                    else if (value is ArrayList sourceList)
                    {
                        // ArrayList 最好克隆一份，因为后续可能会修改这个 List
                        target[key] = sourceList.Clone();
                    }
                    else
                    {
                        target[key] = value;
                    }
                }
            }
        }
    }
}
#endif
