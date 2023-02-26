using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace TitularRoyalty
{
    //Custom Data Structures
    public class RoyalTitleOverride : IExposable
    {
        public PlayerTitleDef titleDef;

        public string label = "None";
        public string labelFemale = "None";

        public string iconName;

        public string rtIconOverridePath;
        private Texture2D RTIconOverrideTex;
		public Texture2D RTIconOverride
        {
            get
            {   
                if (rtIconOverridePath != null)
                {
					return RTIconOverrideTex ??= ContentFinder<Texture2D>.Get(rtIconOverridePath);
				}
                return null;
            }
        }

        public bool? TRInheritable = null;
        public bool? allowDignifiedMeditationFocus = null;
        public bool useTierOverride = false; // Realmtype only
        public TitleTiers? titleTier = null;

        public ExpectationDef minExpectation;

        public RoyalTitleOverride() { }

        public RoyalTitleOverride(PlayerTitleDef playerTitle)
        {
            this.titleDef = playerTitle;

            this.label = playerTitle.label;
            this.labelFemale = playerTitle.labelFemale;

            this.iconName = playerTitle.iconName;

            this.TRInheritable = playerTitle.TRInheritable;
            this.allowDignifiedMeditationFocus = playerTitle.allowDignifiedMeditationFocus;
            this.titleTier = playerTitle.titleTier;
            this.minExpectation = playerTitle.minExpectation ?? ExpectationDefOf.ExtremelyLow;
        }

        public bool HasFemaleTitle() => (labelFemale != "None" && labelFemale != null && labelFemale != string.Empty);
        public bool HasTitle() => !(label == "None" || label == null || label == string.Empty);

        public void ExposeData()
        {
            Scribe_Defs.Look(ref titleDef, nameof(titleDef));

            Scribe_Values.Look(ref label, nameof(label), "None");
            Scribe_Values.Look(ref labelFemale, nameof(labelFemale), "None");

            Scribe_Values.Look(ref iconName, nameof(iconName), string.Empty);

            Scribe_Values.Look(ref TRInheritable, nameof(TRInheritable));
            Scribe_Values.Look(ref allowDignifiedMeditationFocus, nameof(allowDignifiedMeditationFocus));
            Scribe_Values.Look(ref titleTier, nameof(titleTier));
            Scribe_Defs.Look(ref minExpectation, nameof(minExpectation));
        }
    }
}
