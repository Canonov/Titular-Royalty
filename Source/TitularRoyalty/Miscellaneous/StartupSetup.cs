using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace TitularRoyalty
{
	[StaticConstructorOnStartup, UsedImplicitly]
	public class StartupSetup
    {
		public static void ApplyModSettings()
		{
			var settings = TitularRoyaltyMod.Settings;

			foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
			{
				//Apply Vanilla Inheritance
				title.UpdateInheritance();

				//Apply Quality Requirements
				title.requiredMinimumApparelQuality = settings.clothingQualityRequirements ? title.GetApparelQualityfromTier() : QualityCategory.Awful;

				//Apply Title Permit Points
				if (!settings.titlesGivePermitPoints) 
					title.permitPointsAwarded = 0;

			}
		}

		static StartupSetup()
        {
            foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                title.originalTitleFields = new RoyalTitleOverride(title);
            }
        }
    }
}
