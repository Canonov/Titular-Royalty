namespace TitularRoyalty;

public static class TitleDefGenerator
{
    public static IEnumerable<PlayerTitleDef> GeneratedPlayerTitleDefs()
    {
        for (int i = 1; i <= TRSettings.titlesToGenerate; i++)
        {
            yield return CreateTitleDef(i);
        }
    }

    private static PlayerTitleDef CreateTitleDef(int titleNumber)
    {
        string defName = $"TR_TitleDef_{RomanNumerals.FromInt(titleNumber)}";
        
        var titleDef = new PlayerTitleDef
        {
            defName = defName,
            numberGenerated = titleNumber,
        };
        titleDef.ResetToDefault();

        return titleDef;
    }
    
    public static void GenerateImpliedDefs_PreResolve()
    {
        foreach (var titleDef in TitleDefGenerator.GeneratedPlayerTitleDefs())
        {
            DefGenerator.AddImpliedDef(titleDef);
            DefDatabase<RoyalTitleDef>.Add(titleDef);
        }
    }
}