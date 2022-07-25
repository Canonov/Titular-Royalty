//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TitularRoyalty
{
    public class GameComponent_TitularRoyalty : GameComponent
    {

		public List<string> labelsm = new List<string>();
		public List<string> labelsf = new List<string>();

		public GameComponent_TitularRoyalty(Game game)
        {

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

		public void ManageTitleLoc()
        {
			string realmType = GetRealmType();
			//Log.Message(realmType);

			var titles = DefDatabase<RoyalTitleDef>.AllDefsListForReading;

			switch (realmType)
            {
				case "Empire":
					break;
				case "Kingdom":
					break;
				case "Roman":
					break;
				default:
					Log.Error("Titular Royalty: Invalid RealmType");
					return;
			}

			foreach (RoyalTitleDef title in titles)
			{
				if (title.modExtensions != null) { 
					foreach (AlternateTitlesExtension ext in title.modExtensions)
					{


						if (ext.realmType == realmType)
						{
							// Female Labels
							if (title.labelFemale != null && ext.labelf != "none")
							{
								title.labelFemale = ext.labelf;
							}
							else if (title.labelFemale != null && ext.labelf == "none")
							{
								title.labelFemale = null;
								//v.labelFemale = ext.label;
							}
							else if (title.labelFemale == null && ext.labelf != "none")
							{
								title.labelFemale = ext.labelf;
							}
							// and if they're both null we don't need to do anything

							title.label = ext.label; // Change the male label

							break; // You can only have one of these so break the loop
						}
					}
				}
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

            for (int v = 0; v < labelsm.Count; v++)
            {
                Log.Message($"{labelsm[v]} {labelsf[v]}");
            }

			ExposeData();
        }

        public void OnGameStart()
        {

        }

        public override void LoadedGame()
        {
			//ChangeFactionForPermits(Faction.OfPlayer);
			ManageTitleLoc();
		}

        public override void StartedNewGame()
        {
			//ChangeFactionForPermits(Faction.OfPlayer);
			ManageTitleLoc();



		}
    }
}