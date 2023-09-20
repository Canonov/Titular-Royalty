using RimWorld;
using TitularRoyalty.Extensions;
using Verse;

namespace TitularRoyalty.Titles
{
    
    /// <summary>
    /// A Specific Pawn's Instance of a Player Title
    /// </summary>
    public class PlayerTitle : IExposable
    {
        public GameComponent_PlayerTitlesManager TitleManager => GameComponent_PlayerTitlesManager.Current;
        public Comp_PlayerRoyaltyTracker RoyaltyTracker => pawn.PlayerRoyalty();
        
        public PlayerTitleData titleData;
        public Pawn pawn;
        
        public int receivedTick = -1;
        
        public string GetLabelForHolder()
        {
            if (pawn.gender == Gender.Female && titleData.labelFemale != null)
            {
                return titleData.labelFemale;
            }

            return titleData.label.CapitalizeFirst();
        }

        public string GetInspectString()
        {
            return "TR.PawnTitleDescWrap_In".Translate(
                GetLabelForHolder(), Faction.OfPlayer.NameColored).Resolve();
        }
        
        
        public PlayerTitle() {}
        public PlayerTitle(Pawn pawn, PlayerTitleData titleData)
        {
            this.pawn = pawn;
            this.titleData = titleData;
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_References.Look(ref titleData, "titleData");
        }
    }
}