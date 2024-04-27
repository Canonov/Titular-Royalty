using LudeonTK;

namespace TitularRoyalty;

public static class DebugActions
{
    [DebugAction("Titular Royalty", "Log all Title Defs", allowedGameStates = AllowedGameStates.Entry)]
    public static void LogAllPlayerTitles()
    {
        foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            Log.Message($"{title.defName} ({title.numberGenerated})");
    }

    [DebugAction("Titular Royalty", "Log all Titles", allowedGameStates = AllowedGameStates.Playing)]
    public static void LogAllPlayerTitlesInGame()
    {
        foreach (PlayerTitleDef title in TitleDatabase.Get().Titles)
            Log.Message($"{title.defName} ({title.numberGenerated})");
    }
    
    [DebugAction("Titular Royalty", "Add Random Title", allowedGameStates = AllowedGameStates.Playing)]
    public static void AddRandomTitle()
    {
        TitleDatabase.Get().RegisterTitle(new PlayerTitleData("Male", "Female")
            { description = "Test Desc", inheritable = true, minExpectation = ExpectationDefOf.High });
        
        TitleDatabase.Get().RegisterTitle(new PlayerTitleData("Male2", "Female2"));
    }
    
    [DebugAction("Titular Royalty", "Delete Random Title", allowedGameStates = AllowedGameStates.Playing)]
    public static void RemoveTitle()
    {
        var titleDB = TitleDatabase.Get();
        var randTitle = titleDB.Titles.RandomElement();
        Log.Message($"Deleting: {randTitle?.def?.defName ?? "None"}");
        if (randTitle != null)
            titleDB.DeregisterTitle(randTitle);

    }
}