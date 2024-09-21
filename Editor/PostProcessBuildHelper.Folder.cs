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
        //复制文件夹
        private static void CopyFolders(PBXProject proj, string targetGuid, string xcodePath, Hashtable hashtable)
        {
            foreach (DictionaryEntry map in hashtable)
            {
                string src = Path.Combine(Environment.CurrentDirectory, map.Key.ToString().Trim());
                string des = Path.Combine(xcodePath, map.Value.ToString().Trim());
                CopyFolder(src, des);
                AddFolderBuild(proj, targetGuid, xcodePath, map.Value.ToString().Trim());
            }
        }

        private static void AddFolderBuild(PBXProject proj, string targetGuid, string xcodePath, string root)
        {
            //获得源文件下所有目录文件
            string currDir = Path.Combine(xcodePath, root);
            if (root.EndsWith(".framework") || root.EndsWith(".bundle"))
            {
                Debug.LogFormat("add framework or bundle to build:{0}->{1}", currDir, root);
                proj.AddFileToBuild(targetGuid, proj.AddFile(currDir, root, PBXSourceTree.Source));
                return;
            }

            List<string> folders = new List<string>(Directory.GetDirectories(currDir));
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string filePath = Path.Combine(currDir, name);
                string projectPath = Path.Combine(root, name);
                if (folder.EndsWith(".framework") || folder.EndsWith(".bundle"))
                {
                    Debug.LogFormat("add framework or bundle to build:{0}->{1}", filePath, projectPath);
                    proj.AddFileToBuild(targetGuid, proj.AddFile(filePath, projectPath, PBXSourceTree.Source));
                    AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                }
                else
                {
                    AddFolderBuild(proj, targetGuid, xcodePath, projectPath);
                }
            }

            List<string> files = new List<string>(Directory.GetFiles(currDir));
            foreach (string file in files)
            {
                if (IsNeedCopy(file))
                {
                    string name = Path.GetFileName(file);
                    string filePath = Path.Combine(currDir, name);
                    string projectPath = Path.Combine(root, name);
                    proj.AddFileToBuild(targetGuid, proj.AddFile(filePath, projectPath, PBXSourceTree.Source));
                    AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                    Debug.Log("add file to build:" + Path.Combine(root, file));
                }
            }
        }

        private static void CopyFolder(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
            {
                Directory.Delete(dstPath);
            }

            if (File.Exists(dstPath))
            {
                File.Delete(dstPath);
            }

            Directory.CreateDirectory(dstPath);

            foreach (var file in Directory.GetFiles(srcPath))
            {
                if (IsNeedCopy(Path.GetFileName(file)))
                {
                    File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
                }
            }

            foreach (var dir in Directory.GetDirectories(srcPath))
            {
                CopyFolder(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
            }
        }
    }
}
#endif