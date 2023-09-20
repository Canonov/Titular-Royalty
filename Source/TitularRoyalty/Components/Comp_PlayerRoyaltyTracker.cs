using TitularRoyalty.Titles;
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

        public PlayerTitle title;
        public bool HasAnyTitle => title != null;
        
        public override void CompTick()
        {
            title?.PlayerTitleTick();
        }

        public void SetTitle(PlayerTitleData titleData, bool sendLetter = true)
        {
            var prevTitle = title;
            
            title = new PlayerTitle(Pawn, titleData)
            {
                receivedTick = GenTicks.TicksGame
            };

            // Todo Send Letter
        }

        public bool HasTitle(PlayerTitleData titleData)
        {
            return title.titleData == titleData;
        }
        
        public void StripTitles()
        {
            title = null;
            Log.Message($"Stripped Titles from {Pawn.Name}"); // Todo send actual letter
        }

        public override void PostExposeData()
        {
            Scribe_Deep.Look(ref title, "title");
            base.PostExposeData();
        }
    }
}