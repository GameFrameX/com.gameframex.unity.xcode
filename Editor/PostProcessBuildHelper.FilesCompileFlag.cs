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
        /// <param name="list"></param>
        private static void SetFilesCompileFlag(PBXProject proj, string targetGuid, List<XcodeConfigMap> list)
        {
            foreach (var map in list)
            {
                string fileProjPath = map.key;
                string fileGuid = proj.FindFileGuidByProjectPath(fileProjPath);
                var flags = map.value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                proj.SetCompileFlagsForFile(targetGuid, fileGuid, flags);
            }
        }
    }
}
#endif