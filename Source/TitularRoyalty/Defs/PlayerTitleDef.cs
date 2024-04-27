namespace TitularRoyalty;

public class PlayerTitleDef : RoyalTitleDef
{
    public int numberGenerated;

    public void ResetToDefault()
    {
        tags = new List<string>();
        favorCost = 1; // required for titles to show up
        changeHeirQuestPoints = 1; // required for titles to show up
        awardThought = ThoughtDef.Named("GainedTitle");
        lostThought = ThoughtDef.Named("LostTitle");
    }

}