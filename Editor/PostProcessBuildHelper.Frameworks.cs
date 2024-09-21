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
        /// 设置文件编译标记
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="targetGuid"></param>
        /// <param name="xcodeConfigChange"></param>
        private static void SetFrameworks(PBXProject proj, string targetGuid, XcodeConfigChange xcodeConfigChange)
        {
            foreach (var name in xcodeConfigChange.add)
            {
                proj.AddFrameworkToProject(targetGuid, name, false);
            }

            foreach (var name in xcodeConfigChange.remove)
            {
                proj.RemoveFrameworkFromProject(targetGuid, name);
            }
        }
    }
}
#endif