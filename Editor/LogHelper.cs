using System;

namespace GameFrameX.Xcode.Editor
{
    internal static class LogHelper
    {
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log($"[GameFrameX-Xcode:{DateTime.Now:HH:mm:ss.fff}]:" + msg);
        }
    }
}