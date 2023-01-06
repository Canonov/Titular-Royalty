using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TitularRoyalty
{
    public static class Utilities
    {
        //Helpers
        public static void SetupPlayerForTR(this Faction faction)
        {
            faction.allowGoodwillRewards = false;
            faction.allowRoyalFavorRewards = false;

            foreach (PlayerTitlePermitDef permit in DefDatabase<PlayerTitlePermitDef>.AllDefsListForReading)
            {
                permit.faction = faction.def;
            }
        }
    }

    //Custom Data Structures
    public class TitleLabelPair : IExposable
    {
        public string label = "None";
        public string labelFemale = "None";

        public TitleLabelPair() { }

        public TitleLabelPair(string m = "None", string f = "None")
        {
            label = m;
            labelFemale = f;
        }
        public bool HasFemaleTitle()
        {
            if (labelFemale != "None" && labelFemale != null && labelFemale != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref label, nameof(label), "None");
            Scribe_Values.Look(ref labelFemale, nameof(labelFemale), "None");
        }
    }
}
