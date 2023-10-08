using System.Diagnostics;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
    public static class LogTR
    {
        public static readonly Color TRColor = new Color(0.64f, 0.27f, 0.65f);
        public static readonly Color TRColorDebug = new Color(0.65f, 0.04f, 0.38f);
        
        public static string TRPrefixDebug => TRPrefix + " [Debug]".Colorize(TRColorDebug);
        public static string TRPrefix => "[Titular Royalty II]".Colorize(TRColor);
        
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
        [Conditional("DEBUG")]
        public static void DebugMessage(string message)
        {
            Log.Message(TRPrefixDebug + ' ' + message);
        }

        [Conditional("DEBUG")]
        public static void DebugWarning(string message)
        {
            Log.Warning(TRPrefixDebug + ' ' + message);
        }

    }
}