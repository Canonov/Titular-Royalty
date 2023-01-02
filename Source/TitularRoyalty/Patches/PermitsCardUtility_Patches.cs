using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
using RimWorld.QuestGen;
using RimWorld.Planet;
using System.Reflection.Emit;
using System.Reflection;
using Mono.Cecil.Cil;

namespace TitularRoyalty
{

    [HarmonyPatch(typeof(PermitsCardUtility), "ShowSwitchFactionButton", MethodType.Getter)]
    public static class PermitsCardUtility_ShowSwitchFactionButton_Get_Prefix
    {
        public static void Postfix(ref bool __result)
        {
            //TODO - Add a check for if the empire exists
            __result = true;
        }
    }

    /// <summary>
    /// remove IL_0095, 0097, 009c
    /// </summary>
    [HarmonyPatch(typeof(PermitsCardUtility), "DrawRecordsCard")]
    public static class PermitsCardUtility_DrawRecordsCard_Transpiler
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var moddedCodes = codes;
            
            for (int i = 0; i < codes.Count; i++)
            {
                //This could be done better but i'm lazy
                if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand.ToString().Contains("get_IsPlayer"))
                {
                    moddedCodes.RemoveAt(i - 1);
                    moddedCodes.RemoveAt(i - 1);
                    moddedCodes.RemoveAt(i - 1);
                    break;
                }

            }


            return moddedCodes.AsEnumerable();
        }
    }
}
