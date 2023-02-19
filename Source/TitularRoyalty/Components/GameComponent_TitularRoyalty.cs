//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TitularRoyalty
{
    public class GameComponent_TitularRoyalty : GameComponent
    {
        public GameComponent_TitularRoyalty(Game game) { } // Needs this or else Rimworld throws a fit and errors.

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
        public static List<PlayerTitleDef> TitlesBySeniority
        {
            get
            {
                return titlesBySeniority ??= DefDatabase<PlayerTitleDef>.AllDefsListForReading.OrderBy(x => x.seniority).ToList();
            }
        }

        // Custom Titles
        private Dictionary<PlayerTitleDef, RoyalTitleOverride> customTitles;
        public Dictionary<PlayerTitleDef, RoyalTitleOverride> CustomTitles
        {
            get
            {
                return customTitles ??= TitlesBySeniority.ToDictionary(x => x, x => new RoyalTitleOverride());
            }
            private set
            {
                customTitles = value;
            }
        }

        // Required for ExposeData
        private List<PlayerTitleDef> customTitles_List1;
        private List<RoyalTitleOverride> customTitles_List2;

        /// <summary>
        /// Passes on the Realmtype 
        /// </summary>
        public void SetupTitles()
        {
            foreach (PlayerTitleDef title in TitlesBySeniority)
            {
                // Custom Title
                if (CustomTitles.TryGetValue(title, out RoyalTitleOverride titleOverrides) && (titleOverrides.label != "None" || titleOverrides.HasFemaleTitle()) )
                {
                    title.label = titleOverrides.label ?? title.label;
                    title.labelFemale = titleOverrides.HasFemaleTitle() ? titleOverrides.labelFemale : null;

                    title.ClearCachedData();
                    continue;
                }
                // Realm Type, if No Custom
                if (RealmTypeDef.TitlesWithOverrides.TryGetValue(title, out RoyalTitleOverride overrides))
                {
                    title.label = overrides.label ?? title.label;
                    title.labelFemale = overrides.HasFemaleTitle() ? overrides.labelFemale : null;

                    if (overrides.useTierOverride)
                    {
                        title.titleTier = overrides.titleTier;
                    }
                }
                else
                {
                    title.label = title.originalTitleFields.label;
                    title.labelFemale = title.originalTitleFields.labelFemale;
                }

                title.ClearCachedData();
            }
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

            foreach (PlayerTitleDef title in TitlesBySeniority)
            {
                title.label = title.originalTitleFields.label;
                title.labelFemale = title.originalTitleFields.labelFemale;
                title.ClearCachedData();
            }
        }

        /// <summary>
        /// Changes a title name of the given seniority and gender, needs to be manually refreshed with all the others with setup titles or a restart
        /// </summary>
        /// <param name="gender">Gender you want to change Gender.Male or None, or Gender.Female</param>
        /// <param name="newlabel">New Title Name</param>
        public void SaveTitleChange(PlayerTitleDef title, string newlabel, Gender gender)
        {
            if (CustomTitles.TryGetValue(title, out RoyalTitleOverride labels))
            {
                if (gender == Gender.Female)
                {
                    labels.labelFemale = newlabel;
                }
                else
                {
                    labels.label = newlabel;
                }
            }
            else
            {
                Log.Error($"TR: Couldn't find Def {title} in CustomTitles");
            }
        }

        /// <summary>
        /// Code to be run on both loading or starting a new game
        /// </summary>
        public void OnGameStart()
        {
            SetupTitles();
			Faction.OfPlayer.SetupPlayerForTR(); // Set Permit factions and other options
			ModSettingsApplier.ApplySettings(); // Apply ModSettings Changes
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
}