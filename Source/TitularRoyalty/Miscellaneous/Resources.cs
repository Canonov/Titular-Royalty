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
    public static class Resources
    {
        public static readonly Texture2D CrownIcon;
        public static readonly List<Texture2D> TitleTierIcons;

        static Resources()
        {
            CrownIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon");

            //Todo add custom tier icons
            var titletierplaceholder = ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon");
            TitleTierIcons = new List<Texture2D>();
            for (int i = 0; i <= 5; i++)
            {
                TitleTierIcons.Add(titletierplaceholder);
            }
        }
    }
}
