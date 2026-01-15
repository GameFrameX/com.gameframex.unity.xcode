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
        /// <param name="proj">PBX项目</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="xcodeConfigChange">Xcode配置变更</param>
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

        /// <summary>
        /// 将指定系统库添加到Xcode工程中
        /// </summary>
        /// <param name="inst">PBX项目实例</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="lib">需要添加的库文件名（如libz.tbd）</param>
        private static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }

        /// <summary>
        /// 从Xcode工程中移除指定的系统库
        /// </summary>
        /// <param name="inst">PBX项目实例</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="lib">需要移除的库文件名（如libz.tbd）</param>
        private static void RemoveLibFromProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.RemoveFileFromBuild(targetGuid, fileGuid);
        }
    }
}
#endif