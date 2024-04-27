namespace TitularRoyalty;

public class TitleDatabase : IExposable
{
    private List<TitleRecord> titleRecords = new List<TitleRecord>();
    private Stack<PlayerTitleDef> freeTitles;
    private readonly List<PlayerTitleDef> allTitles;

    public TitleDatabase()
    {
        allTitles = DefDatabase<PlayerTitleDef>.AllDefsListForReading
            .OrderBy(x => x.numberGenerated).ToList();
    }
    
    public List<TitleRecord> Titles => titleRecords;
    public int FreeSpace => FreeTitles.Count;
    
    private Stack<PlayerTitleDef> FreeTitles
    {
        get
        {
            if (freeTitles is null)
            {
                freeTitles = new Stack<PlayerTitleDef>();
                foreach (var def in allTitles.Except(titleRecords.Select(x => x.def)))
                {
                    freeTitles.Push(def);
                }
            }
            return freeTitles;
        }
    }
    
    public static TitleDatabase Get()
    {
        return TRGameComponent.Current.titleDatabase;
    }
    
    public TitleRecord GetRecordFromDef(PlayerTitleDef def)
    {
        var result = Titles.FirstOrDefault(record => record.def == def);
        if (result is null)
        {
            Log.Error($"Couldn't find title with defName {def.defName} in titles with data");
        }
        return result;
    }

    public void RegisterTitle(PlayerTitleData data)
    {
        var record = new TitleRecord(FreeTitles.Pop(), data);
        titleRecords.Add(record);
    }

    public void DeregisterTitle(TitleRecord record)
    {
        titleRecords.Remove(record);
        record.def.ResetToDefault();
        freeTitles.Push(record.def);
    }

    public void ModifyTitle(TitleRecord target, PlayerTitleData newData)
    {
        target.titleData = newData;
        target.UpdateDef();
    }
    
    public void ExposeData()
    {
        Scribe_Collections.Look(ref titleRecords, "titleRecords", LookMode.Deep);
    }
}