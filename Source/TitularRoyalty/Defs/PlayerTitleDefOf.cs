using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace TitularRoyalty
{
    [DefOf]
    public static class PlayerTitleDefOf
    {
        public static PlayerTitleDef TitularRoyalty_T_RY_Consort;
        public static PlayerTitleDef TitularRoyalty_T_RY_King;

        static PlayerTitleDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PlayerTitleDefOf));
        }
    }
}
