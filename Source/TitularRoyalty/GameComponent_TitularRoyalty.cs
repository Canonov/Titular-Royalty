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



		public void HandleTitle(List<RoyalTitleDef> titles, string rt)
        {
			if (rt == "Kingdom")
            {
				return;
			}

			//string defaultLabel = title.label;
			//string defaultLabelF = "none";
			//if (title.labelFemale != null) { defaultLabelF = title.labelFemale; }

			foreach (RoyalTitleDef v in titles)
			{

				foreach (AlternateTitlesExtension ext in v.modExtensions)
                {
					if (ext.realmType == rt)
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

		private void ManageTitleLoc()
        {
			string realmType = "Kingdom";
			var titles = DefDatabase<RoyalTitleDef>.AllDefsListForReading;

			switch (realmType)
            {
				case "Kingdom":
					break;
				case "Empire":
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