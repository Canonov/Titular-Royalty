//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using JetBrains.Annotations;

namespace TitularRoyalty
{
    [UsedImplicitly]
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
                return icon ??= ContentFinder<Texture2D>.Get(iconPath ?? string.Empty, false) ?? Resources.CrownIcon;
            }
        }

        private List<Texture2D> tierIconOverridesTex;
        public List<Texture2D> TierIconOverridesTex
        {
            get
            {
                if (tierIconOverridesTex.NullOrEmpty())
                {
                    tierIconOverridesTex = new List<Texture2D>();
                    foreach(string str in tierIconOverrides)
                    {
                        Resources.CustomIcons.TryGetValue(str, out Texture2D texture);
                        if (texture == null)
                        {
                            Log.Warning($"tierIconOverrides for {this.defName} are invalid: \n{tierIconOverrides.ToStringSafe()}\n{Resources.CustomIcons.ToStringFullContents()}");
                            return null;
                        }
                        else
                        {
                            tierIconOverridesTex.Add(texture);
                        }
                    }
                }
                return tierIconOverridesTex;
            }
        }
		public List<string> tierIconOverrides;

		public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

        }
    }
}
