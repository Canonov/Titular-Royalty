using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using static UnityEngine.GraphicsBuffer;

namespace TitularRoyalty
{

    public class Dialog_ManageTitles : Window
    {
        // WINDOW OPTIONS
        public Vector2 scrollPosition = new Vector2(0, 0); 
        public override Vector2 InitialSize => new Vector2(475, 720);

        // TITLE LISTS
        public List<PlayerTitleDef> Titles
        {
            get
            {
                return DefDatabase<PlayerTitleDef>.AllDefsListForReading;
            }
        }

        private List<PlayerTitleDef> titlesBySeniority;
        public List<PlayerTitleDef> TitlesBySeniority
        {
            get
            {
                return titlesBySeniority ??= Titles.OrderBy(x => x.seniority).ToList();
            }
        }

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
        private GameComponent_TitularRoyalty TRComponent;
        private Vector2 titleListScrollPos = new Vector2(0, 0);

        // CONSTRUCTOR
        public Dialog_ManageTitles()
        {
            doCloseX = true;
            forcePause = true;
            draggable = true;
            TRComponent = Current.Game.GetComponent<GameComponent_TitularRoyalty>();
        }

        /// <summary>
        /// Creates a new row for a title
        /// </summary>
        /// <param name="rect">Rect to draw on</param>
        /// <param name="def">Def to display information for</param>
        private void DoRow(Rect rect, PlayerTitleDef def)
        {
            // Copied from the Area manager lol
            if (Mouse.IsOver(rect))
            {
                GUI.color = Color.grey;
                Widgets.DrawHighlight(rect);
                GUI.color = Color.white;

                if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                {
                    List<FloatMenuOption> options = new List<FloatMenuOption>();

                    // Grant Title
					options.Add(new FloatMenuOption("TR_managetitles_m2option_granttitle".Translate(), delegate
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate (LocalTargetInfo targetInfo) {
							targetInfo.Pawn?.royalty?.SetTitle(Faction.OfPlayer, def, true, false, true);
						});
						Close();
					}, itemIcon: Resources.TRCrownWidget, iconColor: Color.white));

                    // Edit Title
					options.Add(new FloatMenuOption("TR_managetitles_m2option_edittitle".Translate(), delegate
					{
						Find.WindowStack.Add(new Dialog_RoyalTitleEditor(TRComponent, def, this));
					}, itemIcon: TexButton.Rename, iconColor: Color.white));

                    // Reset Title
					options.Add(new FloatMenuOption("TR_managetitles_m2option_resettitle".Translate(), delegate
                    {
                        TRComponent.SaveTitleChange(def, new RoyalTitleOverride());
					}, itemIcon: TexButton.RenounceTitle, iconColor: Color.white));

					Find.WindowStack.Add(new FloatMenu(options));
					Event.current.Use();
                }
            }

            Widgets.BeginGroup(rect);

            WidgetRow widgetRow = new WidgetRow(0f, (rect.height - 24f) / 2);
            widgetRow.Gap(4f);
            widgetRow.Icon(Resources.GetTitleIcon(def, TRComponent) ?? BaseContent.BadTex);
            //widgetRow.Gap(4f);

            float width = rect.width - widgetRow.FinalX - 28f - (Text.CalcSize("TR_managetitles_edit".Translate()).x + 6) - (Text.CalcSize("TR_managetitles_grant".Translate()).x + 6);
            widgetRow.Label(def.GetLabelCapForBothGenders(), width, def.GetLabelCapForBothGenders());

            if (widgetRow.ButtonText("TR_managetitles_edit".Translate(), active: !titleEditorOpen)) // Rename Button, opens the title renamer
            {
                //Find.WindowStack.Add(new Dialog_TitleRenamer(def));
                Find.WindowStack.Add(new Dialog_RoyalTitleEditor(TRComponent, def, this));
            }
            if (widgetRow.ButtonText("TR_managetitles_grant".Translate(), active: !titleEditorOpen)) // Grant Button, starts a new targeter, closes the window and grants the title to who you select
            {
                Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate (LocalTargetInfo targetInfo)
                {
					targetInfo.Pawn?.royalty?.SetTitle(Faction.OfPlayer, def, true, false, true);
				});;
                Close();
            }
           
            Widgets.EndGroup();
        }

        /// <summary>
        /// (Re)loads the title list
        /// </summary>
        private void DoTitleList()
        {
            TitleList.Begin(ContentViewRect);
            TitleList.ColumnWidth = ContentViewRect.width;

            TitleList.Gap(6f);

            foreach (var title in TitlesBySeniority)
            {
	            DoRow(TitleList.GetRect(28f), title);
	            //TitleList.Gap(6f);
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
        
        /// <summary>
        /// Draw the UI
        /// </summary>
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
            var realmtypeoptions = new List<FloatMenuOption>();
            if (Widgets.ButtonText(RealmTypeButtonRect, TRComponent.RealmTypeDef.label, active: !titleEditorOpen)) 
            {
                foreach (RealmTypeDef rtdef in DefDatabase<RealmTypeDef>.AllDefsListForReading)
                {
                    realmtypeoptions.Add(new FloatMenuOption(rtdef.label, delegate ()
                    {
                        TRComponent.RealmTypeDefName = rtdef.defName;
                        Messages.Message("TR_realmtypechanged_notify".Translate(), MessageTypeDefOf.NeutralEvent);
                    }, rtdef.Icon ?? BaseContent.BadTex, Color.white));   
                }
                realmtypeoptions = realmtypeoptions.OrderBy(x => x.Label).ToList(); //Sort Alphabetically
                Find.WindowStack.Add(new FloatMenu(realmtypeoptions));
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
            ContentViewRect.height = GetContentHeight(TitlesBySeniority.Count);

            Widgets.BeginScrollView(ContentRect, ref titleListScrollPos, ContentViewRect);
            DoTitleList();
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

            //Log.Message((ButtonsRect.yMax + GenUI.Gap).ToString());

            // Buttons
            if (Widgets.ButtonText(LeftButtonRect, "TR_managetitles_update".Translate(), active: !titleEditorOpen))
            {
                TRComponent.SetupTitles();
                titlesBySeniority = null;
                Messages.Message("TR_managetitles_update_notif".Translate(), MessageTypeDefOf.NeutralEvent);
                DoTitleList();
            }
            if (Widgets.ButtonText(RightButtonRect, "TR_managetitles_resetcustom".Translate(), active: !titleEditorOpen))
            {
                TRComponent.ResetTitles();
                Messages.Message("TR_managetitles_resetcustom_notif".Translate(), MessageTypeDefOf.NeutralEvent);
            }
            TooltipHandler.TipRegion(LeftButtonRect, "TR_managetitles_update_tooltip".Translate());
            TooltipHandler.TipRegion(RightButtonRect, "TR_managetitles_resetcustom_tooltip".Translate());
            //Widgets.EndGroup();

        }
    }
}
