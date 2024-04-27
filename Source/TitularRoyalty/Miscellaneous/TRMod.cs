using UnityEngine;
using SettingsHelper;
using HarmonyLib;

namespace TitularRoyalty;

[UsedImplicitly]
public class TRSettings : ModSettings
{
    internal static int titlesToGenerate = 30; // todo add to settings
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref titlesToGenerate, "titlesToGenerate", 30);
        base.ExposeData();
    }
}

[UsedImplicitly]
public class TRMod : Mod
{
    public TRMod(ModContentPack content) : base(content)
    {
        Log.Message($"Loading Titular Royalty v-{content.ModMetaData.ModVersion}");
        GetSettings<TRSettings>(); //Required to bind this mod class with the settings class.
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
    public override string SettingsCategory() => "TR.modname".Translate();

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
        Widgets.Label(miscOptionsTitleRect, "TR_miscoptionstitle");
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        listingStandard.Gap();

        //First row of checkbox options
        var checkboxes = listingStandard.GetRect(24).BeginListingStandard(6);
        checkboxes.End();
            
        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    // END SETTINGS
    //===================================

}