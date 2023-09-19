using Verse;

namespace TitularRoyalty.Titles
{
    
    /// <summary>
    ///
    /// </summary>
    public class PlayerTitleData : IExposable, ILoadReferenceable
    {
        public int id;
        
        public string label;
        public string labelFemale;

        public PlayerTitleData()
        {
            id = Find.UniqueIDsManager.nextIdeoID;
        }
        
        public void ExposeData()
        {
            Scribe_Values.Look(ref id, "id");
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref labelFemale, "labelFemale");
        }

        public string GetUniqueLoadID()
        {
            return "playertitle_" + id;
        }
    }
}