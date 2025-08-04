using HarmonyLib;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

namespace TitularRoyalty.Patches;

[HarmonyPatch(typeof(ThingDef), nameof(ThingDef.IsRangedWeapon), MethodType.Getter)]
public static class Patch_ThingDef_IsRangedWeapon
{
    // Hack fix for NiceBillTab
    private static bool Prefix(ThingDef __instance, ref bool __result)
    {
        if (__instance.defName == "TitularRoyalty_Staff")
        {
            __result = false;
            return false;
        }

        return true;
    }
}