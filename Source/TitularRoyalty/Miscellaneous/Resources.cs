using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace TitularRoyalty
{
    public enum TitleTiers : int
    {
        Lowborn = 0,
        Gentry = 1,
        LowNoble = 2,
        HighNoble = 3,
        Royalty = 4,
        Sovereign = 5,
    }

    [StaticConstructorOnStartup, UsedImplicitly]
    public class Resources
    {
        public static readonly Texture2D CrownIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon");
        public static readonly Texture2D TRWidget = ContentFinder<Texture2D>.Get("UI/TRwidget");
		public static readonly Texture2D TRCrownWidget = ContentFinder<Texture2D>.Get("UI/TRcrownwidget");


        private static Dictionary<string, Texture2D> customIcons;
        public static Dictionary<string, Texture2D> CustomIcons
        {
            get
            {
                if (customIcons.NullOrEmpty())
                {
                    customIcons = ContentFinder<Texture2D>.GetAllInFolder("TRIcons").ToDictionary(x => x.name, x => x);

                    if (ModLister.HasActiveModWithName("Vanilla Factions Expanded - Empire"))
                    {
						foreach (var icon in ContentFinder<Texture2D>.GetAllInFolder("UI/NobleRanks"))
						{
							customIcons.Add(icon.name, icon);
						}
					}

                }

                return customIcons;
            }
        }

        /*public static readonly Texture2D[] TitleTierIcons_Sov =
        {
            ContentFinder<Texture2D>.Get("UI/TieredIcons/Sov_RankIcon0"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/Sov_RankIcon1"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/Sov_RankIcon2"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/Sov_RankIcon3"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/Sov_RankIcon5"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/Sov_RankIcon5")
        };*/

        public static Texture2D GetIcon(TitleTiers titleTiers, RealmTypeDef realmTypeDef)
        {
            if (!realmTypeDef.tierIconOverrides.NullOrEmpty())
            {
				if (realmTypeDef.TierIconOverridesTex[(int)titleTiers] != null)
                {
                    return realmTypeDef.TierIconOverridesTex[(int)titleTiers];
                }
            }

			return CustomIcons.TryGetValue($"RankIcon{((int)titleTiers)}", BaseContent.BadTex);
		}
		public static Texture2D GetIcon(PlayerTitleDef playerTitleDef, GameComponent_TitularRoyalty TRComponent)
		{
            if (TRComponent.RealmTypeDef.TitlesWithOverrides.TryGetValue(playerTitleDef, out RoyalTitleOverride rtOverride) && rtOverride.RTIconOverride != null) 
            {
                return rtOverride.RTIconOverride;
            }

            if (!playerTitleDef.iconName.NullOrEmpty())
            {
                if (CustomIcons.TryGetValue(playerTitleDef.iconName, out Texture2D result))
                {
                    return result;
                }
                else
                {
					Log.Warning($"{playerTitleDef.label} failed to get icon from name {playerTitleDef.iconName}, it may be missing, reassign it in edit titles");
					playerTitleDef.iconName = null;
                }
            }

            return GetIcon(playerTitleDef.titleTier, Current.Game.GetComponent<GameComponent_TitularRoyalty>().RealmTypeDef);
		}


		static Resources()
        {
            
        }
    }
}
