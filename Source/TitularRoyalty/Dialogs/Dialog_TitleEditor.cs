using UnityEngine;
using Verse.Grammar;

namespace TitularRoyalty;

public class Dialog_TitleEditor : Window
{
    public override Vector2 InitialSize => new Vector2(550, 410);

    private int MaxNameLength => 28;

    private string curName;
    private string curNameFemale;
    private string newName;
    private string newNameFemale;

    private bool isInheritable;
    private bool allowDignifiedMeditation;

    private string iconName;

    private ExpectationDef minExpectation;
    private TitleTiers titleTier;
    
    private RoyalTitleOverride originalOverrides;
    private readonly PlayerTitleDef titleDef;
    private readonly Dialog_ManageTitles manageTitles;

    public Dialog_TitleEditor(PlayerTitleDef titleDef, Dialog_ManageTitles manageTitles)
    {
        doCloseX = true;
        forcePause = true;
        draggable = true;
        closeOnClickedOutside = true;
        this.titleDef = titleDef;

        if (manageTitles != null)
        {
            this.manageTitles = manageTitles;
            this.manageTitles.titleEditorOpen = true;
        }

        originalOverrides = GameComponent_TitularRoyalty.Current.GetCustomTitleOverrideFor(this.titleDef);
        SetDefaultVariables();
    }

    public override void PreClose()
    {
        base.PreClose();
        if (manageTitles != null)
        {
            manageTitles.titleEditorOpen = false;
        }
    }

    private void SetDefaultVariables()
    {
        if (originalOverrides.label is "None" or null)
        {
            curName = titleDef.label;
            curNameFemale = titleDef.labelFemale;
        }
        else
        {
            curName = originalOverrides.label;

            if (originalOverrides.labelFemale == "None")
            {
                curNameFemale = null;
            }
            else
            {
                curNameFemale = originalOverrides.labelFemale ?? null;
            }
        }

        iconName = originalOverrides.iconName ?? titleDef.iconName;
        isInheritable = originalOverrides.TRInheritable ?? titleDef.TRInheritable;
        allowDignifiedMeditation =
            originalOverrides.allowDignifiedMeditationFocus ?? titleDef.allowDignifiedMeditationFocus;
        minExpectation = originalOverrides.minExpectation ?? titleDef.minExpectation ?? ExpectationDefOf.ExtremelyLow;
        titleTier = originalOverrides.titleTier ?? titleDef.titleTier;
    }

    public override void DoWindowContents(Rect inRect)
    {
        /* TITLE */
        var titleRect = new Rect(4, 0, inRect.width - 8, 40);

        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(titleRect, "TR_titleeditor_label".Translate());
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        /* CONTENT */
        var contentRect = new Rect(8, titleRect.yMax + GenUI.GapSmall / 2, inRect.width - 16,
            inRect.height - (titleRect.yMax + GenUI.GapSmall) - 35);
        var contentRectVisual = new Rect(contentRect);
        contentRectVisual.height -= 2;
        contentRect = contentRect.ContractedBy(2);
        contentRect.x += 2;

        Widgets.DrawTitleBG(contentRectVisual);
        Widgets.DrawBox(contentRectVisual, -1, BaseContent.WhiteTex);

        var listingStandard = new Listing_Standard();
        listingStandard.maxOneColumn = true;

        DrawListing(contentRect, listingStandard);

        /* FINALIZE BUTTONS */
        float iconWidth = 35;
        var bottomRect = new Rect(contentRectVisual.xMin, contentRectVisual.yMax + 1, contentRectVisual.width,
            iconWidth);
        bottomRect.SplitVertically(iconWidth, out var iconRect, out var bottomRowRect);

        Widgets.DrawTitleBG(bottomRect);
        Widgets.DrawBox(bottomRect, -1, BaseContent.WhiteTex);
        Widgets.DrawBox(iconRect, -1, BaseContent.WhiteTex);

        //var buttonsRect = bottomRowRect.RightPart(0.4f);
        var leftButtonRect = bottomRowRect.LeftHalf();
        var rightButtonRect = bottomRowRect.RightHalf();

        // Icon
        if (Widgets.ButtonImage(iconRect, Resources.CustomIcons.TryGetValue(iconName) ?? Resources.GetTitleIcon(titleDef) ?? BaseContent.BadTex))
        {
            var menuOptions = new List<FloatMenuOption>
            {
                new FloatMenuOption("Default", delegate { iconName = null; },
                    iconTex: Resources.GetTitleIcon(titleTier, GameComponent_TitularRoyalty.Current.realmTypeDef), iconColor: Color.white)
            };
            foreach (string key in Resources.CustomIcons.Keys)
            {
                menuOptions.Add(new FloatMenuOption(key, delegate { iconName = key; },
                    iconTex: Resources.CustomIcons.TryGetValue(key, BaseContent.BadTex), iconColor: Color.white));
            }

            Find.WindowStack.Add(new FloatMenu(menuOptions));
        }

        // Reset and Submit Buttons
        if (Widgets.ButtonText(leftButtonRect, "TR_titleeditor_reset".Translate(), false,
                overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            ResetTitleOverride();
            Messages.Message("Resetting Title", MessageTypeDefOf.NeutralEvent, false);
        }

        if (Widgets.ButtonText(rightButtonRect, "TR_titleeditor_submit".Translate(), false,
                overrideTextAnchor: TextAnchor.MiddleCenter))
        {
            TrySubmitTitleChanges();
            this.Close();
        }
    }

    private void DrawListing(Rect contentRect, Listing_Standard listingStandard)
    {
        listingStandard.Begin(contentRect);

        // Title Labels
        var namesRect = listingStandard.GetRect(35 + Text.CalcHeight("TR_titleeditor_title".Translate(), contentRect.width) + 10);
        DrawTitleNamesRow(namesRect);
        listingStandard.Gap(GenUI.GapSmall);

        // Dropdowns
        var dropdownRect = listingStandard.GetRect(35);
        DrawTiersAndExpectationsDropdowns(dropdownRect);
        listingStandard.Gap(GenUI.GapSmall);

        // Checkbox Settings
        listingStandard.Gap(GenUI.GapSmall / 2);

        Text.Anchor = TextAnchor.MiddleCenter;
        listingStandard.Label("TR_titleeditor_checkboxes_label".Translate());
        Text.Anchor = TextAnchor.UpperLeft;

        listingStandard.Gap(GenUI.GapSmall / 2);

        var checkboxRect = listingStandard.GetRect(28);
        DrawDoubleCheckboxRow(checkboxRect, "TR_titleeditor_checkbox_inheritable".Translate(), ref isInheritable,
            "TR_titleeditor_checkbox_allowmeditationfocus".Translate(), ref allowDignifiedMeditation);

        // End the List
        listingStandard.End();
    }

    private void DrawTiersAndExpectationsDropdowns(Rect dropdownRect)
    {
        var titleTiersRect = dropdownRect.LeftHalf();
        var minExpectationsRect = dropdownRect.RightHalf();

        titleTiersRect.width -= 8;
        minExpectationsRect.width -= 8;
        minExpectationsRect.x += 4;

        TooltipHandler.TipRegion(titleTiersRect, new TipSignal("TR_titleeditor_titletier_tooltip".Translate()));
        TooltipHandler.TipRegion(minExpectationsRect, new TipSignal("TR_titleeditor_expectations_tooltip".Translate()));

        if (Widgets.ButtonText(titleTiersRect, "TR_titleeditor_titletier".Translate(titleTier.ToString())))
        {
            var options = new List<FloatMenuOption>();
            
            foreach (TitleTiers tier in Enum.GetValues(typeof(TitleTiers))) 
                options.Add(new FloatMenuOption(tier.ToString(), delegate { titleTier = tier; }));

            Find.WindowStack.Add(new FloatMenu(options));
        }
        if (Widgets.ButtonText(minExpectationsRect, "TR_titleeditor_expectations".Translate(minExpectation.ToStringSafe())))
        {
            var options = new List<FloatMenuOption>();
            
            foreach (var expectation in DefDatabase<ExpectationDef>.AllDefsListForReading) 
                options.Add(new FloatMenuOption(expectation.LabelCap, delegate { minExpectation = expectation; }));

            Find.WindowStack.Add(new FloatMenu(options));
        }
    }

    private void DrawTitleNamesRow(Rect namesRect)
    {
        // Labels
        var labelsRect = namesRect.TopPartPixels(namesRect.height - 35 - (GenUI.GapSmall / 2));
        labelsRect.y += GenUI.GapSmall / 2;
        var maleLabelRect = labelsRect.LeftHalf();
        var femaleLabelRect = labelsRect.RightHalf();

        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(maleLabelRect, "TR_titleeditor_title".Translate());
        Widgets.Label(femaleLabelRect, "TR_titleeditor_titlefemale".Translate());
        Text.Anchor = TextAnchor.UpperLeft;

        // Input Rects
        var textFieldsRect = namesRect.BottomPartPixels(35);
        var maleNameRect = textFieldsRect.LeftHalf();
        var femaleNameRect = textFieldsRect.RightHalf();

        maleNameRect.width -= 8;
        femaleNameRect.width -= 8;
        femaleNameRect.x += 4;

        // Tooltips
        TooltipHandler.TipRegion(maleNameRect, new TipSignal("TR_titleeditor_title_tooltip".Translate()));
        TooltipHandler.TipRegion(femaleNameRect, new TipSignal("TR_titleeditor_titlefemale_tooltip".Translate()));

        // User Input
        curName = Widgets.TextField(maleNameRect, curName, MaxNameLength);
        curNameFemale = Widgets.TextField(femaleNameRect, curNameFemale, MaxNameLength);
        newName = curName?.Trim();
        newNameFemale = curNameFemale?.Trim();
    }

    private void DrawDoubleCheckboxRow(Rect checkboxRect, string checkbox1Label, ref bool checkbox1Value,
        string checkbox2Label, ref bool checkbox2Value)
    {
        var leftRect = checkboxRect.LeftHalf();
        var rightRect = checkboxRect.RightHalf();

        leftRect.width -= 8;
        rightRect.width -= 8;
        rightRect.x += 4;

        Widgets.CheckboxLabeled(leftRect, checkbox1Label, ref checkbox1Value, placeCheckboxNearText: false);
        Widgets.CheckboxLabeled(rightRect, checkbox2Label, ref checkbox2Value, placeCheckboxNearText: false);
    }

    private bool IsValidName(string name, bool femaleTitle = false)
    {
        if ((!femaleTitle && name.Length == 0) || name.Length > MaxNameLength || GrammarResolver.ContainsSpecialChars(name))
            return false;
        
        return true;
    }

    /* APPLY METHODS */
    private void ResetTitleOverride()
    {
        originalOverrides = new RoyalTitleOverride();
        GameComponent_TitularRoyalty.Current.SaveTitleChange(titleDef, originalOverrides);
        SetDefaultVariables();
    }

    private void TrySubmitTitleChanges()
    {
        /* LABELS */
        if (!IsValidName(newName))
        {
            Messages.Message("Titular Royalty: Male Title is Invalid, is it nothing?, is it over 64 characters or contains special characters?",
                MessageTypeDefOf.RejectInput, false);
            return;
        }
        if (!IsValidName(newNameFemale, femaleTitle: true))
        {
            Messages.Message("Titular Royalty: Female Title is Invalid, is it over 64 characters or contains special characters?",
                MessageTypeDefOf.RejectInput, false);
            return;
        }

        originalOverrides.label = newName;
        originalOverrides.labelFemale = newNameFemale == string.Empty ? "None" : newNameFemale;

        /* OTHER */
        originalOverrides.iconName = iconName.NullOrEmpty() ? originalOverrides.iconName : iconName;
        originalOverrides.TRInheritable = isInheritable;
        originalOverrides.titleTier = titleTier;
        originalOverrides.allowDignifiedMeditationFocus = allowDignifiedMeditation;
        originalOverrides.minExpectation = minExpectation ?? ExpectationDefOf.Low;

        GameComponent_TitularRoyalty.Current.SetupTitle(titleDef);
        Messages.Message("Titular Royalty: Change Title Success", MessageTypeDefOf.NeutralEvent, false);
    }
}