//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TitularRoyalty
{
	/// This really needs a rewrite it's kinda a mess
    public class GameComponent_TitularRoyalty : GameComponent
    {
        public GameComponent_TitularRoyalty(Game game) // Needs this or else Rimworld throws a fit and errors.
        {
        }

        private string realmTypeDefName;
        public string RealmType
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
                return realmTypeDef ??= DefDatabase<RealmTypeDef>.GetNamed(RealmType, false);
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
        private Dictionary<PlayerTitleDef, TitleLabelPair> customTitles;
        public Dictionary<PlayerTitleDef, TitleLabelPair> CustomTitles
        {
            get
            {
                return customTitles ??= TitlesBySeniority.ToDictionary(x => x, x => new TitleLabelPair());
            }
            private set
            {
                customTitles = value;
            }
        }

        // Required for ExposeData
        private List<PlayerTitleDef> customTitles_List1;
        private List<TitleLabelPair> customTitles_List2;

        /// <summary>
        /// Passes on the Realmtype 
        /// </summary>
        public void SetupTitles()
        {
            foreach (PlayerTitleDef title in TitlesBySeniority)
            {
                // Custom Title
                if (CustomTitles.TryGetValue(title, out TitleLabelPair titleLabels) && (titleLabels.label != "None" || titleLabels.HasFemaleTitle()) )
                {
                    title.label = titleLabels.label ?? title.label;
                    title.labelFemale = titleLabels.HasFemaleTitle() ? titleLabels.labelFemale : null;
                    goto Finalize;
                }
                // Realm Type, if No Custom
                if (RealmTypeDef.TitlesWithOverrides.TryGetValue(title, out RealmTypeTitle overrides))
                {
                    title.label = overrides.label ?? title.label;
                    title.labelFemale = overrides.HasFemaleTitle() ? overrides.labelFemale : null;

                    if (overrides.useTierOverride)
                    {
                        title.titleTier = overrides.tierOverride;
                    }
                }
                else
                {
                    title.label = title.originalLabels.label;
                    title.labelFemale = title.originalLabels.labelFemale;
                }

                Finalize:
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
                title.label = title.originalLabels.label;
                title.labelFemale = title.originalLabels.labelFemale;
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
            if (CustomTitles.TryGetValue(title, out TitleLabelPair labels))
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