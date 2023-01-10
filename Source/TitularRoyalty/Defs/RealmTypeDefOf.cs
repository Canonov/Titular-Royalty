 using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TitularRoyalty
{

    /*
    [DefOf]
    public class RealmTypeDefOf
    {
        public static RealmTypeDef RealmType_Empire;
        public static RealmTypeDef RealmType_Kingdom;
        public static RealmTypeDef RealmType_Roman;
        public static RealmTypeDef RealmType_LateRoman;

        static RealmTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RealmTypeDefOf));
        }
    }*/

    [StaticConstructorOnStartup]
    public static class BaseRealmType
    {
        //public static RealmTypeDef Def;
        
        static BaseRealmType()
        {
            /*Def = new RealmTypeDef()
            {
                defName = "RealmTypeBase",
                label = "Base Realm Type",
            };*/

            //f.titleOverrides = new List<RealmTypeTitle>();
            
            foreach(PlayerTitleDef title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                /*f.titleOverrides.Add(new RealmTypeTitle()
                {
                    titleDef = title,

                    label = title.label,
                    labelFemale = title.labelFemale,
                    tierOverride = title.titleTier
                    
                });*/
                title.originalLabels = new TitleLabelPair(title.label, title.labelFemale);
            }
        }
    }
}
