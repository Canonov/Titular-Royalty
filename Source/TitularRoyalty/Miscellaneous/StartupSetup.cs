namespace TitularRoyalty;

[StaticConstructorOnStartup, UsedImplicitly]
public class StartupSetup
{
    public static void ApplyModSettings()
    {
        foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
        {

        }
    }

    static StartupSetup()
    {
        foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
        {

        }
    }
}