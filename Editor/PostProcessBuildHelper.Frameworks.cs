#if UNITY_IOS
using System.Collections;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 设置框架
        /// </summary>
        /// <param name="proj">PBX项目</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="hashtable">框架配置哈希表</param>
        private static void SetFrameworks(PBXProject proj, string targetGuid, Hashtable hashtable)
        {
            if (hashtable != null)
            {
                if (hashtable["+"] is ArrayList addList)
                {
                    foreach (string framework in addList)
                    {
                        proj.AddFrameworkToProject(targetGuid, framework, false);
                    }
                }

                if (hashtable["-"] is ArrayList removeList)
                {
                    foreach (string framework in removeList)
                    {
                        proj.RemoveFrameworkFromProject(targetGuid, framework);
                    }
                }
            }
        }
    }
}
#endif