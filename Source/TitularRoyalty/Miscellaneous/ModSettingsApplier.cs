using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TitularRoyalty
{
    public static class ModSettingsApplier
    {
        public static void ApplySettings()
        {
            var Settings = TitularRoyaltyMod.Instance.Settings;

            foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                //Apply Vanilla Inheritance
                if (Settings.inheritanceEnabled && title.TRInheritable)
                {
                    title.canBeInherited = true;
                }
                else
                {
                    title.canBeInherited = false;
                }

                //Apply Quality Requirements
                if (Settings.clothingQualityRequirements)
                {
                    title.requiredMinimumApparelQuality = title.GetApparelQualityfromTier();
                }
                else
                {
                    title.requiredMinimumApparelQuality = QualityCategory.Awful;
                }

                //Apply Title Permit Points
                if (!Settings.titlesGivePermitPoints)
                {
                    title.permitPointsAwarded = 0;
                }

            }
        } 
    }
}
