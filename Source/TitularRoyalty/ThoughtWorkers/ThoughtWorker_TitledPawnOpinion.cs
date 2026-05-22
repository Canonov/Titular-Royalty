namespace TitularRoyalty.ThoughtWorkers;

public class ThoughtWorker_TitledPawnOpinion : ThoughtWorker
{
    protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
    {
        if (p == otherPawn || p?.Faction != Faction.OfPlayer)
            return ThoughtState.Inactive;

        return TitleThoughtUtility.TryGetPlayerTitle(otherPawn, out PlayerTitleDef titleDef)
            ? TitleThoughtUtility.ThoughtStateForTitle(titleDef)
            : ThoughtState.Inactive;
    }
}
