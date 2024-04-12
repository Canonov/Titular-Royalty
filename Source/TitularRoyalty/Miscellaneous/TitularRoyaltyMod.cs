using UnityEngine;
using SettingsHelper;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld.QuestGen;

namespace TitularRoyalty
{
    public class TRSettings : ModSettings
    {
        public bool inheritanceEnabled;
        public bool clothingQualityRequirements;
        public bool titlesGivePermitPoints;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref inheritanceEnabled, "inheritanceEnabled", true);
            Scribe_Values.Look(ref clothingQualityRequirements, "clothingQualityRequirements", true);
            Scribe_Values.Look(ref titlesGivePermitPoints, "titlesGivePermitPoints", true);
            base.ExposeData();
        }
    }

    [UsedImplicitly]
    public class TitularRoyaltyMod : Mod
    {
        public static TRSettings Settings { get; private set; }

        // A mandatory constructor which resolves the reference to our settings.
        public TitularRoyaltyMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<TRSettings>();

            // Harmony Stuff
            // Use Patch Categories next harmony update?
            var harmony = new Harmony("com.TitularRoyalty.patches");

            // Prevent Player pawns from giving you royalty quests
            harmony.Patch(original: AccessTools.Method(typeof(QuestNode_GetPawn), "IsGoodPawn"),
                postfix: new HarmonyMethod(typeof(QuestGen_Patches), nameof(QuestGen_Patches.IsGoodPawn_Postfix)));

            // Add a widget to the playsettings to open the Dialog_ManageTitles
            harmony.Patch(original: AccessTools.Method(typeof(PlaySettings), "DoPlaySettingsGlobalControls", (Type[])null, (Type[])null),
                postfix: new HarmonyMethod(typeof(ManageTitlesWidget), nameof(ManageTitlesWidget.AddWidget)));
            
	        // Add icons to the Royal Titles
	        /* Temporarily disabled until compatible with 1.5
            if (ModLister.HasActiveModWithName("Vanilla Factions Expanded - Empire"))
            {
				harmony.Patch(original: AccessTools.Method(typeof(Widgets), nameof(Widgets.DefIcon)),
	                prefix: new HarmonyMethod(typeof(DefIcon_RoyalIconsPrefix), "Patch"));
			} */

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
            listingStandard.Gap(12);

            var miscOptionsTitleRect = listingStandard.GetRect(32);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(miscOptionsTitleRect, "TR_miscoptionstitle".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            listingStandard.Gap(12);

            //First row of checkbox options
            var checkboxes = listingStandard.GetRect(24).BeginListingStandard(2);
            checkboxes.CheckboxLabeled("TR_checkbox_vanillainheritance".Translate(), ref Settings.inheritanceEnabled);
            checkboxes.CheckboxLabeled("TR_checkbox_needsclothesquality".Translate(), ref Settings.clothingQualityRequirements);
            //Checkboxes.CheckboxLabeled("TR_checkbox_titlegivespermitpoints".Translate(), ref Settings.titlesGivePermitPoints);
            checkboxes.End();
            
			listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // END SETTINGS
        //===================================

    }

}
