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
        /// 设置构建属性
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="targetGuid"></param>
        /// <param name="list"></param>
        private static void SetBuildProperties(PBXProject proj, string targetGuid, List<XcodeConfigMap> list)
        {
            foreach (var map in list)
            {
                proj.SetBuildProperty(targetGuid, map.key, map.value);
            }
        }
    }
}
#endif