using System.Linq;
using System.Text;
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
        
        public PlayerTitle(Pawn pawn, PlayerTitleData titleData)
        {
            this.pawn = pawn;
            this.titleData = titleData;
        }

        public void ExposeData()
        {
            // int lastKnownDataIndex = -1;
            // if (Scribe.mode == LoadSaveMode.Saving)
            // {
            //     lastKnownDataIndex = TitleManager.Titles.IndexOf(titleData);
            // }
            //
            // Scribe_Values.Look(ref lastKnownDataIndex, "lastIndex");
            
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_References.Look(ref titleData, "titleData");

            
            // Handle Missing Title
            /*if (Scribe.mode == LoadSaveMode.LoadingVars && titleData == null)
            {
                Log.Warning($"Title Data was null for pawn {pawn.Name} on load, trying to demote them or will remove their title");
                
                // Couldn't find the index - Remove their title
                if (lastKnownDataIndex <= -1)
                {
                    Log.Error($"Couldn't find the last known index for {pawn.Name}, removing their title");
                    RoyaltyTracker.StripTitles();
                    return;
                }
                
                // The First Title - Remove it 
                if (lastKnownDataIndex == 0)
                {
                    Log.Error($"Couldn't find the last known index for {pawn.Name} was the first, removing their title");
                    RoyaltyTracker.StripTitles();
                    return;
                }
                
                // Higher than the highest title - Give them the highest title
                if (lastKnownDataIndex >= TitleManager.Titles.Count - 1) // Their title was higher than the highest title, so give them that
                {
                    Log.Error($"Last known index for {pawn.Name} was out of the upper bound, giving them the highest title");
                    titleData = TitleManager.Titles.Last();
                    return;
                }
                
                titleData = TitleManager.Titles[lastKnownDataIndex - 1];
                
            }*/
            
        }
    }
}