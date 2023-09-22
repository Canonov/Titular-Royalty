using System.Collections.Generic;
using RimWorld;
using TitularRoyalty.Extensions;
using Verse;

namespace TitularRoyalty
{
    
    /// <summary>
    /// A Specific Pawn's Instance of a Player Title
    /// </summary>
    public class PlayerTitle : IExposable
    {
        public GameComponent_PlayerTitlesManager TitleManager => GameComponent_PlayerTitlesManager.Current;
        public Comp_PlayerRoyaltyTracker RoyaltyTracker => pawn.PlayerRoyalty();

        public List<TitleFeature> features = new List<TitleFeature>();
        
        public PlayerTitleData titleData;
        public Pawn pawn;
        
        public int receivedTick = -1;
        
        #region Feature Management

        public TitleFeature AddFeature(TitleFeatureDef featureDef)
        {
            if (features.Any(x => x.def == featureDef))
                return null;
            
            var feature = TitleFeatureMaker.MakeFeature(this, featureDef);
            
            features.Add(feature);
            feature.PostAdd();

            return feature;
        }

        public void RemoveFeature(TitleFeatureDef featureDef)
        {
            var feature = features.FirstOrDefault(x => x.def == featureDef);
            
            if (feature == null)
                return;
            
            features.Remove(feature);
            feature.PostRemove();
        }

        public TitleFeature GetFeature(TitleFeatureDef featureDef)
            => features.FirstOrDefault(feature => feature.def == featureDef);
        
        public bool HasFeature(TitleFeatureDef featureDef) 
            => features.Any(x => x.def == featureDef);

        public void InitializeAllFeatures()
        {
            foreach (var featureDef in titleData.featureDefs)
            {
                AddFeature(featureDef);
            }
        }

        #endregion

        #region Events

        public void Tick() // Called from Comp
        {
            foreach (var feature in features)
            {
                feature.Tick();
            }
        }

        public void PreRemove() // Before the Title is Destroyed
        {
            foreach (var feature in features)
                feature.Notify_TitleRemoved();
        }

        #endregion
        

        
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
            Scribe_Collections.Look(ref features, "features", LookMode.Deep);
        }
    }
}