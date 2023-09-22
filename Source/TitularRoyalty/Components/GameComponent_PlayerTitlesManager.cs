using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using TitularRoyalty.Extensions;
using Verse;

namespace TitularRoyalty
{
    [UsedImplicitly]
    public class GameComponent_PlayerTitlesManager : GameComponent
    {
        public static GameComponent_PlayerTitlesManager Current { get; private set; }
        
        public GameComponent_PlayerTitlesManager(Game game)
        {
            Current = this;
            Log.Message("Initializing PlayerTitleManager");
        }
        
        public List<PlayerTitleData> Titles => titles ??= new List<PlayerTitleData>();
        private List<PlayerTitleData> titles = new List<PlayerTitleData>();

        private HashSet<Pawn> pawnsWithTitlesCached;
        public HashSet<Pawn> PawnsWithTitles
        {
            get
            {
                if (pawnsWithTitlesCached == null)
                {
                    var foundPawns = new HashSet<Pawn>();

                    // Get every pawn in a map, caravan, cryptosleep, or transport pod
                    foreach (var pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
                    {
                        if (pawn.PlayerRoyalty() != null && pawn.PlayerRoyalty().HasAnyTitle)
                        {
                            foundPawns.Add(pawn);
                        }
                    }

                    pawnsWithTitlesCached = foundPawns;
                }

                return pawnsWithTitlesCached;
            }
        }
        
        /// <summary>
        /// When a title is modified, update all pawns with that title
        /// </summary>
        public void Notify_TitleFeaturesModified(PlayerTitleData titleData, List<TitleFeatureDef> toRemove, List<TitleFeatureDef> toAdd)
        {
            foreach (var pawn in PawnsWithTitles)
            {
                if (pawn.PlayerRoyalty().HasTitle(titleData))
                {
                    var title = pawn.PlayerRoyalty().title;
                    
                    // Remove the features
                    title.features.RemoveAll(x => toRemove.Contains(x.def));

                    // Add new ones
                    foreach (var featureDef in toAdd)
                    {
                        title.AddFeature(featureDef);
                    }
                }
            }
        }

        /// <summary>
        /// Add the pawn to the cache of pawns with titles
        /// </summary>
        /// <param name="pawn">Pawn to add</param>
        public void AddPawnToCache(Pawn pawn)
        {
            if (pawn.PlayerRoyalty().HasAnyTitle)
                pawnsWithTitlesCached.Add(pawn);
        }
        
        /// <summary>
        /// Add a new title to the manager
        /// </summary>
        /// <param name="titleData">Title to add to the list</param>
        /// <param name="createAfter">If not null, the title to insert the new one after</param>
        public void AddTitle(PlayerTitleData titleData, PlayerTitleData createAfter = null)
        {
            if (createAfter != null)
            {
                var nextIdx = Titles.IndexOf(createAfter);
                if (nextIdx == -1)
                {
                    Log.Error($"Couldn't find title {createAfter} in the list of titles");
                }
                
                Titles.Insert(nextIdx, titleData);
                return;
            }
            
            Titles.Add(titleData);
        }

        /// <summary>
        /// Remove Title from the Manager
        /// </summary>
        /// <param name="titleData"></param>
        public void RemoveTitle(PlayerTitleData titleData)
        {
            if (Titles.Contains(titleData))
            {
                Titles.Remove(titleData);
                return;
            }
            
            Log.Error("Tried to remove a title that doesn't exist");
        }

        private void OnGameLoad()
        {
            LogTR.Message($"Loaded and found {PawnsWithTitles.Count} pawns with titles");

            foreach (var pawnsWithTitle in PawnsWithTitles)
            {
                LogTR.Message(pawnsWithTitle.Name.ToString());
            }
        }

        public override void LoadedGame() => OnGameLoad();
        public override void StartedNewGame() => OnGameLoad();

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref titles, "playerTitles", LookMode.Deep);
        }
    }
}