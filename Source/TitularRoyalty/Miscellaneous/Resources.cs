using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
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

    [StaticConstructorOnStartup]
    public class Resources
    {
        public static readonly Texture2D CrownIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon");
        public static readonly Texture2D[] TitleTierIcons =
        {
            ContentFinder<Texture2D>.Get("UI/TieredIcons/RankIcon0"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/RankIcon1"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/RankIcon2"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/RankIcon3"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/RankIcon4"),
            ContentFinder<Texture2D>.Get("UI/TieredIcons/RankIcon5")
        };

        public static Texture2D[] TierIconsForGovernment(RealmTypeDef.GovernmentType govType)
        {
            return TitleTierIcons;
        }
        

    }
}
