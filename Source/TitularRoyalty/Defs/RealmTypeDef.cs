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
            Communist
        }

        public GovernmentType governmentType = GovernmentType.Monarchy;
        
        public List<RoyalTitleOverride> titleOverrides;
        public Dictionary<PlayerTitleDef, RoyalTitleOverride> TitlesWithOverrides
        {
            get
            {
                if (!titleOverrides.NullOrEmpty())
                {
                    return titleOverrides.ToDictionary(x => x.titleDef, x => x);
                }
                return new Dictionary<PlayerTitleDef, RoyalTitleOverride>();
            }
        }

        public string iconPath;
        private Texture2D icon;
        public Texture2D Icon
        {
            get
            {
                if (icon == null)
                {
                    if (iconPath != null)
                    {
                        icon = ContentFinder<Texture2D>.Get(iconPath);
                    }
                    else
                    {
                        icon = Resources.CrownIcon;
                    }
                }
                return icon;
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
