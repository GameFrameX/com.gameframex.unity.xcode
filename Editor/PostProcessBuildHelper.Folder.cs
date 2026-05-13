#if UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 复制文件夹到 Xcode 工程
        /// </summary>
        /// <param name="proj">PBXProject 对象</param>
        /// <param name="targetGuid">目标 GUID</param>
        /// <param name="xcodePath">Xcode 工程路径</param>
        /// <param name="hashtable">配置数据，Key为源路径，Value为目标路径</param>
        private static void CopyFolders(PBXProject proj, string targetGuid, string mainTargetGuid, string xcodePath, Hashtable hashtable)
        {
            if (hashtable == null)
            {
                return;
            }

            foreach (DictionaryEntry map in hashtable)
            {
                string src = Path.Combine(Environment.CurrentDirectory, map.Key.ToString().Trim());
                string des = Path.Combine(xcodePath, map.Value.ToString().Trim());
                CopyFolder(src, des);
                AddFolderBuild(proj, targetGuid, mainTargetGuid, xcodePath, map.Value.ToString().Trim());
            }
        }

        /// <summary>
        /// 将文件夹内容添加到构建列表
        /// </summary>
        /// <param name="proj">PBXProject 对象</param>
        /// <param name="targetGuid">目标 GUID</param>
        /// <param name="xcodePath">Xcode 工程路径</param>
        /// <param name="root">相对根目录</param>
        private static void AddFolderBuild(PBXProject proj, string targetGuid, string mainTargetGuid, string xcodePath, string root)
        {
            //获得源文件下所有目录文件
            string currDir = Path.Combine(xcodePath, root);
            if (root.EndsWith(".framework") || root.EndsWith(".bundle"))
            {
                Debug.LogFormat("add framework/bundle to build:{0}->{1}", currDir, root);
                string fileGuid = proj.AddFile(currDir, root, PBXSourceTree.Source);
                proj.AddFileToBuild(targetGuid, fileGuid);
                if (root.EndsWith(".bundle"))
                {
                    proj.AddFileToBuild(mainTargetGuid, fileGuid);
                }
                // 添加为 linked framework
                proj.AddFrameworkToProject(targetGuid, root, false);
                return;
            }

            if (root.EndsWith(".xcframework"))
            {
                Debug.LogFormat("add xcframework to build:{0}->{1}", currDir, root);
                string fileGuid = proj.AddFile(currDir, root, PBXSourceTree.Source);
                proj.AddFileToBuild(targetGuid, fileGuid);
                return;
            }

            if (root.EndsWith(".a"))
            {
                Debug.LogFormat("add static library to build:{0}->{1}", currDir, root);
                // 静态库添加到 Link Binary With Libraries 阶段
                string fileGuid = proj.AddFile(currDir, root, PBXSourceTree.Source);
                proj.AddFileToBuild(targetGuid, fileGuid);
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
                    Debug.LogFormat("add framework/bundle to build:{0}->{1}", filePath, projectPath);
                    string fileGuid = proj.AddFile(filePath, projectPath, PBXSourceTree.Source);
                    proj.AddFileToBuild(targetGuid, fileGuid);
                    if (folder.EndsWith(".bundle"))
                    {
                        proj.AddFileToBuild(mainTargetGuid, fileGuid);
                    }
                    else
                    {
                        proj.AddFrameworkToProject(targetGuid, name, false);
                    }
                    AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                }
                else if (folder.EndsWith(".xcframework"))
                {
                    Debug.LogFormat("add xcframework to build:{0}->{1}", filePath, projectPath);
                    string fileGuid = proj.AddFile(filePath, projectPath, PBXSourceTree.Source);
                    proj.AddFileToBuild(targetGuid, fileGuid);
                    AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                }
                else if (folder.EndsWith(".a"))
                {
                    Debug.LogFormat("add static library to build:{0}->{1}", filePath, projectPath);
                    string fileGuid = proj.AddFile(filePath, projectPath, PBXSourceTree.Source);
                    proj.AddFileToBuild(targetGuid, fileGuid);
                    AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                }
                else
                {
                    AddFolderBuild(proj, targetGuid, mainTargetGuid, xcodePath, projectPath);
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

                    if (file.EndsWith(".framework") || file.EndsWith(".bundle"))
                    {
                        Debug.LogFormat("add framework/bundle to build:{0}->{1}", filePath, projectPath);
                        string fileGuid = proj.AddFile(filePath, projectPath, PBXSourceTree.Source);
                        proj.AddFileToBuild(targetGuid, fileGuid);
                        if (file.EndsWith(".bundle"))
                        {
                            proj.AddFileToBuild(mainTargetGuid, fileGuid);
                        }
                        else
                        {
                            proj.AddFrameworkToProject(targetGuid, name, false);
                        }
                        AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                    }
                    else if (file.EndsWith(".xcframework"))
                    {
                        Debug.LogFormat("add xcframework to build:{0}->{1}", filePath, projectPath);
                        string fileGuid = proj.AddFile(filePath, projectPath, PBXSourceTree.Source);
                        proj.AddFileToBuild(targetGuid, fileGuid);
                        AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                    }
                    else if (file.EndsWith(".a"))
                    {
                        Debug.LogFormat("add static library to build:{0}->{1}", filePath, projectPath);
                        string fileGuid = proj.AddFile(filePath, projectPath, PBXSourceTree.Source);
                        proj.AddFileToBuild(targetGuid, fileGuid);
                        AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                    }
                    else
                    {
                        proj.AddFileToBuild(targetGuid, proj.AddFile(filePath, projectPath, PBXSourceTree.Source));
                        AutoAddSearchPath(proj, xcodePath, targetGuid, filePath);
                        Debug.Log("add file to build:" + Path.Combine(root, file));
                    }
                }
            }
        }

        /// <summary>
        /// 递归复制文件夹
        /// </summary>
        /// <param name="srcPath">源文件夹路径</param>
        /// <param name="dstPath">目标文件夹路径</param>
        private static void CopyFolder(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
            {
                Directory.Delete(dstPath, true);
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
