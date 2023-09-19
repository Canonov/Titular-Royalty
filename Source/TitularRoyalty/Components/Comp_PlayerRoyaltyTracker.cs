using Verse;

namespace TitularRoyalty
{
    
    /// <summary>
    /// Applied to Pawns to track their royalty status
    /// </summary>
    public class Comp_PlayerRoyaltyTracker : ThingComp
    {
        public CompProperties_PlayerRoyaltyTracker Props => (CompProperties_PlayerRoyaltyTracker)props;
        public Pawn Pawn => (Pawn)parent;

        public override void PostExposeData()
        {
            base.PostExposeData();
        }
    }
}