#if UNITY_IOS
using System;
using System.Collections;
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
        /// <param name="proj">PBXProject 对象</param>
        /// <param name="targetGuid">目标 GUID</param>
        /// <param name="hashtable">配置数据，Key为文件路径，Value为编译标记字符串（空格分隔）</param>
        private static void SetFilesCompileFlag(PBXProject proj, string targetGuid, Hashtable hashtable)
        {
            if (hashtable == null)
            {
                return;
            }

            foreach (DictionaryEntry map in hashtable)
            {
                string fileProjPath = map.Key.ToString();
                string fileGuid = proj.FindFileGuidByProjectPath(fileProjPath);
                var flags = map.Value.ToString().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                proj.SetCompileFlagsForFile(targetGuid, fileGuid, flags);
            }
        }
    }
}
#endif
