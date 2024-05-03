#nullable enable
namespace TitularRoyalty;

public class PlayerTitleData : IExposable
{
    public string maleTitle = null!;
    public string? femaleTitle;
    public string? description;
    public bool inheritable;
    
    public ExpectationDef? minExpectation;
    public List<ApparelRequirement> requiredApparel = new List<ApparelRequirement>();

    public PlayerTitleData() { } // ONLY FOR EXPOSEDATA USE
    
    public PlayerTitleData(string maleTitle, string? femaleTitle = null)
    {
        this.maleTitle = maleTitle;
        this.femaleTitle = femaleTitle;
    }
    
    public PlayerTitleData(PlayerTitleData other)
    {
        maleTitle = other.maleTitle;
        femaleTitle = other.femaleTitle;
        description = other.description;
        inheritable = other.inheritable;
        minExpectation = other.minExpectation;
        requiredApparel = new List<ApparelRequirement>(other.requiredApparel);
    }
    
    public void Apply(PlayerTitleDef titleDef)
    {
        titleDef.label = maleTitle;
        titleDef.labelFemale = femaleTitle;
        titleDef.description = description;
        titleDef.canBeInherited = inheritable;
        titleDef.minExpectation = minExpectation ?? ExpectationDefOf.ExtremelyLow;
        titleDef.requiredApparel = new List<ApparelRequirement>(requiredApparel);
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref maleTitle!, "maleTitle");
        Scribe_Values.Look(ref femaleTitle, "femaleTitle");
        Scribe_Values.Look(ref description, "description");
        Scribe_Values.Look(ref inheritable, "inheritable");
        Scribe_Defs.Look(ref minExpectation, "minExpectation");
        Scribe_Collections.Look(ref requiredApparel, "requiredApparel", LookMode.Deep);
    }
}