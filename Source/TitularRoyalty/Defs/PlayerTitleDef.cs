//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TitularRoyalty
{
    public class PlayerTitleDef : RoyalTitleDef
    {
        public TitleTiers titleTier = TitleTiers.Lowborn;
        public Texture2D tierIcon
        {
            get { return Resources.TitleTierIcons[(int)titleTier]; }
        }

        public bool TRInheritable = false;
        public RoyalTitleOverride originalTitleFields; //Assigned via StartupSetup

        public QualityCategory GetApparelQualityfromTier()
        {
            //TODO: Move these into xml or something
            return titleTier switch 
            {
                TitleTiers.Gentry => QualityCategory.Poor,
                TitleTiers.LowNoble => QualityCategory.Normal,
                TitleTiers.HighNoble => QualityCategory.Good,
                TitleTiers.Royalty => QualityCategory.Excellent,
                TitleTiers.Sovereign => QualityCategory.Excellent,
                _ => QualityCategory.Awful,
            };
        }

        public void ResetToDefaultValues()
        {
            label = originalTitleFields.label;
            labelFemale = originalTitleFields.labelFemale;

            titleTier = originalTitleFields.titleTier ?? TitleTiers.Lowborn;
            TRInheritable = originalTitleFields.TRInheritable ?? false;
            minExpectation = originalTitleFields.minExpectation;
            allowDignifiedMeditationFocus = originalTitleFields.allowDignifiedMeditationFocus ?? false;

            ClearCachedData();
        }

        public void UpdateInheritance()
        {
            if (TitularRoyaltyMod.Settings.inheritanceEnabled) 
            {
                canBeInherited = TRInheritable;
            }
            else
            {
                canBeInherited = false;
            }
        }
    }
}
