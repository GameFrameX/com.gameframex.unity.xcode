#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        private static void RunPlist(string path, List<XcodeConfigPlist> xcodeConfigPlists)
        {
            //plist
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            foreach (var configPlist in xcodeConfigPlists)
            {
                
            }
            
            plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            plist.root.SetString("NSUserTrackingUsageDescription", "此标识符将用于向您推荐个性化广告");
            // SetPlist(proj, rootDict, table.SGet<Hashtable>("plist"));
            //写入
            plist.WriteToFile(plistPath);
            Debug.Log("设置项目[Info.plist]结束");
        }


        /// <summary>
        /// 设置plist
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="node"></param>
        /// <param name="arg"></param>
        private static void SetPlist(PBXProject proj, PlistElementDict node, Hashtable arg)
        {
            if (arg != null)
            {
                foreach (DictionaryEntry i in arg)
                {
                    string key = i.Key.ToString();
                    object val = i.Value;
                    var vType = i.Value.GetType();
                    if (vType == typeof(string))
                    {
                        node.SetString(key, (string)val);
                    }
                    else if (vType == typeof(bool))
                    {
                        node.SetBoolean(key, (bool)val);
                    }
                    else if (vType == typeof(double))
                    {
                        int v = int.Parse(val.ToString());
                        node.SetInteger(key, v);
                    }
                    else if (vType == typeof(ArrayList))
                    {
                        var t = node.CreateArray(key);
                        var array = val as ArrayList;
                        SetPlist(proj, t, array);
                    }
                    else if (vType == typeof(Hashtable))
                    {
                        var t = node.CreateDict(key);
                        var table = val as Hashtable;
                        SetPlist(proj, t, table);
                    }
                }
            }
        }

        private static void SetPlist(PBXProject proj, PlistElementArray node, ArrayList arg)
        {
            if (arg != null)
            {
                foreach (object i in arg)
                {
                    object val = i;
                    var vType = i.GetType();
                    if (vType == typeof(string))
                    {
                        node.AddString((string)val);
                    }
                    else if (vType == typeof(bool))
                    {
                        node.AddBoolean((bool)val);
                    }
                    else if (vType == typeof(double))
                    {
                        int v = int.Parse(val.ToString());
                        node.AddInteger(v);
                    }
                    else if (vType == typeof(ArrayList))
                    {
                        var t = node.AddArray();
                        var array = val as ArrayList;
                        SetPlist(proj, t, array);
                    }
                    else if (vType == typeof(Hashtable))
                    {
                        var t = node.AddDict();
                        var table = val as Hashtable;
                        SetPlist(proj, t, table);
                    }
                }
            }
        }
    }
}
#endif