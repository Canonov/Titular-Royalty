using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace TitularRoyalty
{

    [StaticConstructorOnStartup]
    public class OnStartup
    {
		public static void ApplyModSettings()
		{
			var Settings = TitularRoyaltyMod.Settings;

			foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
			{
				//Apply Vanilla Inheritance
				title.UpdateInheritance();

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

		static OnStartup()
        {
            foreach (PlayerTitleDef title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                title.originalTitleFields = new RoyalTitleOverride(title);
            }
        }
    }
}
