using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using SettingsHelper;
using HarmonyLib;
using UnityEngine.UIElements;
using System.Linq;
using System;
using JetBrains.Annotations;
using RimWorld.QuestGen;
using RimWorld;

namespace TitularRoyalty
{
    public class TRSettings : ModSettings
    {
        public override void ExposeData()
        {
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

            // Add a widget to the playsettings to open the Dialog_ManageTitles
            harmony.Patch(original: AccessTools.Method(typeof(PlaySettings), "DoPlaySettingsGlobalControls", (Type[])null, (Type[])null),
                postfix: new HarmonyMethod(typeof(PlaySettings_ManageTitlesWidgetPatch), nameof(PlaySettings_ManageTitlesWidgetPatch.AddWidget)));
            
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
            //var checkboxes = listingStandard.GetRect(24).BeginListingStandard(2);
            //checkboxes.CheckboxLabeled("TR_checkbox_vanillainheritance".Translate(), ref Settings.inheritanceEnabled);
            //checkboxes.CheckboxLabeled("TR_checkbox_needsclothesquality".Translate(), ref Settings.clothingQualityRequirements);
            //Checkboxes.CheckboxLabeled("TR_checkbox_titlegivespermitpoints".Translate(), ref Settings.titlesGivePermitPoints);
            //checkboxes.End();
            
			listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // END SETTINGS
        //===================================

    }

}
