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
				Log.Warning("no RealmType Found : Defaulting to Kingdom");
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

		public void SaveTest()
        {
			amogus = "test";
        }

		public string amogus;

        public override void ExposeData()
        {
            base.ExposeData();
			Log.Message(amogus);
			Scribe_Values.Look(ref amogus, "testsavemogus", "defaultvalue");
			Log.Message(amogus);
		}

        public void OnGameStart()
        {

        }

        public override void LoadedGame()
        {
			//ChangeFactionForPermits(Faction.OfPlayer);
			ManageTitleLoc();
			SaveTest();
		}

        public override void StartedNewGame()
        {
			//ChangeFactionForPermits(Faction.OfPlayer);
			ManageTitleLoc();
		}
    }
}