using HarmonyLib;
using UnityEngine;

namespace TitularRoyalty.Patches;

[HarmonyPatchCategory("VFEEmpire")]
[HarmonyPatch(typeof(Widgets), nameof(Widgets.DefIcon))]
public static class Patch_RoyalTitleIcons
{
	private static bool Prefix(Rect rect, Def def, float scale = 1f)
	{
		if (def is not PlayerTitleDef titleDef) 
			return true;
		
		var icon = Resources.GetTitleIcon(titleDef);
		
		if (icon != null)
		{
			Widgets.DrawTextureFitted(rect, icon, scale);
			return false;
		}

		Log.Message("Titular Royalty: Could not find icon for " + titleDef.defName + " in DefIcon_RoyalIconsPrefix.");
		return true;
	}
}