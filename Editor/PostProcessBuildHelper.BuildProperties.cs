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
        /// 设置构建属性
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="targetGuid"></param>
        /// <param name="table"></param>
        private static void SetBuildProperties(PBXProject proj, string targetGuid, Hashtable table)
        {
            if (table != null)
            {
                Hashtable setTable = table.Get<Hashtable>("=");
                foreach (DictionaryEntry i in setTable)
                {
                    proj.SetBuildProperty(targetGuid, i.Key.ToString(), i.Value.ToString());
                }

                Hashtable addTable = table.Get<Hashtable>("+");
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

                Hashtable removeTable = table.Get<Hashtable>("-");
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
    }
}
#endif