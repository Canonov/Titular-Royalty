using TitularRoyalty.Extensions;
using Verse;

namespace TitularRoyalty
{
    
    /// <summary>
    /// Applied to Pawns to track their royalty status
    /// </summary>
    public class Comp_PlayerRoyaltyTracker : ThingComp
    {
        public GameComponent_PlayerTitlesManager TitlesManager => GameComponent_PlayerTitlesManager.Current;
        public CompProperties_PlayerRoyaltyTracker Props => (CompProperties_PlayerRoyaltyTracker)props;
        public Pawn Pawn => (Pawn)parent;

        public PlayerTitle title;
        public bool HasAnyTitle => title != null;
        
        public override void CompTick()
        {
            title?.Tick();
        }

        public void SetTitle(PlayerTitleData titleData, bool sendLetter = true)
        {
            //var prevTitle = title;
            
            var newTitle = new PlayerTitle(Pawn, titleData)
            {
                receivedTick = GenTicks.TicksGame
            };
            
            newTitle.InitializeAllFeatures();
            title = newTitle;
            
            TitlesManager.AddPawnToCache(Pawn);
            // Todo Send Letter
        }

        public bool HasTitle(PlayerTitleData titleData)
        {
            return title.titleData == titleData;
        }

        public void RemoveTitle()
        {
            title.PreRemove();
            title = null;
            
            TitlesManager.RemovePawnFromCache(Pawn);
        }

        public override void PostExposeData()
        {
            Scribe_Deep.Look(ref title, "title");
            base.PostExposeData();
        }
    }
}