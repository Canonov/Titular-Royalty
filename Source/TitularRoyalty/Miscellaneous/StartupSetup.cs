namespace TitularRoyalty;

[StaticConstructorOnStartup, UsedImplicitly]
public class StartupSetup
{
    public static void ApplyModSettings()
    {
        foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
        {
            //Apply Vanilla Inheritance
            title.UpdateInheritance();

            //Apply Quality Requirements
            title.requiredMinimumApparelQuality = TRSettings.clothingQualityRequirements
                ? title.GetApparelQualityfromTier()
                : QualityCategory.Awful;
        }
    }

    static StartupSetup()
    {
        foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
        {
            title.originalTitleFields = new RoyalTitleOverride(title);
        }
    }
}