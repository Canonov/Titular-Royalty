using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace TitularRoyalty { 

	public static class DebugActionsTitularRoyalty
	{

		[DebugAction("Mods", "TR: Rename Title", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void TryChangeCustomTitle()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PlayerTitleDef title in Faction.OfPlayer.def.RoyalTitlesAllInSeniorityOrderForReading)
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
        public static void TryResetCustomTitles()
		{
			Current.Game.GetComponent<GameComponent_TitularRoyalty>().ResetTitles();
		}

        [DebugAction("Mods", "TR: Refresh Titles", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void UpdateTitles()
		{
			Current.Game.GetComponent<GameComponent_TitularRoyalty>().SetupTitles();
		}

        [DebugAction("Mods", "TR: Try Apply ModSettings", false, false, allowedGameStates = AllowedGameStates.Playing)]
        public static void ReloadSettings()
        {
			ModSettingsApplier.ApplySettings();
        }
		
    }

}