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

        public List<RealmType> AlternateTitles = new List<RealmType>();
    }
}
