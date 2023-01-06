//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TitularRoyalty
{
    public class RealmTypeTitle
    {
        public PlayerTitleDef titleDef;

        public string label;
        public string labelFemale = "None";
        
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

        public TitleTiers tierOverride;
        public bool useTierOverride = false;
    }
}
