using System.Collections.Generic;
using Verse;

namespace TitularRoyalty.Extensions
{
    public static class PawnExtensions
    {
        public static Dictionary<Pawn, Comp_PlayerRoyaltyTracker> CachedPawnComps { get; private set; } =
            new Dictionary<Pawn, Comp_PlayerRoyaltyTracker>();
        
        /// <summary>
        /// Gets the cached royalty tracker, or finds and caches it.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns>the Pawn's PlayerRoyaltyTracker</returns>
        public static Comp_PlayerRoyaltyTracker PlayerRoyalty(this Pawn pawn)
        {
            if (CachedPawnComps.TryGetValue(pawn, out var royaltyTracker))
            {
                return royaltyTracker;
            }

            royaltyTracker = pawn.GetComp<Comp_PlayerRoyaltyTracker>();

            if (royaltyTracker != null)
            {
                CachedPawnComps.Add(pawn, royaltyTracker);
            }
            
            return royaltyTracker;
        }

        /// <summary>
        /// Check if the pawn has any player titles
        /// </summary>
        /// <returns>Whether the pawn has any titles</returns>
        public static bool HasAnyPlayerTitle(this Pawn pawn)
        {
            return pawn.PlayerRoyalty().HasAnyTitle;
        }
    }
}