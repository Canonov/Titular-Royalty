namespace TitularRoyalty.ThoughtWorkers;

public class ThoughtWorker_HasTitle : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!TRSettings.titleHolderMoodBonus)
            return ThoughtState.Inactive;

        return TitleThoughtUtility.TryGetPlayerTitle(p, out PlayerTitleDef titleDef)
            ? TitleThoughtUtility.ThoughtStateForTitle(titleDef)
            : ThoughtState.Inactive;
    }
}
