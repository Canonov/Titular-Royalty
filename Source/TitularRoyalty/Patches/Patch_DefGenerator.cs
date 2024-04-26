using HarmonyLib;

namespace TitularRoyalty.Patches;

[HarmonyPatch(typeof(DefGenerator))]
public static class Patch_DefGenerator
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
    public static void GenerateImpliedDefs_PreResolve_Postfix()
    {
        foreach (var titleDef in TitleDefGenerator.GeneratedPlayerTitleDefs())
        {
            DefGenerator.AddImpliedDef(titleDef);
        }
    }
}