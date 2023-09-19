using System.Reflection;
using HarmonyLib;
using RimWorld;

namespace TitularRoyalty.Patches
{
    public static class HarmonyFields
    {
        public static FieldInfo CharacterCardUtility_TmpStackElements =
            AccessTools.Field(typeof(CharacterCardUtility), "tmpStackElements");
    }
}