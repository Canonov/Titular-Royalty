using UnityEngine;
using SettingsHelper;
using HarmonyLib;

namespace TitularRoyalty;

[UsedImplicitly]
public class TRSettings : ModSettings
{
    internal static bool inheritanceEnabled;
    internal static bool clothingQualityRequirements;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref inheritanceEnabled, "inheritanceEnabled", true);
        Scribe_Values.Look(ref clothingQualityRequirements, "clothingQualityRequirements", true);
        base.ExposeData();
    }
}

[UsedImplicitly]
public class TitularRoyaltyMod : Mod
{
    public TitularRoyaltyMod(ModContentPack content) : base(content)
    {
        Log.Message($"Loading Titular Royalty v-{content.ModMetaData.ModVersion}");
        ApplyPatches();
    }
    
    private static void ApplyPatches()
    {
        var harmony = new Harmony("com.TitularRoyalty.patches");
        
        // Add icons to the Royal Titles
        if (ModLister.HasActiveModWithName("Vanilla Factions Expanded - Empire")) 
            harmony.PatchCategory("VFEEmpire");
        
        // Apply Patches
        harmony.PatchAllUncategorized();
    }

    //Name that shows at the top
    public override string SettingsCategory() => "TR_modname".Translate();

    //Main Rendering
    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.AddHorizontalLine();

        //Miscellanous Toggles
        listingStandard.Gap();

        var miscOptionsTitleRect = listingStandard.GetRect(32);
        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(miscOptionsTitleRect, "TR_miscoptionstitle".Translate());
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        listingStandard.Gap();

        //First row of checkbox options
        var checkboxes = listingStandard.GetRect(24).BeginListingStandard(2);
        checkboxes.CheckboxLabeled("TR_checkbox_vanillainheritance".Translate(), ref TRSettings.inheritanceEnabled);
        checkboxes.CheckboxLabeled("TR_checkbox_needsclothesquality".Translate(), ref TRSettings.clothingQualityRequirements);
        checkboxes.End();
            
        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    // END SETTINGS
    //===================================

}