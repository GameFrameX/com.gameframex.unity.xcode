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
        private static void SetLibrary(PBXProject proj, string targetGuid, XcodeConfigChange xcodeConfigChange)
        {
            foreach (var name in xcodeConfigChange.add)
            {
                AddLibToProject(proj, targetGuid, name);
            }

            foreach (var name in xcodeConfigChange.remove)
            {
                RemoveLibFromProject(proj, targetGuid, name);
            }
        }

        private static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }

        private static void RemoveLibFromProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.RemoveFileFromBuild(targetGuid, fileGuid);
        }
    }
}
#endif