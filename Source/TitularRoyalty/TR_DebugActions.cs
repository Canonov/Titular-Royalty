using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace TitularRoyalty { 

	public static class DebugActionsTitularRoyalty
	{

		[DebugAction("Mods", "TR: Try to change title", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryChangeCustomTitle()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (RoyalTitleDef title in Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading)
			{
				list.Add(new DebugMenuOption($"{title.GetLabelForBothGenders()}", DebugMenuOptionMode.Action, delegate
				{
					//Current.Game.GetComponent<GameComponent_TitularRoyalty>();
					Find.WindowStack.Add(new Dialog_TitleRenamer(title));
				}
				));

			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[DebugAction("Mods", "TR: Reset Custom Titles", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryResetCustomTitles()
		{
			Current.Game.GetComponent<GameComponent_TitularRoyalty>().ResetTitles();
		}

        [DebugAction("Mods", "TR: Update Titles", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void UpdateTitles()
		{
			Current.Game.GetComponent<GameComponent_TitularRoyalty>().ManageTitleLoc();
		}
	}

}