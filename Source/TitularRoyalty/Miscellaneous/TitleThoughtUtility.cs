using System.Diagnostics.CodeAnalysis;

namespace TitularRoyalty;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public static class TitleThoughtUtility
{
    private const int TitledPlayerPawnsCacheTicks = 250;

    private static readonly HashSet<PawnRelationDef> CloseFamilyRelations =
    [
        PawnRelationDefOf.Parent,
        PawnRelationDefOf.Child,
        PawnRelationDefOf.Sibling,
        PawnRelationDefOf.HalfSibling,
        PawnRelationDefOf.Spouse,
        PawnRelationDefOf.Fiance,
        PawnRelationDefOf.Lover
    ];

    private static readonly List<Pawn> TitledPlayerPawns = [];
    private static int titledPlayerPawnsCacheTick = -999999;

    public static List<Pawn> TitledPlayerPawnsForReading
    {
        get
        {
            var ticksGame = Find.TickManager?.TicksGame ?? 0;
            if (ticksGame - titledPlayerPawnsCacheTick >= TitledPlayerPawnsCacheTicks)
            {
                RebuildTitledPlayerPawnsCache(ticksGame);
            }

            return TitledPlayerPawns;
        }
    }

    public static void Notify_TitlesChanged()
    {
        titledPlayerPawnsCacheTick = -999999;
    }

    public static bool TryGetPlayerTitle(Pawn pawn, out PlayerTitleDef titleDef)
    {
        titleDef = null;

        if (pawn?.Faction != Faction.OfPlayer || pawn?.royalty == null)
            return false;

        titleDef = pawn.royalty.GetCurrentTitle(Faction.OfPlayer) as PlayerTitleDef;
        return titleDef != null;
    }

    public static ThoughtState ThoughtStateForTitle(PlayerTitleDef titleDef)
    {
        return ThoughtState.ActiveAtStage((int)titleDef.titleTier);
    }

    public static bool PawnIsCloseFamily(Pawn pawn, Pawn otherPawn)
    {
        if (pawn == null || otherPawn == null || pawn == otherPawn)
        {
            return false;
        }

        return pawn.GetRelations(otherPawn)
            .Any(relation => CloseFamilyRelations.Contains(relation));
    }

    private static void RebuildTitledPlayerPawnsCache(int ticksGame)
    {
        TitledPlayerPawns.Clear();
        titledPlayerPawnsCacheTick = ticksGame;

        foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Where(pawn =>
                     TryGetPlayerTitle(pawn, out _)))
        {
            TitledPlayerPawns.Add(pawn);
        }
    }
}
