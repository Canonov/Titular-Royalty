//using System; 

using JetBrains.Annotations;

namespace TitularRoyalty;

[UsedImplicitly]
public class GameComponent_TitularRoyalty : GameComponent
{

    public static GameComponent_TitularRoyalty Current { get; private set; }

    public GameComponent_TitularRoyalty(Game game) { Current = this; } // Needs this or else Rimworld throws a fit and errors.

    private string realmTypeDefName;
    public string RealmTypeDefName
    {
        get
        {
            return realmTypeDefName ??= "RealmType_Kingdom";
        }
        set
        {
            realmTypeDefName = value;
            realmTypeDef = null;
        }
    }
        
    private RealmTypeDef realmTypeDef;
    public RealmTypeDef RealmTypeDef
    {
        get
        {
            return realmTypeDef ??= DefDatabase<RealmTypeDef>.GetNamed(RealmTypeDefName, false);
        }
    }

    // Get the Titles List in Order and Cache it
    private static List<PlayerTitleDef> titlesBySeniority;
    public static List<PlayerTitleDef> TitlesBySeniority => titlesBySeniority ??=
        DefDatabase<PlayerTitleDef>.AllDefsListForReading.OrderBy(x => x.seniority).ToList();

    // Custom Titles
    private Dictionary<PlayerTitleDef, RoyalTitleOverride> customTitles;
    public Dictionary<PlayerTitleDef, RoyalTitleOverride> CustomTitles
    {
        get => customTitles ??= TitlesBySeniority.ToDictionary(x => x, x => new RoyalTitleOverride());
        private set => customTitles = value;
    }

    // Required for ExposeData
    private List<PlayerTitleDef> customTitles_List1;
    private List<RoyalTitleOverride> customTitles_List2;

    /// <summary>
    /// Passes on the Realmtype 
    /// </summary>
    public void SetupAllTitles()
    {
        foreach (var title in TitlesBySeniority) 
            SetupTitle(title);
    }

    public void SetupTitle(PlayerTitleDef title)
    {
        // Custom Title
        if (CustomTitles.TryGetValue(title, out RoyalTitleOverride titleOverrides))
        {
            if (titleOverrides.HasTitle())
            {
                ApplyTitleOverrides(title, titleOverrides);
                return;
            }
        }

        // Realm Type, if No Custom
        if (RealmTypeDef.TitlesWithOverrides.TryGetValue(title, out RoyalTitleOverride realmTypeOverrides))
        {
            ApplyTitleOverrides(title, realmTypeOverrides);
        }
        else
        {
            ApplyTitleOverrides(title, title.originalTitleFields);
        }
    }

    private static void ApplyTitleOverrides(PlayerTitleDef title, RoyalTitleOverride titleOverrides, bool isRealmType = false)
    {
        if (titleOverrides.HasTitle())
        {
            title.label = titleOverrides.label;
            title.labelFemale = titleOverrides.HasFemaleTitle() ? titleOverrides.labelFemale : null;
        }
        else
        {
            title.label = title.originalTitleFields.label;
            title.labelFemale = title.originalTitleFields.labelFemale;
        }

        title.titleTier = titleOverrides.titleTier ?? title.originalTitleFields.titleTier ?? TitleTiers.Lowborn;
        title.allowDignifiedMeditationFocus = titleOverrides.allowDignifiedMeditationFocus ?? title.originalTitleFields.allowDignifiedMeditationFocus ?? false;

        title.iconName = titleOverrides.iconName.NullOrEmpty() ? null : titleOverrides.iconName;

        title.TRInheritable = titleOverrides.TRInheritable ?? title.originalTitleFields.TRInheritable ?? false;
        title.canBeInherited = TitularRoyaltyMod.Settings.inheritanceEnabled && title.TRInheritable;

        title.minExpectation = titleOverrides.minExpectation ?? title.originalTitleFields.minExpectation ?? ExpectationDefOf.ExtremelyLow;

        title.ClearCachedData();
    }

    /// <summary>
    /// Resets all changed titles to their default realmType or Base Value
    /// </summary>
    public void ResetTitles()
    {
        customTitles = null;
        titlesBySeniority = null;
        customTitles_List1 = null;
        customTitles_List2 = null;

        foreach (var title in TitlesBySeniority)
        {
            title.ResetToDefaultValues();
            SetupTitle(title);
        }
    }

    public RoyalTitleOverride GetCustomTitleOverrideFor(PlayerTitleDef titleDef)
    {
        if (CustomTitles.TryGetValue(titleDef, out var titleOverride))
        {
            return titleOverride;
        }
        Log.Error($"Titular Royalty: Could not find custom title override for {titleDef.defName} {titleDef.label}");
        return null;
    }


    public void SaveTitleChange(PlayerTitleDef title, RoyalTitleOverride newOverride)
    {
        customTitles[title] = newOverride;
        SetupTitle(title);
    }

    /// <summary>
    /// Code to be run on both loading or starting a new game
    /// </summary>
    private void OnGameStart()
    {
        Current = this;

        SetupAllTitles();
        Faction.OfPlayer.allowGoodwillRewards = false;
        Faction.OfPlayer.allowRoyalFavorRewards = false;
        StartupSetup.ApplyModSettings();
    }

    public override void LoadedGame() => OnGameStart();
    public override void StartedNewGame() => OnGameStart();

    /// <summary>
    /// Saves & Loads the title data, if updating from a TR 1.1 save or the values are otherwise not found, generate new ones
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref realmTypeDefName, "realmTypeDefName", "RealmType_Kingdom");
        Scribe_Collections.Look(ref customTitles, "TRCustomTitles", LookMode.Def, LookMode.Deep, ref customTitles_List1, ref customTitles_List2);
    }

}