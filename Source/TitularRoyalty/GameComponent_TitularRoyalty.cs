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

		public GameComponent_TitularRoyalty()
        {

        }

		private void ChangeTitles(string rt, List<RoyalTitleDef> titles)
        {
			// list of tuple (titleDef, maleTitle, femaleTitle)

		}

		private void ManageTitleLoc()
        {
			string realmType = "Kingdom";
			var titles = DefDatabase<RoyalTitleDef>.AllDefsListForReading;

			switch (realmType)
            {
				case "Kingdom":
					break;
				case "Empire":
					ChangeTitles(realmType, titles);
					break;
				default:
					Log.Error("Titular Royalty: Invalid RealmType saved");
					break;
			}
        }

		public void OnGameStart()
        {

        }

        public override void LoadedGame()
        {

        }

        public override void StartedNewGame()
        {

        }
    }
}