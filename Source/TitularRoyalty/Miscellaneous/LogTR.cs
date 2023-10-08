using System.Diagnostics;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
    public static class LogTR
    {
        public static readonly Color TRColor = new Color(0.64f, 0.27f, 0.65f);
        public static string TRPrefix => "[Titular Royalty]".Colorize(TRColor);
        
        public static void Message(string message)
        {
            Log.Message(TRPrefix + ' ' + message);
        }
        
        public static void Warning(string message)
        {
            Log.Warning(TRPrefix + ' ' + message);
        }
        
        public static void Error(string message)
        {
            Log.Error(TRPrefix + ' ' + message);
        }
        
        // Debug exclusive, call is ignored in release builds
        [Conditional("DEBUG")] public static void DebugMessage(string message) => Message(message);
        [Conditional("DEBUG")] public static void DebugWarning(string message) => Warning(message);

    }
}