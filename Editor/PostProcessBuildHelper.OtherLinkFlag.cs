#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static void AddOtherLinkFlag(PBXProject proj, string targetGuid, List<XcodeConfigMap> table)
        {
            foreach (var kv in table)
            {
                proj.AddBuildProperty(targetGuid, kv.key, kv.value);
            }
        }
    }
}
#endif