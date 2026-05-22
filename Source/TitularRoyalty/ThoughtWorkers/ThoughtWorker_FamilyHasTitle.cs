namespace TitularRoyalty.ThoughtWorkers;

public class ThoughtWorker_FamilyHasTitle : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!TRSettings.familyTitleMoodBonus)
            return ThoughtState.Inactive;

        if (p?.Faction != Faction.OfPlayer)
            return ThoughtState.Inactive;

        PlayerTitleDef bestTitle = null;

        foreach (Pawn otherPawn in TitleThoughtUtility.TitledPlayerPawnsForReading)
        {
            if (!TitleThoughtUtility.PawnIsCloseFamily(p, otherPawn) ||
                !TitleThoughtUtility.TryGetPlayerTitle(otherPawn, out PlayerTitleDef titleDef))
            {
                continue;
            }

            if (bestTitle == null || titleDef.titleTier > bestTitle.titleTier)
            {
                bestTitle = titleDef;
            }
        }

        return bestTitle == null
            ? ThoughtState.Inactive
            : TitleThoughtUtility.ThoughtStateForTitle(bestTitle);
    }
}
