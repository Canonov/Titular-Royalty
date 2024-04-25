using HarmonyLib;
using RimWorld.QuestGen;

namespace TitularRoyalty.Patches;

[HarmonyPatch]
public static class Patch_PreventSameFactionQuests
{
    [HarmonyPatch(typeof(QuestNode_GetPawn), "IsGoodPawn")]
    [HarmonyPostfix]
    private static void IsGoodPawn_Postfix(ref bool __result, QuestNode_GetPawn __instance, Pawn pawn, Slate slate)
    {
        // May have to change this if it pops up in events with other conditions but it seems hospitality_joiners is the only one with problems and this is what that uses
        if (__instance.mustHaveRoyalTitleInCurrentFaction.GetValue(slate)) 
        {
            bool? isPlayerFaction = pawn.Faction?.def.isPlayer;
            if (isPlayerFaction == true)
            {
                __result = false;
            }
        }

    }
}