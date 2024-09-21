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
        /// <param name="proj"></param>
        /// <param name="targetGuid"></param>
        /// <param name="hashtable"></param>
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