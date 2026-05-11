#if UNITY_IOS
using System.Collections;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 设置系统库（.tbd 静态库）
        /// </summary>
        /// <param name="proj">PBX项目</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="hashtable">库配置哈希表</param>
        private static void SetLibs(PBXProject proj, string targetGuid, Hashtable hashtable)
        {
            if (hashtable == null)
            {
                return;
            }

            if (hashtable["+"] is ArrayList addList)
            {
                foreach (string lib in addList)
                {
                    AddLibToProject(proj, targetGuid, lib);
                }
            }

            if (hashtable["-"] is ArrayList removeList)
            {
                foreach (string lib in removeList)
                {
                    RemoveLibFromProject(proj, targetGuid, lib);
                }
            }
        }

        /// <summary>
        /// 将指定系统库添加到Xcode工程中
        /// </summary>
        /// <param name="proj">PBX项目实例</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="lib">需要添加的库文件名（如libz.tbd）</param>
        private static void AddLibToProject(PBXProject proj, string targetGuid, string lib)
        {
            string fileGuid = proj.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            proj.AddFileToBuild(targetGuid, fileGuid);
        }

        /// <summary>
        /// 从Xcode工程中移除指定的系统库
        /// </summary>
        /// <param name="proj">PBX项目实例</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="lib">需要移除的库文件名（如libz.tbd）</param>
        private static void RemoveLibFromProject(PBXProject proj, string targetGuid, string lib)
        {
            string fileGuid = proj.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            proj.RemoveFileFromBuild(targetGuid, fileGuid);
        }
    }
}
#endif