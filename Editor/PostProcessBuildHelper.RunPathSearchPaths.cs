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
        /// <param name="proj"></param>
        /// <param name="targetGuid"></param>
        /// <param name="table"></param>
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