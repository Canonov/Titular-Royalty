namespace TitularRoyalty;

[StaticConstructorOnStartup]
public class TitleDatabase
{
    public static List<PlayerTitleDef> Titles { get; }
    public static List<PlayerTitleDef> TitlesBySeniority { get; private set; }

    static TitleDatabase()
    {
        Titles = DefDatabase<PlayerTitleDef>.AllDefsListForReading;
        TitlesBySeniority = Titles.OrderBy(x => x.seniority).ToList();
    }

    public static void Notify_TitleSeniorityChanged()
    {
        TitlesBySeniority = Titles.OrderBy(x => x.seniority).ToList();
    }
}