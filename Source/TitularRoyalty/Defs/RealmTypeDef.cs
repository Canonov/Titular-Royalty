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
        public enum GovernmentType
        {
            Monarchy,
            Military,
            Communist
        }

        public GovernmentType governmentType = GovernmentType.Monarchy;
        
        public List<RealmTypeTitle> titleOverrides;
        public Dictionary<PlayerTitleDef, RealmTypeTitle> TitlesWithOverrides
        {
            get
            {
                if (!titleOverrides.NullOrEmpty())
                {
                    return titleOverrides.ToDictionary(x => x.titleDef, x => x);
                }
                return new Dictionary<PlayerTitleDef, RealmTypeTitle>();
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
