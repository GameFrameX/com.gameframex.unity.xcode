using System;

namespace GameFrameX.Xcode.Editor
{
    internal static class LogHelper
    {
        private static string GetTime()
        {
            return $"[GameFrameX-Xcode:{DateTime.Now:HH:mm:ss.fff}]:";
        }

        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(GetTime() + msg);
        }

        public static void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(GetTime() + msg);
        }

        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError(GetTime() + msg);
        }
    }
}