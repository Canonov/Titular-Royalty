//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TitularRoyalty
{
    public class RealmTypeDef : Def
    {

        public List<RealmTypeTitle> titleOverrides;
        
        public Dictionary<PlayerTitleDef, RealmTypeTitle> TitlesWithOverrides
        {
            get
            {
                return titleOverrides.ToDictionary(x => x.titleDef, x => x);
            }
        }
        
        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }
        }
    }
}
