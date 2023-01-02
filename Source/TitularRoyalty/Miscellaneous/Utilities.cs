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
}
