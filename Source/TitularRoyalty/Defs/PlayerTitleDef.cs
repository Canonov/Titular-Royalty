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

        public List<RealmTypeTitle> AlternateTitles = new List<RealmTypeTitle>();

        public QualityCategory GetApparelQualityfromTier()
        {
            //TODO: Move these into xml or something
            switch (titleTier)
            {
                case TitleTiers.Gentry:
                    return QualityCategory.Poor;
                case TitleTiers.LowNoble:
                    return QualityCategory.Normal;
                case TitleTiers.HighNoble:
                    return QualityCategory.Good;
                case TitleTiers.Royalty:
                    return QualityCategory.Excellent;
                case TitleTiers.Sovereign:
                    return QualityCategory.Excellent;
                default:
                    return QualityCategory.Awful;
            }
        }
    }
}
