#if UNITY_IOS
using System.Collections;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 添加运行搜索路径
        /// </summary>
        /// <param name="proj">PBX项目</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="table">运行搜索路径MAP</param>
        private static void AddRunPathSearchPaths(PBXProject proj, string targetGuid, Hashtable table)
        {
            if (table == null)
            {
                return;
            }

            foreach (DictionaryEntry kv in table)
            {
                var keyStr = kv.Key.ToString().Trim();
                if (kv.Value is ArrayList list)
                {
                    var parts = new System.Collections.Generic.List<string>(list.Count);
                    foreach (var item in list)
                    {
                        var s = item?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(s))
                        {
                            parts.Add(s);
                        }
                    }

                    proj.AddBuildProperty(targetGuid, keyStr, string.Join(" ", parts));
                }
                else
                {
                    proj.AddBuildProperty(targetGuid, keyStr, kv.Value.ToString().Trim());
                }
            }
        }
    }
}
#endif