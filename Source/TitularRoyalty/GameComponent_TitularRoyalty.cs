//using System; 
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TitularRoyalty
{
    public class GameComponent_TitularRoyalty : GameComponent
    {
		/*private class FactionTitleList
		{
			Faction _thefaction;
			Dictionary<int, RoyalTitleDef> _titles;

			public FactionTitleList(Faction inputfaction)
			{
				_thefaction = inputfaction;
			}
			public Faction TheFaction
			{
				get { return _thefaction; }
				set { _thefaction = value; }
			}
			public Dictionary<int, RoyalTitleDef> Titles
			{
				get { return _titles; }
			}

			public void AppendTitle(int senior, RoyalTitleDef title)
			{
				_titles.Add(senior, title);
			}
		}*/

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
				Log.Warning("no RealmType Found");
				return "Kingdom";
            }
		}

		private void ManageTitleLoc()
        {
			Log.Message("Before Realmtype");
			string realmType = GetRealmType();
			Log.Message(realmType);

			var titles = DefDatabase<RoyalTitleDef>.AllDefsListForReading;

			switch (realmType)
            {
				case "Empire":
					break;
				case "Kingdom":
					break;
				default:
					Log.Error("Titular Royalty: Invalid RealmType");
					return;
			}

			foreach (RoyalTitleDef v in titles)
			{
				Log.Message("first foreach");
				if (v.modExtensions != null) { 
					foreach (AlternateTitlesExtension ext in v.modExtensions)
					{
						Log.Message("second foreach");
						if (ext.realmType == realmType)
						{
							// Female Labels
							if (v.labelFemale != null && ext.labelf != "none")
							{
								v.labelFemale = ext.labelf;
							}
							else if (v.labelFemale != null && v.labelFemale == "none")
							{
								v.labelFemale = null;
								//v.labelFemale = ext.label;
							}
							else if (v.labelFemale == null && v.labelFemale != "none")
							{
								v.labelFemale = ext.labelf;
							}
							// and if they're both null we don't need to do anything

							v.label = ext.label; // Change the male label

							break; // You can only have one of these so break the loop
						}
					}
				}
			}
			Log.Message("Titles Changed");
		}

		public void OnGameStart()
        {

        }

        public override void LoadedGame()
        {
			Log.Message("LoadedGame");
			ManageTitleLoc();
		}

        public override void StartedNewGame()
        {
			Log.Message("NewGame");
			ManageTitleLoc();
		}
    }
}