using System;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GombleTask.Utility
{
    public enum LogLevel
    {
        Default,
        Warning,
        Error,
    }
    
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class Printer
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private const int _ONE_LINE_LENGTH_LIMIT = 40;

        public static string DefaultTag = "GombleTask";
        public static int CurrentPriority = 0;

        private static StringBuilder S_stringBuilder;

        static Printer()
        {
            S_stringBuilder = new StringBuilder();
        }
#endif

        public static void Print(object message, string customTag = null, LogLevel logLevel = LogLevel.Default, int priority = 0)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (priority < CurrentPriority) return;

            S_stringBuilder.Clear();
            S_stringBuilder.Append(BraketedTag(customTag));

            string msg = message.ToString();

            if (msg.Length > _ONE_LINE_LENGTH_LIMIT)
            {
                S_stringBuilder.AppendLine();
            }

            S_stringBuilder.Append(msg);
            S_stringBuilder.AppendLine();

            switch (logLevel)
            {
                case LogLevel.Default: 
                    Debug.Log(S_stringBuilder);
                    break;
                case LogLevel.Warning: 
                    Debug.LogWarning(S_stringBuilder);
                    break;
                case LogLevel.Error: 
                    Debug.LogError(S_stringBuilder);
                    break;
            }
#endif
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private static string BraketedTag(string customTag = null)
        {
            string tag = customTag ?? DefaultTag;
            return $"[{tag}] : ";
        }
#endif
    }
}