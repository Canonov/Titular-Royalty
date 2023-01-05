 using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TitularRoyalty
{

    [DefOf]
    public class RealmTypeDefOf
    {
        static RealmTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RealmTypeDefOf));
        }
    }

    [StaticConstructorOnStartup]
    public static class BaseRealmType
    {
        public static RealmTypeDef Def;
        
        static BaseRealmType()
        {
            Def = new RealmTypeDef()
            {
                defName = "RealmTypeBase",
                label = "Base Realm Type",
            };

            Def.titleOverrides = new List<RealmTypeTitle>();
            
            foreach(PlayerTitleDef title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                Def.titleOverrides.Add(new RealmTypeTitle()
                {
                    titleDef = title,

                    label = title.label,
                    labelFemale = title.labelFemale,
                    tierOverride = title.titleTier
                    
                });
            }
        }
    }
}
