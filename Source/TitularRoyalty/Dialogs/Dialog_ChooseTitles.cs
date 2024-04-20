using UnityEngine;

namespace TitularRoyalty;

public class Dialog_ChooseTitles : Window
{
    private readonly Pawn chosenPawn;
    private Vector2 scrollPosition = new Vector2(0, 0);
    private const int ColumnCount = 1;
    private readonly List<PlayerTitleDef> titlesBySeniority;

    public Dialog_ChooseTitles(Pawn targPawn)
    {
        chosenPawn = targPawn;
        doCloseX = true;
        doCloseButton = true;
        closeOnClickedOutside = true;

        titlesBySeniority = DefDatabase<PlayerTitleDef>.AllDefsListForReading.OrderBy(def => def.seniority).ToList();
    }
    private static string GetDisplayTitle(RoyalTitleDef titleDef, Gender gender)
    {
        // Prince-Consort doesn't fit in the GUI and Queen would show up twice
        if (titleDef == PlayerTitleDefOf.TitularRoyalty_T_RY_Consort)
        {
            if (titleDef.GetLabelFor(gender) == PlayerTitleDefOf.TitularRoyalty_T_RY_King.GetLabelFor(gender))
                return titleDef.GetLabelFor(gender) + "TR_consort".Translate();
        }
        // Only women use different titles
        if (gender == Gender.Female && titleDef.labelFemale != null)
            return titleDef.labelFemale;
        
        return titleDef.label;
    }
    public override Vector2 InitialSize => new Vector2(800f / 4f, 500f);
    public override void DoWindowContents(Rect inRect)
    {
        var outRect = new Rect(inRect);
        outRect.yMin += 30f;
        outRect.yMax -= 40f;

        if (titlesBySeniority.Count == 0)
        {
            Widgets.Label(new Rect(0, 10, 300f, 30f), "TR_NoTitles".Translate());
            Log.Error($"Titular Royalty: Couldn't fill title dialog, relevant variables for author: {titlesBySeniority.Count}");
            return;
        }

        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(new Rect(0, 10, inRect.width, 30f), "TR_choosetitle".Translate()); 
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        var viewRect = new Rect(0f, 30f, outRect.width - 16f, titlesBySeniority.Count / 4 * 128f + 256f);
        Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

        var rectIconFirst = new Rect(10, 20f, 80f, 24f);
        TooltipHandler.TipRegion(rectIconFirst, "TR_CurrentTitle".Translate());

        int i = 0;
        foreach (var titleDef in titlesBySeniority)
        {
            // work with pair.Key and pair.Value
            var iconRect = new Rect(
                32 * (i % ColumnCount) + 10,
                32 * (i / ColumnCount) + 32f,
                125f, 32f);
            i++;
                    
            TooltipHandler.TipRegion(iconRect, GetDisplayTitle(titleDef, chosenPawn!.gender));
            if (!Widgets.ButtonText(iconRect, GetDisplayTitle(titleDef, chosenPawn!.gender), drawBackground: true))
                break;
                    
            if (chosenPawn.royalty != null && chosenPawn.royalty.GetCurrentTitle(Faction.OfPlayer) != titleDef)
            {
                try
                {
                    chosenPawn.royalty.SetTitle(Faction.OfPlayer, titleDef, grantRewards: true, sendLetter: true);
                }
                catch (NullReferenceException ex)
                {
                    Log.Error($"Titular Royalty: Failed vanilla royalty set title\nException Info: {ex}");
                }
            }
            this.Close();
        }
            
        Widgets.EndScrollView();
    }
}