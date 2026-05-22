using HarmonyLib;

namespace TitularRoyalty.Patches;

[HarmonyPatch(typeof(Pawn_RoyaltyTracker), nameof(Pawn_RoyaltyTracker.SetTitle))]
public static class Patch_PawnRoyaltyTracker_SetTitle
{
    [HarmonyPostfix]
    public static void Postfix(Faction faction)
    {
        if (faction == Faction.OfPlayerSilentFail)
        {
            TitleThoughtUtility.Notify_TitlesChanged();
        }
    }
}
