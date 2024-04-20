using UnityEngine;

namespace TitularRoyalty;

//Custom Data Structures
public class RoyalTitleOverride : IExposable
{
    public PlayerTitleDef titleDef;

    public string label = "None";
    public string labelFemale = "None";

    public string iconName;

    public string rtIconOverridePath;
    private Texture2D RTIconOverrideTex;
    public Texture2D RTIconOverride
    {
        get
        {   
            if (rtIconOverridePath != null)
                return RTIconOverrideTex ??= ContentFinder<Texture2D>.Get(rtIconOverridePath);
            
            return null;
        }
    }

    public bool? TRInheritable;
    public bool? allowDignifiedMeditationFocus;
    public TitleTiers? titleTier;

    public ExpectationDef minExpectation;

    public RoyalTitleOverride() { }

    public RoyalTitleOverride(PlayerTitleDef cloneTitle)
    {
        this.titleDef = cloneTitle;

        this.label = cloneTitle.label;
        this.labelFemale = cloneTitle.labelFemale;

        this.iconName = cloneTitle.iconName;

        this.TRInheritable = cloneTitle.TRInheritable;
        this.allowDignifiedMeditationFocus = cloneTitle.allowDignifiedMeditationFocus;
        this.titleTier = cloneTitle.titleTier;
        this.minExpectation = cloneTitle.minExpectation ?? ExpectationDefOf.ExtremelyLow;
    }

    public bool HasFemaleTitle() => labelFemale != "None" && !string.IsNullOrEmpty(labelFemale);
    public bool HasTitle() => label is not ("None" or null or "");

    public void ExposeData()
    {
        Scribe_Defs.Look(ref titleDef, nameof(titleDef));

        Scribe_Values.Look(ref label, nameof(label), "None");
        Scribe_Values.Look(ref labelFemale, nameof(labelFemale), "None");

        Scribe_Values.Look(ref iconName, nameof(iconName), string.Empty);

        Scribe_Values.Look(ref TRInheritable, nameof(TRInheritable));
        Scribe_Values.Look(ref allowDignifiedMeditationFocus, nameof(allowDignifiedMeditationFocus));
        Scribe_Values.Look(ref titleTier, nameof(titleTier));
        Scribe_Defs.Look(ref minExpectation, nameof(minExpectation));
    }
}