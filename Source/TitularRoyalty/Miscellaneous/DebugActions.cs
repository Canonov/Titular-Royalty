using LudeonTK;

namespace TitularRoyalty;

public static class DebugActions
{

    [DebugAction("Titular Royalty", "Try Apply ModSettings", allowedGameStates = AllowedGameStates.Playing)]
    public static void ReloadSettings()
    {
        StartupSetup.ApplyModSettings();
    }
    
    [DebugAction("Titular Royalty", "Log all Title Defs", allowedGameStates = AllowedGameStates.Entry)]
    public static void LogAllTitles()
    {
        foreach (var title in TitleDatabase.Titles) 
            Log.Message($"{title.defName} ({title.numberGenerated})");
    }

}