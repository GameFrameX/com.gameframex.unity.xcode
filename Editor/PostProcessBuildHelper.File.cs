#if UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        //复制文件
        private static void RunCopyFiles(PBXProject proj, string targetGuid, string xcodePath, Hashtable hashtable)
        {
            foreach (DictionaryEntry map in hashtable)
            {
                string src = Path.Combine(Environment.CurrentDirectory, map.Key.ToString());
                string des = Path.Combine(xcodePath, map.Value.ToString());
                CopyFile(proj, targetGuid, xcodePath, src, des);
            }
        }

        private static void CopyFile(PBXProject proj, string targetGuid, string xcodePath, string src, string des)
        {
            bool needCopy = IsNeedCopy(src);
            if (needCopy)
            {
                File.Copy(src, des);
                proj.AddFileToBuild(targetGuid, proj.AddFile(des, des.Replace(xcodePath + "/", ""), PBXSourceTree.Absolute));
                AutoAddSearchPath(proj, xcodePath, targetGuid, des);
                Debug.Log("copy file " + src + " -> " + des);
            }
        }

        /// <summary>
        /// 在复制文件加入工程时，当文件中有framework、h、a文件时，自动添加相应的搜索路径
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="xcodePath"></param>
        /// <param name="targetGuid"></param>
        /// <param name="filePath"></param>
        private static void AutoAddSearchPath(PBXProject proj, string xcodePath, string targetGuid, string filePath)
        {
            if (filePath.EndsWith(".framework"))
            {
                //添加框架搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, string.Empty));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("FRAMEWORK_SEARCH_PATHS", array);
                SetBuildProperties(proj, targetGuid, arg);
            }
            else if (filePath.EndsWith(".h"))
            {
                //添加头文件搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, string.Empty));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("HEADER_SEARCH_PATHS", array);
                SetBuildProperties(proj, targetGuid, arg);
            }
            else if (filePath.EndsWith(".a"))
            {
                //添加静态库搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, string.Empty));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("LIBRARY_SEARCH_PATHS", array);
                SetBuildProperties(proj, targetGuid, arg);
            }
        }

        private static bool IsNeedCopy(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            if (fileName.StartsWith(".") || file.EndsWith(".gitkeep") || file.EndsWith(".DS_Store"))
            {
                return false;
            }

            return true;
        }
    }
}
#endif