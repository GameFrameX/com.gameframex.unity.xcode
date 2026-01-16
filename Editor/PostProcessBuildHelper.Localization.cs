#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        private static void RunLocalization(PBXProject project, string targetGuid, string path, ArrayList localizations)
        {
            if (localizations == null)
            {
                return;
            }

            LogHelper.Log("Setting project [Localization] started");

            foreach (Hashtable loc in localizations)
            {
                if (!loc.ContainsKey("languageCode"))
                {
                    continue;
                }

                string langCode = loc["languageCode"].ToString();
                if (string.IsNullOrEmpty(langCode))
                {
                    continue;
                }

                ArrayList validMap = loc["validMap"] as ArrayList;
                if (validMap == null || validMap.Count == 0)
                {
                    continue;
                }

                // Create .lproj directory
                string lprojName = langCode + ".lproj";
                string lprojPath = Path.Combine(path, lprojName);
                if (!Directory.Exists(lprojPath))
                {
                    Directory.CreateDirectory(lprojPath);
                }

                // Prepare InfoPlist.strings content
                StringBuilder sb = new StringBuilder();
                foreach (Hashtable map in validMap)
                {
                    if (map.ContainsKey("key") && map.ContainsKey("value"))
                    {
                        string key = map["key"].ToString();
                        string value = map["value"].ToString();
                        sb.Append($"\"{key}\" = \"{value}\";\n");
                    }
                }

                if (sb.Length > 0)
                {
                    string fileName = "InfoPlist.strings";
                    string filePath = Path.Combine(lprojPath, fileName);
                    File.WriteAllText(filePath, sb.ToString());

                    // Add to project
                    string relativePath = Path.Combine(lprojName, fileName);
                    // AddFile parameters: relative path within the project folder (usually), path relative to source tree (SOURCE_ROOT is default)
                    // The path argument to this method is the build folder. 
                    // AddFile expects path relative to the project root (which is 'path').
                    string fileGuid = project.AddFile(relativePath, relativePath);
                    project.AddFileToBuild(targetGuid, fileGuid);
                    
                    LogHelper.Log($"Added Localization: {langCode}");
                }
            }

            LogHelper.Log("Setting project [Localization] finished");
        }
    }
}
#endif
