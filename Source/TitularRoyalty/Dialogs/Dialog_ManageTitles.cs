using UnityEngine;

namespace TitularRoyalty;

public class Dialog_ManageTitles : Window
{
    // WINDOW OPTIONS
    public Vector2 scrollPosition = new Vector2(0, 0); 
    public override Vector2 InitialSize => new Vector2(475, 720);

    public bool titleEditorOpen = false;

    // Variables for Updating and Setting the Lists
    private Listing_Standard TitleList;
    private Rect ContentRect;
    private Rect ContentViewRect;
    private Rect TitleRect;
    private Rect RealmTypeRect;
    private Rect RealmTypeButtonRect;
    private Rect ButtonsRect;
    private Rect LeftButtonRect;
    private Rect RightButtonRect;
    private Vector2 titleListScrollPos = new Vector2(0, 0);

    // CONSTRUCTOR
    public Dialog_ManageTitles()
    {
        doCloseX = true;
        forcePause = true;
        draggable = true;
    }
    
    private void DrawRow(Rect rect, PlayerTitleDef titleDef)
    {
        // Copied from the Area manager lol
        if (Mouse.IsOver(rect))
        {
            GUI.color = Color.grey;
            Widgets.DrawHighlight(rect);
            GUI.color = Color.white;
                
            // Right Click Options
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                DoRightClickMenu(titleDef);
            }
        }

        Widgets.BeginGroup(rect);

        var widgetRow = new WidgetRow(0f, (rect.height - 24f) / 2);
        widgetRow.Gap(4f);
        widgetRow.Icon(Resources.GetTitleIcon(titleDef) ?? BaseContent.BadTex);

        float width = rect.width - widgetRow.FinalX - 28f
                      - (Text.CalcSize("TR_managetitles_edit".Translate()).x + 6)
                      - (Text.CalcSize("TR_managetitles_grant".Translate()).x + 6);
            
        widgetRow.Label(titleDef.GetLabelCapForBothGenders(), width, titleDef.GetLabelCapForBothGenders());

        // Edit Button, opens the title editor window
        if (widgetRow.ButtonText("TR_managetitles_edit".Translate(), active: !titleEditorOpen)) 
        {
            Find.WindowStack.Add(new Dialog_TitleEditor(titleDef, this));
        }
        // Grant Button, opens a targeter, closes the window and grants the title to who you select
        if (widgetRow.ButtonText("TR_managetitles_grant".Translate(), active: !titleEditorOpen))
        {
            Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate (LocalTargetInfo targetInfo)
            {
                targetInfo.Pawn?.royalty?.SetTitle(Faction.OfPlayer, title: titleDef, grantRewards: true);
            });
            this.Close();
        }
           
        Widgets.EndGroup();
    }

    private void DoRightClickMenu(PlayerTitleDef titleDef)
    {
        var menuOptions = new List<FloatMenuOption>
        {
            // Grant Title
            new FloatMenuOption("TR_managetitles_m2option_granttitle".Translate(), delegate
            {
                Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(),
                    action: delegate(LocalTargetInfo targetInfo)
                    {
                        targetInfo.Pawn?.royalty?.SetTitle(Faction.OfPlayer, titleDef, true);
                    });
                this.Close();
            }, iconTex: Resources.TRCrownWidget, iconColor: Color.white),
                    
            // Edit Title
            new FloatMenuOption("TR_managetitles_m2option_edittitle".Translate(),
                action: delegate
                {
                    Find.WindowStack.Add(new Dialog_TitleEditor(titleDef, this));
                },
                iconTex: TexButton.Rename, iconColor: Color.white),
                    
            // Reset Title
            new FloatMenuOption("TR_managetitles_m2option_resettitle".Translate(),
                action: delegate
                {
                    GameComponent_TitularRoyalty.Current.SaveTitleChange(titleDef, new RoyalTitleOverride());
                },
                iconTex: TexButton.RenounceTitle, iconColor: Color.white)
        };

        Find.WindowStack.Add(new FloatMenu(menuOptions));
        Event.current.Use();
    }

    /// <summary>
    /// (Re)loads the title list
    /// </summary>
    private void DrawTitleList()
    {
        TitleList.Begin(ContentViewRect);
        TitleList.ColumnWidth = ContentViewRect.width;

        TitleList.Gap(6f);

        foreach (var title in TitleDatabase.TitlesBySeniority)
        {
            DrawRow(TitleList.GetRect(28f), title);
        }

        TitleList.Gap(6f);
        TitleList.End();
    }

    private static float GetContentHeight(int rowCount)
    {
        float result = 6 * 2; // Gaps
        result += 28 * rowCount; // Row

        return result;
    }

    public override void DoWindowContents(Rect inRect)
    {
        Text.Font = GameFont.Medium;

        //Title
        TitleRect = new Rect(4, 17, inRect.width - 8, 40);
        //Widgets.DrawBox(TitleRect, -2, BaseContent.GreyTex);
        GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
        Widgets.Label(TitleRect, "TR_managetitles_title".Translate());
        //Widgets.DrawTitleBG(TitleRect);

        //RealmType
        RealmTypeRect = new Rect(4, TitleRect.y + TitleRect.height + 7, inRect.width - 8, 40);
        Widgets.DrawBox(RealmTypeRect, -2, BaseContent.GreyTex);
        Widgets.DrawTitleBG(RealmTypeRect);

        //Titleset: 
        Text.Font = GameFont.Medium;
        var realmTypeLabelRect = RealmTypeRect.LeftHalf().ContractedBy(4);
        Widgets.Label(realmTypeLabelRect, "TR_realmtype".Translate() + ":");
        GenUI.ResetLabelAlign();

        //Button with Dropdown to select titleset
        RealmTypeButtonRect = RealmTypeRect.RightHalf().ContractedBy(4);
        var realmTypeOptions = new List<FloatMenuOption>();
            
        if (Widgets.ButtonText(RealmTypeButtonRect, GameComponent_TitularRoyalty.Current.realmTypeDef.label, active: !titleEditorOpen)) 
        {
            foreach (var realmTypeDef in DefDatabase<RealmTypeDef>.AllDefsListForReading)
            {
                realmTypeOptions.Add(new FloatMenuOption(realmTypeDef.label, delegate
                {
                    GameComponent_TitularRoyalty.Current.realmTypeDef = realmTypeDef;
                    Messages.Message("TR_realmtypechanged_notify".Translate(), MessageTypeDefOf.NeutralEvent);
                }, realmTypeDef.Icon ? realmTypeDef.Icon : BaseContent.BadTex, Color.white));   
            }
            realmTypeOptions = realmTypeOptions.OrderBy(x => x.Label).ToList(); //Sort Alphabetically
            Find.WindowStack.Add(new FloatMenu(realmTypeOptions));
        }

        //Box for the Content
        ContentRect = new Rect(4, RealmTypeRect.y + RealmTypeRect.height + 7, inRect.width - 8, 516);
        Widgets.DrawTitleBG(ContentRect);
        Widgets.DrawBox(ContentRect, -2, BaseContent.GreyTex);

        //List of Titles
        TitleList = new Listing_Standard();

        //Titles List
        ContentViewRect = new Rect(ContentRect);
        ContentViewRect.width -= 20f;
        ContentViewRect.height = GetContentHeight(TitleDatabase.Titles.Count);

        Widgets.BeginScrollView(ContentRect, ref titleListScrollPos, ContentViewRect);
        DrawTitleList();
        Widgets.EndScrollView();

        //Buttons - Update Titles, 
        ButtonsRect = new Rect(ContentRect.xMin, ContentRect.yMax + 7, ContentRect.width, 40);

        Widgets.DrawTitleBG(ButtonsRect);
        Widgets.DrawBox(ButtonsRect, -2, BaseContent.GreyTex);

        LeftButtonRect = ButtonsRect.LeftHalf();

        LeftButtonRect.height -= 10;
        LeftButtonRect.y += 5;
        LeftButtonRect.width -= 18;
        LeftButtonRect.x += 8;

        RightButtonRect = ButtonsRect.RightHalf();

        RightButtonRect.height -= 10;
        RightButtonRect.y += 5;
        RightButtonRect.width -= 18;
        RightButtonRect.x += 4;

        // Buttons
        if (Widgets.ButtonText(LeftButtonRect, "TR_managetitles_update".Translate(), active: !titleEditorOpen))
        {
            GameComponent_TitularRoyalty.Current.SetupAllTitles();
            DrawTitleList();
            Messages.Message("TR_managetitles_update_notif".Translate(), MessageTypeDefOf.NeutralEvent);
        }
        if (Widgets.ButtonText(RightButtonRect, "TR_managetitles_resetcustom".Translate(), active: !titleEditorOpen))
        {
            GameComponent_TitularRoyalty.Current.ResetTitles();
            Messages.Message("TR_managetitles_resetcustom_notif".Translate(), MessageTypeDefOf.NeutralEvent);
        }
        TooltipHandler.TipRegion(LeftButtonRect, "TR_managetitles_update_tooltip".Translate());
        TooltipHandler.TipRegion(RightButtonRect, "TR_managetitles_resetcustom_tooltip".Translate());
    }
}