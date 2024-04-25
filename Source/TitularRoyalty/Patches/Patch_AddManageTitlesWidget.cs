using HarmonyLib;

namespace TitularRoyalty.Patches;

[HarmonyPatch]
public class Patch_AddManageTitlesWidget
{
	[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	[HarmonyPostfix]
	private static void PlaySettings_DoPlaySettingsGlobalControls_Postfix(WidgetRow row, bool worldView)
	{
		if (!worldView && row.ButtonIcon(Resources.TRWidget, "Open Titular Royalty Manager"))
		{
			Find.WindowStack.Add(new Dialog_ManageTitles());
		}
	}
}