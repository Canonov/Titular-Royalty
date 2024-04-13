

// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedField.Global

namespace TitularRoyalty;

[DefOf]
public static class PlayerTitleDefOf
{
    public static PlayerTitleDef TitularRoyalty_T_RY_Consort;
    public static PlayerTitleDef TitularRoyalty_T_RY_King;

    static PlayerTitleDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(PlayerTitleDefOf));
    }
}