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

		private void ManageTitleLoc()
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

			foreach (RoyalTitleDef v in titles)
			{
				if (v.modExtensions != null) { 
					foreach (AlternateTitlesExtension ext in v.modExtensions)
					{
						if (ext.realmType == realmType)
						{
							// Female Labels
							if (v.labelFemale != null && ext.labelf != "none")
							{
								v.labelFemale = ext.labelf;
							}
							else if (v.labelFemale != null && ext.labelf == "none")
							{
								v.labelFemale = null;
								//v.labelFemale = ext.label;
							}
							else if (v.labelFemale == null && ext.labelf != "none")
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
		}

		public void ChangeFactionForPermits(Faction playerFaction)
        {
			Log.Message($"ran ChangeFactionforPermits with arg {playerFaction} : {playerFaction.def.defName}");
			foreach (RoyalTitlePermitDef permit in DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading)
            {
				if (permit.HasModExtension<PlayerPermitExtension>())
                {
					try
					{
						permit.faction.defName = playerFaction.def.defName;
					}
					catch
					{
						Log.Error($"Failed to change permit {permit.defName}'s faction from {permit.faction.defName} to {playerFaction.def.defName}");
					}
				}

				try
				{
					Log.Message($"2: {permit.defName} | {permit.faction.defName}");
					Log.Message($"2: {permit.faction.defName}");
				}
				catch
				{
					Log.Warning("failed 2");
				}
			}
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