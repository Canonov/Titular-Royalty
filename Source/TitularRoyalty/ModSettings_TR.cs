using System.Collections.Generic;
using UnityEngine;
using Verse;
using SettingsHelper;

namespace TitularRoyalty
{
    public class TRSettings : ModSettings
    {
        public string realmType;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref realmType, "realmType", "Kingdom");
            base.ExposeData();
        }
    }

    public class TitularRoyaltyMod : Mod
    {
        /// <summary>
        /// A reference to our settings.
        /// </summary>
        TRSettings settings;

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public TitularRoyaltyMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<TRSettings>();
        }

        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>

        public static string[] realmTypes = { "Kingdom", "Empire" };

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.AddLabeledRadioList($"Realm Type, default is Kingdom, these will change the names for your titles for lists of what titles are in which realm type see the mod page",
                                                 realmTypes, ref settings.realmType);
            //listingStandard.CheckboxLabeled("exampleBoolExplanation", ref settings.exampleBool, "exampleBoolToolTip");
            //listingStandard.Label("exampleFloatExplanation");
            //settings.exampleFloat = listingStandard.Slider(settings.exampleFloat, 100f, 300f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Titular Royalty"; //.Translate()
        }
    }
}
