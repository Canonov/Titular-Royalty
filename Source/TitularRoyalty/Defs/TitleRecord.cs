namespace TitularRoyalty;

public class TitleRecord : IExposable
{
    public PlayerTitleDef def;
    public PlayerTitleData titleData;
        
    public TitleRecord(PlayerTitleDef def, PlayerTitleData titleData)
    {
        this.def = def;
        this.titleData = titleData;
        
        // Make the title available.
        def.tags.Add("PlayerTitle"); 
        Utils.ClearFactionTitlesCache(Faction.OfPlayer);
        
        // Apply Data to Title
        UpdateDef();
    }
    
    public TitleRecord() { } // ONLY FOR EXPOSEDATA USE

    public void UpdateDef()
    {
        titleData.Apply(def);
    }

    public static implicit operator PlayerTitleDef(TitleRecord t) => t.def;
    public static implicit operator PlayerTitleData(TitleRecord t) => t.titleData;
        
    public void ExposeData()
    {
        Scribe_Defs.Look(ref def, "def");
        Scribe_Deep.Look(ref titleData, "data");
    }
}