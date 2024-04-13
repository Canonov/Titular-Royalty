using System.IO;
using System.Text;
using LudeonTK;


namespace TitularRoyalty
{
    public static class DebugActions
    {
        [DebugAction("Titular Royalty", "TR: Edit Title", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void EditTitle()
        {
            var options = DefDatabase<PlayerTitleDef>.AllDefsListForReading
                .Select(title => new DebugMenuOption($"{title.GetLabelForBothGenders()}", DebugMenuOptionMode.Action,
                    delegate
                    {
                        var comp = Current.Game.GetComponent<GameComponent_TitularRoyalty>();
                        Find.WindowStack.Add(new Dialog_RoyalTitleEditor(comp, title, null));
                    }))
                .ToList();

            Find.WindowStack.Add(new Dialog_DebugOptionListLister(options));
        }

        [DebugAction("Titular Royalty", "TR: Reset Custom Titles",
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void TryResetCustomTitles()
        {
            Current.Game.GetComponent<GameComponent_TitularRoyalty>().ResetTitles();
        }

        [DebugAction("Titular Royalty", "TR: Refresh Titles", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void UpdateTitles()
        {
            Current.Game.GetComponent<GameComponent_TitularRoyalty>().SetupTitles();
        }

        [DebugAction("Titular Royalty", "Try Apply ModSettings", allowedGameStates = AllowedGameStates.Playing)]
        public static void ReloadSettings()
        {
            StartupSetup.ApplyModSettings();
        }

        [DebugAction("Titular Royalty", "Export Titlelist to Doc",
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void ExportTitlesToDoc()
        {
            var doc = new StringBuilder();

            foreach (var title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                doc.AppendLine($"  <li> <!--{title.originalTitleFields.label.CapitalizeFirst()}-->");
                doc.AppendLine($"    <titleDef>{title.defName}</titleDef>");
                doc.AppendLine($"    <label>{title.label}</label>");
                doc.AppendLine($"    <labelFemale>{title.labelFemale ?? "None"}</labelFemale>");
                doc.AppendLine($"  </li>");
            }

            File.WriteAllText($"RealmTypeList-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.xml", doc.ToString());
            Log.Message("Saved to your rimworld folder.");
        }
    }
}