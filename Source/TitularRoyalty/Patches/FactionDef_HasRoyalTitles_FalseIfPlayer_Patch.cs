/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;
using System.Diagnostics;
using RimWorld.QuestGen;

namespace TitularRoyalty.Patches
{

    [HarmonyPatch(typeof(RimWorld.FactionDef))]
    [HarmonyPatch("HasRoyalTitles", MethodType.Getter)]
    static class FactionDef_HasRoyalTitles_FalseIfPlayer_Patch
    {
        static bool Prefix(ref bool __result, ref FactionDef __instance, MethodBase __originalMethod)
        {
            StackTrace stackTrace = new StackTrace();
            Log.Message(stackTrace.GetFrame(2).GetMethod().Name);

            if (__instance.isPlayer)
            {
                Log.Message("was player");
                __result = false;
                return false;
            }
            return true;
            /*StackTrace stackTrace = new StackTrace();

            //Log.Message(__instance.label); 
            if (__instance.isPlayer && stackTrace.GetFrame(2).GetMethod().Name.Contains("TryFindFactionForPawnGeneration"))
            {
                Log.Message("Successfully result false");
                __result = false;    
            }

            Log.Message($"{__result} {__instance.label} {__instance.isPlayer} {stackTrace.GetFrame(2).GetMethod().Name} : {stackTrace.GetFrame(2).GetMethod().Name.Contains("TryFindFactionForPawnGeneration")}");
            return !__result;*/
        /*}

        static bool PostFix(ref bool __result)
        {
            Log.Message(__result.ToString());
            return true;
        }

    }
}
*/