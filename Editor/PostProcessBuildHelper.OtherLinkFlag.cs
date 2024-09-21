#if UNITY_IOS
using System.Collections;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 添加其他链接标记
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="targetGuid"></param>
        /// <param name="table"></param>
        private static void AddOtherLinkFlag(PBXProject proj, string targetGuid, Hashtable table)
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