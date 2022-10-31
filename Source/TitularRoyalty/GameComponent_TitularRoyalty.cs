//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TitularRoyalty
{
    public class GameComponent_TitularRoyalty : GameComponent
    {	
		// Labels and PlayerTitles lists
		public List<string> labelsm = new List<string>();
		public List<string> labelsf = new List<string>();
		public List<RoyalTitleDef> playerTitles = new List<RoyalTitleDef>();

		public GameComponent_TitularRoyalty(Game game) // Needs this or else Rimworld throws a fit and errors.
        {

        }

		/// <summary>
		/// Adds all the TitleDefs that are meant for the player to a list
		/// </summary>
		public void PopulatePlayerTitles()
        {
			Log.Message("Titular Royalty: Populating Titlelist");
			if (playerTitles.Count > 0)
            {
				playerTitles.Clear();
            }

			foreach (RoyalTitleDef title in DefDatabase<RoyalTitleDef>.AllDefsListForReading.OrderBy(x => x.seniority) )
            {
				//Log.Message($"Populate Player Titles: {title.defName}, {title.seniority}");
				if (title.tags.Contains("PlayerTitle"))
                {
					playerTitles.Add(title);
                }

			}

		}

		/// <summary>
		/// Resets all changed titles to their default realmType or Base Value
		/// </summary>
		public void ResetTitles()
        {
			// change every item to none so they're ignored
			for (int i = 0; i < labelsm.Count; i++)
            {
				labelsm[i] = "none";
            }
			for (int i = 0; i < labelsf.Count; i++)
			{
				labelsf[i] = "none";
			}

			// Save the changes, hopefully.
			this.ExposeData();
		}

        public void DoTitleChange(RoyalTitleDef title, string basert)
        {
			int titleIndex = playerTitles.IndexOf(title);

			// Custom Titles
			if (titleIndex >= 0)
            {
				// Female Title
				if (labelsf[titleIndex] != "none")
                {
					if (labelsf[titleIndex] == "remove")
                    {
						title.labelFemale = null;
                    }
					else
                    {
						title.labelFemale = labelsf[titleIndex];
                    }
                }
				else if (labelsf[titleIndex] == "none" && labelsm[titleIndex] != "none") // Just incase
                {
                    labelsf[titleIndex] = "remove";
					title.labelFemale = null;
                }

				// Male Title
				if (labelsm[titleIndex] != "none")
				{
					title.label = labelsm[titleIndex];
                    title.ClearCachedData(); //Clear the cached label
                    return;
				}
            }
			else
            {
				Log.Error("Titular Royalty: Failed DoTitleChange()");
				return;
            }

			// TitleLists
			if (title.modExtensions != null)
            {
				foreach (AlternateTitlesExtension ext in title.modExtensions)
                {
					if (ext.realmType == basert)
                    {
						// Male / Neutral Label
						title.label = ext.label;

						// Female Labels
						if (ext.labelf == null || ext.labelf == "none")
                        {
							title.labelFemale = null;
                        }
						else
                        {
							title.labelFemale = ext.labelf;
                        }

						break;
                    }
                }
            }
            title.ClearCachedData(); //Clear the cached label
        }

		/// <summary>
		/// Passes on the Realmtype 
		/// </summary>
		public void ManageTitleLoc()
        {
			bool hasRealm = false;
			string realmType = LoadedModManager.GetMod<TitularRoyaltyMod>().GetSettings<TRSettings>().realmType;

			while(!hasRealm)
            {
				if (realmType != null)
				{
					//Log.Message(realmType);

					switch (realmType)
					{
						case "Empire":
							break;
						case "Kingdom":
							break;
						case "Roman":
							break;
                        case "Roman (Alt)":
                            break;
						default:
							Log.Message("Titular Royalty: Invalid RealmType, make sure one is selected in settings");
							LoadedModManager.GetMod<TitularRoyaltyMod>().GetSettings<TRSettings>().realmType = "Kingdom";
							LoadedModManager.GetMod<TitularRoyaltyMod>().GetSettings<TRSettings>().ExposeData();
							realmType = "Kingdom";
							break;
					}

					foreach (RoyalTitleDef title in playerTitles)
					{
						DoTitleChange(title, realmType);
					}
					hasRealm = true;
				}
				else
				{
					Log.Message("Titular Royalty: no RealmType Found : Defaulting to Kingdom");
					LoadedModManager.GetMod<TitularRoyaltyMod>().GetSettings<TRSettings>().realmType = "Kingdom";
					LoadedModManager.GetMod<TitularRoyaltyMod>().GetSettings<TRSettings>().ExposeData();
					realmType = "Kingdom";
					continue;
				}
				break;
			}


		}

        #region SaveData
        /// <summary>
        /// Changes a title name of the given seniority and gender
        /// </summary>
        /// <param name="gender">Gender you want to change Gender.Male or None, or Gender.Female</param>
        /// <param name="Seniority">Seniority of your Title</param>
        public void SaveTitleChange(Gender gender, int Seniority, string s)
        {
			var titlesSeniorityOrder = Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading;

			if (labelsf.Count == 0 || labelsm.Count == 0)
            {
				Log.Error("Titular Royalty: Invalid save data count"); // Should never happen but who knows
				return;
            }

			int i = 0;

			// Female Titles
			if (gender == Gender.Female)
            {
                foreach (RoyalTitleDef item in titlesSeniorityOrder)
                {
					if (item.seniority == Seniority)
					{
						labelsf[i] = s;
                    }
					i++;
				}
            }
			// Male or Neutral Titles
			else
            {
				foreach (RoyalTitleDef item in titlesSeniorityOrder)
				{
					if (item.seniority == Seniority)
                    {
						labelsm[i] = s;
					}
					i++;
				}
			}

			ExposeData();
        }

		/// <summary>
		/// Saves & Loads the title data, if updating from a TR 1.1 save or the values are otherwise not found, generate new ones
		/// </summary>
		public override void ExposeData()
		{
			try
			{
				//List<RoyalTitleDef> playerTitles = DefDatabase<RoyalTitleDef>.AllDefsListForReading;

				if (labelsf.Count == 0)
				{
					foreach (RoyalTitleDef title in playerTitles)
					{
						if (title.tags.Contains("PlayerTitle"))
						{
							labelsf.Add("none");
						}
					}
				}
				if (labelsm.Count == 0)
				{
					foreach (RoyalTitleDef title in playerTitles)
					{
						if (title.tags.Contains("PlayerTitle"))
						{
							labelsm.Add("none");
						}
					}
				}
				base.ExposeData();

				// This saves the lists
				Scribe_Collections.Look(ref labelsm, "CustomTitlesM", LookMode.Value);
				Scribe_Collections.Look(ref labelsf, "CustomTitlesF", LookMode.Value);
			}
			catch (System.NullReferenceException)
			{
				Log.Message($"Titular Royalty: Loaded TR 1.1 save");
				this.labelsm = new List<string>();
				this.labelsf = new List<string>();
				this.playerTitles =	new List<RoyalTitleDef>();
				ExposeData();
			}
		}
        #endregion


        #region GameComponent Methods
        public void OnGameStart()
        {
            PopulatePlayerTitles();
            if (labelsf.Count == 0)
            {
                foreach (RoyalTitleDef title in playerTitles)
                {
                    if (title.tags.Contains("PlayerTitle"))
                    {
                        labelsf.Add("none");
                    }
                }
            }
            if (labelsm.Count == 0)
            {
                foreach (RoyalTitleDef title in playerTitles)
                {
                    if (title.tags.Contains("PlayerTitle"))
                    {
                        labelsm.Add("none");
                    }
                }
            }
            ManageTitleLoc();

			Faction.OfPlayer.allowGoodwillRewards = false;
			Faction.OfPlayer.allowRoyalFavorRewards = false;
        }


        public override void LoadedGame()
        {
			OnGameStart();
        }

        public override void StartedNewGame()
        {
			OnGameStart();

        }
        #endregion
    }
}