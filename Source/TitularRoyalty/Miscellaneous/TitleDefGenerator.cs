namespace TitularRoyalty;

public static class TitleDefGenerator
{
    public static IEnumerable<PlayerTitleDef> GeneratedPlayerTitleDefs()
    {
        for (int i = 1; i < 51; i++)
        {
            yield return CreateTitleDef(i);
        }
    }

    private static PlayerTitleDef CreateTitleDef(int titleNumber)
    {
        string defName = $"TR_TitleDef_{RomanNumerals.To(titleNumber)}";
        
        var titleDef = new PlayerTitleDef();
        titleDef.defName = defName;
        titleDef.numberGenerated = titleNumber;
        titleDef.tags = new List<string> {"PlayerTitle"};
        
        return titleDef;
    }
}