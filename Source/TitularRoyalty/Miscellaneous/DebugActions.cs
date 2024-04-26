using System.IO;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace TitularRoyalty;

public static class DebugActions
{
    [DebugAction("Titular Royalty", "TR: Edit Title", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void EditTitle()
    {
        var titles = DefDatabase<PlayerTitleDef>.AllDefsListForReading;
        var options = titles.Select(title => new DebugMenuOption(title.GetLabelForBothGenders(), DebugMenuOptionMode.Action, () => 
            Find.WindowStack.Add(new Dialog_TitleEditor(title, null))
        ));

        Find.WindowStack.Add(new Dialog_DebugOptionListLister(options.ToList()));
    }

    [DebugAction("Titular Royalty", "TR: Reset Custom Titles", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void TryResetCustomTitles()
    {
        GameComponent_TitularRoyalty.Current.ResetTitles();
    }

    [DebugAction("Titular Royalty", "TR: Refresh Titles", allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void UpdateTitles()
    {
        GameComponent_TitularRoyalty.Current.SetupAllTitles();
    }

    [DebugAction("Titular Royalty", "Try Apply ModSettings", allowedGameStates = AllowedGameStates.Playing)]
    public static void ReloadSettings()
    {
        StartupSetup.ApplyModSettings();
    }

    [DebugAction("Titular Royalty", "Export Titlelist to Doc", allowedGameStates = AllowedGameStates.PlayingOnMap)]
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
        
        string exportsFolderPath = Path.Combine(GenFilePaths.ConfigFolderPath, "Titular Royalty", "Exports");
        string filePath = Path.Combine(exportsFolderPath, $"RealmType-{Faction.OfPlayer.Name}-{Rand.Int}.xml");
        try
        {
            File.WriteAllText(filePath, doc.ToString());
            Log.Message("Saved to your rimworld folder.");
        }
        catch (IOException)
        {
            Log.Error($"Failed exporting title list, ensure you have access to write.");
            throw;
        }
        
    }
}