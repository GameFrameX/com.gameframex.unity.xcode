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
                proj.AddBuildProperty(targetGuid, kv.Key.ToString().Trim(), kv.Value.ToString().Trim());
            }
        }
    }
}
#endif