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
        public override Vector2 InitialSize => new Vector2(400, 747);

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

        // Variables for Updating and Setting the Lists
        private Listing_Standard TitleList;
        private Rect ContentRect;
        private Rect TitleRect;
        private Rect RealmTypeRect;
        private Rect RealmTypeButtonRect;
        private Rect ButtonsRect;
        private Rect LeftButtonRect;
        private Rect RightButtonRect;
        private GameComponent_TitularRoyalty TRComponent;
        private bool isWorldView = false;

        // CONSTRUCTOR
        public Dialog_ManageTitles(bool Worldview = false)
        {
            doCloseX = true;
            forcePause = true;
            draggable = true;
            isWorldView = Worldview;
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
            }

            Widgets.BeginGroup(rect);
            WidgetRow widgetRow = new WidgetRow(0f, 0f);
            widgetRow.Gap(4f);
            widgetRow.Icon(Resources.TierIconsForGovernment(TRComponent.RealmTypeDef.governmentType)[(int)def.titleTier]);
            //widgetRow.Gap(4f);

            float width = rect.width - widgetRow.FinalX - 4f - Text.CalcSize("TR_managetitles_rename".Translate()).x - 16f - 4f - Text.CalcSize("TR_managetitles_grant".Translate()).x - 16f - 4f;
            widgetRow.Label(def.GetLabelCapForBothGenders(), width, def.GetLabelCapForBothGenders());

            if (widgetRow.ButtonText("TR_managetitles_rename".Translate())) // Rename Button, opens the title renamer
            {
                Find.WindowStack.Add(new Dialog_TitleRenamer(def));
            }
            if (widgetRow.ButtonText("TR_managetitles_grant".Translate(), active: !isWorldView)) // Grant Button, starts a new targeter, closes the window and grants the title to who you select
            {
                Action<LocalTargetInfo> action = delegate (LocalTargetInfo targetinfo)
                {
                    if (targetinfo.Pawn.royalty != null)
                    {
                        targetinfo.Pawn.royalty.SetTitle(Faction.OfPlayer, def, grantRewards: true, sendLetter: true);
                    }
                    else
                    {
                        return;
                    }
                };
                Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), action);
                Close();
            }
           
            Widgets.EndGroup();
        }

        /// <summary>
        /// (Re)loads the title list
        /// </summary>
        public void DoTitleList()
        {
            TitleList.Begin(ContentRect);
            TitleList.Gap(6f);

            for (int j = 0; j < TitlesBySeniority.Count; j++)
            {
                DoRow(TitleList.GetRect(24f), TitlesBySeniority[j]);
                TitleList.Gap(6f);
            }

            TitleList.Gap(6f);
            TitleList.End();
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
            Rect RealmTypeLabelRect = RealmTypeRect.LeftHalf().ContractedBy(4);
            Widgets.Label(RealmTypeLabelRect, "TR_realmtype".Translate() + ":");
            GenUI.ResetLabelAlign();

            //Button with Dropdown to select titleset
            RealmTypeButtonRect = RealmTypeRect.RightHalf().ContractedBy(4);
            List<FloatMenuOption> realmtypeoptions = new List<FloatMenuOption>();
            if (Widgets.ButtonText(RealmTypeButtonRect, TRComponent.RealmTypeDef.label)) 
            {
                foreach (RealmTypeDef rtdef in DefDatabase<RealmTypeDef>.AllDefsListForReading)
                {
                    realmtypeoptions.Add(new FloatMenuOption(rtdef.label, delegate ()
                    {
                        TRComponent.RealmType = rtdef.defName;
                        Messages.Message("TR_realmtypechanged_notify".Translate(), MessageTypeDefOf.NeutralEvent);
                    }));   
                }
                realmtypeoptions = realmtypeoptions.OrderBy(x => x.Label).ToList(); //Sort Alphabetically
                Find.WindowStack.Add(new FloatMenu(realmtypeoptions));
            }
            

            //Box for the Content
            ContentRect = new Rect(4, RealmTypeRect.y + RealmTypeRect.height + 7, inRect.width - 8, (30 * Titles.Count) + 12 + 30 + 6);
            Widgets.DrawTitleBG(ContentRect);
            Widgets.DrawBox(ContentRect, -2, BaseContent.GreyTex);

            //List of Titles
            TitleList = new Listing_Standard();
            TitleList.ColumnWidth = ContentRect.width;

            //Buttom 3 Buttons - Update Titles, 
            ButtonsRect = new Rect(ContentRect.xMin, ContentRect.yMax - 36, ContentRect.width, 30);

            LeftButtonRect = ButtonsRect.LeftHalf();
            LeftButtonRect.width -= 18;
            LeftButtonRect.x += 6;
            RightButtonRect = ButtonsRect.RightHalf();
            RightButtonRect.width -= 18;
            RightButtonRect.x += 12 - 6;

            DoTitleList();

            if (Widgets.ButtonText(LeftButtonRect, "TR_managetitles_update".Translate()))
            {
                TRComponent.SetupTitles();
                titlesBySeniority = null;
                Messages.Message("TR_managetitles_update_notif".Translate(), MessageTypeDefOf.NeutralEvent);
                DoTitleList();
            }
            if (Widgets.ButtonText(RightButtonRect, "TR_managetitles_resetcustom".Translate()))
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
