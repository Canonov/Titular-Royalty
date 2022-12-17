using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using SettingsHelper;
using HarmonyLib;
using UnityEngine.UIElements;

namespace TitularRoyalty
{
    public class TRSettings : ModSettings
    {
        public string realmType;
        public bool inheritanceEnabled;
        public bool clothingQualityRequirements;
        //public bool mealQualityRequirements;
        //public bool disableWorkforRoyals;


        public override void ExposeData()
        {
            Scribe_Values.Look(ref realmType, "realmType", "Kingdom");
            Scribe_Values.Look(ref inheritanceEnabled, "inheritanceEnabled", false);
            Scribe_Values.Look(ref clothingQualityRequirements, "clothingQualityRequirements", true);
            //Scribe_Values.Look(ref mealQualityRequirements, "mealQualityRequirements", true);
            //Scribe_Values.Look(ref disableWorkforRoyals, "disableWorkforRoyals", true);
            base.ExposeData();
        }
    }



    public class TitularRoyaltyMod : Mod
    {

        public TRSettings Settings { get; private set; }
        public static TitularRoyaltyMod Instance { get; private set; }

        // A mandatory constructor which resolves the reference to our settings.
        public TitularRoyaltyMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<TRSettings>();
            Instance = this;

            // Harmony Stuff
            var harmony = new Harmony("com.TitularRoyalty.patches");
            harmony.PatchAll();
        }

        public static string[] realmTypes = { "Kingdom", "Empire", "Roman", "Roman (Alt)" };

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            //Explains more about the titlesets
            listingStandard.Label("TR_selectorexplanation".Translate());
            listingStandard.AddHorizontalLine();

            //Radio list to choose titles
            listingStandard.Gap(12);
            Rect realmtypesTitleRect = listingStandard.GetRect(32);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(realmtypesTitleRect, "TR_realmtypeslist_title".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            listingStandard.AddLabeledRadioList("TR_realmtypeslist_header".Translate(), realmTypes, ref Settings.realmType);
            listingStandard.Gap(24);
            listingStandard.AddHorizontalLine();

            //Miscellanous Toggles
            listingStandard.Gap(12);

            Rect miscOptionsTitleRect = listingStandard.GetRect(32);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(miscOptionsTitleRect, "TR_miscoptionstitle".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            listingStandard.Gap(12);

            //First row of checkbox options
            Listing_Standard Checkboxes = listingStandard.GetRect(24).BeginListingStandard(2);
            Checkboxes.CheckboxLabeled("TR_checkbox_vanillainheritance".Translate(), ref Settings.inheritanceEnabled);
            Checkboxes.CheckboxLabeled("TR_checkbox_needsclothesquality".Translate(), ref Settings.clothingQualityRequirements);
            //Checkboxes.CheckboxLabeled("TR_checkbox_needsmealquality".Translate(), ref Settings.mealQualityRequirements);
            //Checkboxes.CheckboxLabeled("TR_checkbox_disableworkroyals".Translate(), ref Settings.disableWorkforRoyals);
            Checkboxes.End();

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "TR_modname".Translate();
        }


        // END SETTINGS
        //===================================

    }

}
