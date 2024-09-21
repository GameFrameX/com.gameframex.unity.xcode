#if UNITY_IOS
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameFrameX.Xcode.Editor
{
    public sealed class XcodeConfigProcessorHelper
    {
        const string ConfigPath = "Assets/XcodeConfig/Editor/XcodeConfig.json";
        private static string targetGuid;

        /*[PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                try
                {
                    Run(path);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }*/

        private static void Run(string path)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            var proj = new PBXProject();
            Debug.Log(JsonConvert.SerializeObject(proj));
            Debug.Log(projPath);
            // PBXProject.
            targetGuid = proj.GetUnityFrameworkTargetGuid();

            proj.ReadFromString(File.ReadAllText(projPath));
            //读取配置文件
            string json = File.ReadAllText(ConfigPath);
            Hashtable table = json.HashtableFromJson();

            //lib
            SetLibs(proj, table.SGet<Hashtable>("libs"));
            //framework
            SetFrameworks(proj, table.SGet<Hashtable>("frameworks"));
            //building setting
            SetBuildProperties(proj, table.SGet<Hashtable>("properties"));
            //复制文件
            CopyFiles(proj, path, table.SGet<Hashtable>("files"));
            //复制文件夹
            CopyFolders(proj, path, table.SGet<Hashtable>("folders"));
            //文件编译符号
            SetFilesCompileFlag(proj, table.SGet<Hashtable>("filesCompileFlag"));
            // Linker Flag
            AddOtherLinkFlag(proj, table.SGet<Hashtable>("otherLinkerFlag"));
            //写入
            File.WriteAllText(projPath, proj.WriteToString());


            //plist
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;

            // SetPlist(proj, rootDict, table.SGet<Hashtable>("plist"));
            //写入
            plist.WriteToFile(plistPath);
        }

        private static void AddLibToProject(PBXProject inst, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }

        private static void RemoveLibFromProject(PBXProject inst, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.RemoveFileFromBuild(targetGuid, fileGuid);
        }

        //设置frameworks
        private static void SetFrameworks(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                if (table["+"] is ArrayList addList)
                {
                    foreach (string i in addList)
                    {
                        proj.AddFrameworkToProject(targetGuid, i, false);
                    }
                }

                if (table["-"] is ArrayList removeList)
                {
                    foreach (string i in removeList)
                    {
                        proj.RemoveFrameworkFromProject(targetGuid, i);
                    }
                }
            }
        }

        //设置libs
        private static void SetLibs(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                if (table["+"] is ArrayList addList)
                {
                    foreach (string i in addList)
                    {
                        AddLibToProject(proj, i);
                    }
                }

                if (table["-"] is ArrayList removeList)
                {
                    foreach (string i in removeList)
                    {
                        RemoveLibFromProject(proj, i);
                    }
                }
            }
        }

        //设置编译属性
        private static void SetBuildProperties(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                Hashtable setTable = table.SGet<Hashtable>("=");
                foreach (DictionaryEntry i in setTable)
                {
                    proj.SetBuildProperty(targetGuid, i.Key.ToString(), i.Value.ToString());
                }

                Hashtable addTable = table.SGet<Hashtable>("+");
                foreach (DictionaryEntry i in addTable)
                {
                    ArrayList array = i.Value as ArrayList;
                    List<string> list = new List<string>();
                    if (array != null)
                    {
                        foreach (var flag in array)
                        {
                            list.Add(flag.ToString());
                        }
                    }

                    proj.UpdateBuildProperty(targetGuid, i.Key.ToString(), list, null);
                }

                Hashtable removeTable = table.SGet<Hashtable>("-");
                foreach (DictionaryEntry i in removeTable)
                {
                    ArrayList array = i.Value as ArrayList;
                    List<string> list = new List<string>();
                    if (array != null)
                    {
                        foreach (var flag in array)
                        {
                            list.Add(flag.ToString());
                        }
                    }

                    proj.UpdateBuildProperty(targetGuid, i.Key.ToString(), null, list);
                }
            }
        }


        //复制文件
        private static void CopyFiles(PBXProject proj, string xcodePath, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                string src = Path.Combine(System.Environment.CurrentDirectory, i.Key.ToString());
                string des = Path.Combine(xcodePath, i.Value.ToString());
                CopyFile(proj, xcodePath, src, des);
            }
        }

        //复制文件夹
        private static void CopyFolders(PBXProject proj, string xcodePath, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                string src = Path.Combine(System.Environment.CurrentDirectory, i.Key.ToString());
                string des = Path.Combine(xcodePath, i.Value.ToString());
                CopyFolder(src, des);
                AddFolderBuild(proj, xcodePath, i.Value.ToString());
            }
        }

        private static void CopyFile(PBXProject proj, string xcodePath, string src, string des)
        {
            bool needCopy = IsNeedCopy(src);
            if (needCopy)
            {
                File.Copy(src, des);
                proj.AddFileToBuild(targetGuid, proj.AddFile(des, des.Replace(xcodePath + "/", ""), PBXSourceTree.Absolute));
                AutoAddSearchPath(proj, xcodePath, des);
                Debug.Log("copy file " + src + " -> " + des);
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

        private static void AddFolderBuild(PBXProject proj, string xcodePath, string root)
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
                    AutoAddSearchPath(proj, xcodePath, filePath);
                }
                else
                {
                    AddFolderBuild(proj, xcodePath, projectPath);
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
                    AutoAddSearchPath(proj, xcodePath, filePath);
                    Debug.Log("add file to build:" + Path.Combine(root, file));
                }
            }
        }

        /// <summary>
        /// 添加其他链接标记
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="table"></param>
        private static void AddOtherLinkFlag(PBXProject proj, Hashtable table)
        {
            foreach (DictionaryEntry kv in table)
            {
                proj.AddBuildProperty(targetGuid, kv.Key.ToString(), kv.Value.ToString());
            }
        }

        /// <summary>
        /// 在复制文件加入工程时，当文件中有framework、h、a文件时，自动添加相应的搜索路径
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="xcodePath"></param>
        /// <param name="filePath"></param>
        private static void AutoAddSearchPath(PBXProject proj, string xcodePath, string filePath)
        {
            if (filePath.EndsWith(".framework"))
            {
                //添加框架搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("FRAMEWORK_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
            else if (filePath.EndsWith(".h"))
            {
                //添加头文件搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("HEADER_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
            else if (filePath.EndsWith(".a"))
            {
                //添加静态库搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("LIBRARY_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
        }

        /// <summary>
        /// 设置文件编译标记
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="arg"></param>
        private static void SetFilesCompileFlag(PBXProject proj, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                string fileProjPath = i.Key.ToString();
                string fileGuid = proj.FindFileGuidByProjectPath(fileProjPath);
                ArrayList flags = i.Value as ArrayList;
                List<string> list = new List<string>();
                foreach (var flag in flags)
                {
                    list.Add(flag.ToString());
                }

                proj.SetCompileFlagsForFile(targetGuid, fileGuid, list);
            }
        }

        private static bool IsNeedCopy(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileEx = Path.GetExtension(file);
            if (fileName.StartsWith(".") || file.EndsWith(".gitkeep") || file.EndsWith(".DS_Store"))
            {
                return false;
            }

            return true;
        }
    }
}
#endif