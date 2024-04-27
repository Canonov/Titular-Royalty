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
    
    [DebugAction("Titular Royalty", "Log all Player Title Defs", allowedGameStates = AllowedGameStates.Entry)]
    public static void LogAllTitles()
    {
        foreach (var title in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
            Log.Message($"{title.defName}");
    }
    
    [DebugAction("Titular Royalty", "Add Random Title", allowedGameStates = AllowedGameStates.Playing)]
    public static void AddRandomTitle()
    {
        TitleDatabase.Get().RegisterTitle(new PlayerTitleData("Male", "Female")
            { description = "Test Desc", inheritable = true, minExpectation = ExpectationDefOf.High });
        
        TitleDatabase.Get().RegisterTitle(new PlayerTitleData("Male2", "Female2"));
    }
}