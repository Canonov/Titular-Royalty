//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TitularRoyalty
{
    public class GameComponent_TitularRoyalty : GameComponent
    {

		public List<string> labelsm = new List<string>();
		public List<string> labelsf = new List<string>();
		public List<RoyalTitleDef> playerTitles = new List<RoyalTitleDef>();

		public GameComponent_TitularRoyalty(Game game)
        {

        }

		public void PopulatePlayerTitles()
        {
			Log.Message("Populating Titlelist");
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

		public string GetRealmType()
        {
			string realmType = LoadedModManager.GetMod<TitularRoyaltyMod>().GetSettings<TRSettings>().realmType;

			if (realmType != null)
            {
				return realmType;
            }
			else
            {
				Log.Warning("Titular Royalty: no RealmType Found : Defaulting to Kingdom");
				return "Kingdom";
            }
		}

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

		/*public void DoTitleChange(string basert)
        {
			var playerTitlesOrdered = Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading;
			List<string> rtListM = new List<string>();
			List<string> rtListF = new List<string>();

			for (int i = 0; i < playerTitlesOrdered.Count; i++)
            {
				rtListF[i] = "none"; rtListM[i] = "none";
				if (playerTitlesOrdered[i].modExtensions != null)
                {
					foreach (AlternateTitlesExtension ext in playerTitlesOrdered[i].modExtensions)
					{
						if (ext.realmType == basert)
						{

						}
					}
				}

            }


		}*/

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

        }

		public void ManageTitleLoc()
        {
			string realmType = GetRealmType();
			//Log.Message(realmType);

			//Log.Message("Populating Titlelist");
			//PopulatePlayerTitles();

			switch (realmType)
            {
				case "Empire":
					break;
				case "Kingdom":
					break;
				case "Roman":
					break;
				default:
					Log.Error("Titular Royalty: Invalid RealmType, make sure one is selected in settings");
					realmType = "Kingdom";
					break;
			}

			foreach (RoyalTitleDef title in playerTitles)
            {
				DoTitleChange(title, realmType);
			}


		}

        public override void ExposeData()
        {
			List<RoyalTitleDef> playerTitles = DefDatabase<RoyalTitleDef>.AllDefsListForReading;

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

        public void SaveTitleChange(Gender gender, int Seniority, string s)
        {
			var titlesSeniorityOrder = Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading;

			if (labelsf.Count == 0 || labelsm.Count == 0)
            {
				Log.Error("Titular Royalty: Invalid save data count");
				return;
            }

			int i = 0;
			if (gender == Gender.Female)
            {
                foreach (RoyalTitleDef item in Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading)
                {
					if (item.seniority == Seniority)
					{
						labelsf[i] = s;
                    }
					i++;
				}
            }
			else
            {
				foreach (RoyalTitleDef item in Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading)
				{
					if (item.seniority == Seniority)
                    {
						labelsm[i] = s;
					}
					i++;
				}
			}

            //for (int v = 0; v < labelsm.Count; v++)
            //{
            //    Log.Message($"{labelsm[v]} {labelsf[v]}");
            //}

			ExposeData();
        }

        public void OnGameStart()
        {

        }

        public override void LoadedGame()
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
		}

        public override void StartedNewGame()
        {
			//ChangeFactionForPermits(Faction.OfPlayer);
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
		}
    }
}