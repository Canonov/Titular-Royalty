using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TitularRoyalty
{
    public class AlternateTitlesExtension : DefModExtension 
    {
        public string realmType;
        public string label;
        public string labelf = "none";

        //public bool enabled = true;
    }

    // Used to see if a permit is for the player
    public class IsPlayerPermit : DefModExtension
    {
    }

    public class TitlePlayerPermitsExtension : DefModExtension
    {
        public List<RoyalTitlePermitDef> permits;
    }

}
