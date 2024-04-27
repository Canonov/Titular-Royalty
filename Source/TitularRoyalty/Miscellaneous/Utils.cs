using System.Reflection;
using HarmonyLib;

namespace TitularRoyalty;

public static class Utils
{
    private static readonly FieldInfo field_royalTitlesAwardableInSeniorityOrderForReading =
        AccessTools.Field(typeof(FactionDef), "royalTitlesAwardableInSeniorityOrderForReading");
    
    private static readonly FieldInfo field_royalTitlesAllInSeniorityOrderForReading =
        AccessTools.Field(typeof(FactionDef), "royalTitlesAllInSeniorityOrderForReading");
    
    public static void SetupPlayerFaction()
    {
        var playerFaction = Faction.OfPlayer;
        
        playerFaction.allowGoodwillRewards = false;
        playerFaction.allowRoyalFavorRewards = false;
    }

    public static void ClearFactionTitlesCache(Faction faction)
    {
        field_royalTitlesAwardableInSeniorityOrderForReading.SetValue(faction.def, null);
        field_royalTitlesAllInSeniorityOrderForReading.SetValue(faction.def, null);
    }
}

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
internal class ReloadableAttribute : Attribute { } // Used with doorstop
