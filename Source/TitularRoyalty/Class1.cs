using RimWorld;
using System.Reflection;
using System;
using System.Collections.Generic;
using Verse;
using HarmonyLib;

namespace TitularRoyalty
{

    /*
    [HarmonyPatch(typeof(Pawn_RoyaltyTracker), nameof(Pawn_RoyaltyTracker.RefundPermits))]
    class TRPermitPatch
    {
        static void Prefix(ref Pawn ___pawn, ref List<FactionPermit> ___factionPermits, int favorCost, Faction faction)
        {
            ___pawn.royalty.GainFavor(faction, 20);
            ___factionPermits.Clear();

            Log.Message($"From Harmony {___pawn.royalty.GetCurrentTitleInFaction(faction)}");
            return;
        }
    }
    */

    [HarmonyPatch(typeof(PermitsCardUtility), nameof(PermitsCardUtility.ShowSwitchFactionButton))]
    class TRPermitPatch
    {
        static void Prefix()
        {

        }
    }


}