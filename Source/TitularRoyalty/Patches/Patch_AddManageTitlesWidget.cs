using HarmonyLib;

namespace TitularRoyalty.Patches;

[HarmonyPatch]
public class Patch_AddManageTitlesWidget
{
	[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	[HarmonyPostfix]
	private static void PlaySettings_DoPlaySettingsGlobalControls_Postfix(WidgetRow row, bool worldView)
	{
		if (!worldView && row.ButtonIcon(BaseContent.BadTex, "Open Titular Royalty Manager"))
		{
			Log.Warning("Not Yet Readded"); // todo 
			//Find.WindowStack.Add(new Dialog_ManageTitles());
		}
	}
}