using Verse;

namespace TitularRoyalty
{
    public class TitleFeature : IExposable, ILoadReferenceable
    {

        public int loadId = -1;

        public TitleFeatureDef def;
        public PlayerTitle title;

        public Pawn Pawn => title.pawn;

        public virtual bool Active => true;
         
        public virtual string Label => def.label;
        public virtual string LabelCap => Label.CapitalizeFirst();

        #region Constructors
        
        public TitleFeature(PlayerTitle title, TitleFeatureDef def)
        {
            this.title = title;
            this.def = def;
            loadId = Find.UniqueIDsManager.GetNextGeneID();
        }
        
        // For ExposeData 
        public TitleFeature() { }
        
        #endregion

        #region Events
        
        public virtual void Notify_TitleRemoved() { }
        public virtual void Notify_Inherited(Pawn prevHolder, Pawn inheritor) { }
        public virtual void PostMake() { }
        public virtual void PostAdd() { }
        public virtual void PostRemove() { }
        public virtual void Tick() { }

        #endregion

        
        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref loadId, "loadId");
            Scribe_Defs.Look(ref def, "featureDef");
        }

        public string GetUniqueLoadID() => "TitleFeature_" + loadId;
    }
}