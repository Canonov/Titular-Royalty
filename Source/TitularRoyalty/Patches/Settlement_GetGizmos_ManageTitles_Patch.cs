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
using static HarmonyLib.Code;

namespace TitularRoyalty
{

    [HarmonyPatch(nameof(Settlement), "GetGizmos")]
    public static class Settlement_GetGizmos_ManageTitles_Patch
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, Settlement __instance)
        {
            if (__instance.Faction == Faction.OfPlayer)
            {
                Command_Action manage_titles = new Command_Action
                {
                    icon = Resources.CrownIcon,
                    defaultLabel = "TR_Command_managetitles_label".Translate(),
                    defaultDesc = "TR_Command_managetitles_desc".Translate(),

                    action = delegate
                    {
                        Dialog_ManageTitles window = new Dialog_ManageTitles(true);
                        Find.WindowStack.Add(window);
                    }
                };
                yield return manage_titles;
            }

            foreach (Gizmo gizmo in gizmos)
            {
                yield return gizmo;
            }
        }
        
    }
}
