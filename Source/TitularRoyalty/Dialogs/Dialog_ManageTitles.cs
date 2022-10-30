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
        public override Vector2 InitialSize => new Vector2(400, 700);

        // TITLE LISTS
        public List<RoyalTitleDef> Titles
        {
            get
            {
                List<RoyalTitleDef> titles = new List<RoyalTitleDef>();
                foreach (RoyalTitleDef v in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
                {
                    //Log.Message($"Defname: {v.ToString()} Label: {v.label}");
                    /*foreach (var li in v.tags)
                    {
                        if (li.Contains("PlayerTitle"))
                        {
                            seniorityTitles.Add(v, v.seniority);
                        }
                    }*/
                    if (v.HasModExtension<AlternateTitlesExtension>() && v.tags.Contains("PlayerTitle"))
                    {
                        titles.Add(v);
                    }
                }
                return titles;
            }
        }
        public List<RoyalTitleDef> TitlesBySeniority
        {
            get
            {
                return Titles.OrderBy(o => o.seniority).ToList();
            }
        }

        // Variables for Updating and Setting the Lists
        private Listing_Standard TitleList;
        private Rect ContentRect;
        private Rect TitleRect;
        private Rect ButtonsRect;
        private Rect LeftButtonRect;
        private Rect RightButtonRect;
        private Color borderColor = new Color(97, 108, 122);

        // CONSTRUCTOR
        public Dialog_ManageTitles()
        {
            doCloseX = true;
            forcePause = true;
            draggable = true;
        }

        private void DoRow(Rect rect, RoyalTitleDef def)
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
            widgetRow.Icon(Resources.CrownIcon);
            //widgetRow.Gap(4f);

            float width = rect.width - widgetRow.FinalX - 4f - Text.CalcSize("TR_managetitles_rename".Translate()).x - 16f - 4f - Text.CalcSize("TR_managetitles_grant".Translate()).x - 16f - 4f;
            widgetRow.Label(def.GetLabelCapForBothGenders(), width);

            if (widgetRow.ButtonText("TR_managetitles_rename".Translate()))
            {
                Find.WindowStack.Add(new Dialog_TitleRenamer(def));
            }
            if (widgetRow.ButtonText("TR_managetitles_grant".Translate()))
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
        

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;

            //Title
            TitleRect = new Rect(4, 17, inRect.width - 8, 40);
            Widgets.DrawBox(TitleRect, -2, BaseContent.GreyTex);
            GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
            Widgets.Label(TitleRect, "TR_managetitles_title".Translate());
            Widgets.DrawTitleBG(TitleRect);
            GenUI.ResetLabelAlign();

            //Box for the Content
            ContentRect = new Rect(4, 57 + 7, inRect.width - 8, (30 * Titles.Count) + 12 + 30 + 6); //old height inRect.height - (57 + 7) - 40
            Widgets.DrawTitleBG(ContentRect);
            Widgets.DrawBox(ContentRect, -2, BaseContent.GreyTex);

            //List of Titles
            TitleList = new Listing_Standard();
            TitleList.ColumnWidth = ContentRect.width;

            //Buttom 3 Buttons - Update Titles, 
            ButtonsRect = new Rect(ContentRect.xMin, ContentRect.yMax - 36, ContentRect.width, 30);
            //Widgets.DrawBoxSolid(buttonsRect, Color.red);
            //Widgets.BeginGroup(buttonsRect);

            LeftButtonRect = ButtonsRect.LeftHalf();
            LeftButtonRect.width -= 18;
            LeftButtonRect.x += 6;
            RightButtonRect = ButtonsRect.RightHalf();
            RightButtonRect.width -= 18;
            RightButtonRect.x += 12 - 6;

            DoTitleList();

            if (Widgets.ButtonText(LeftButtonRect, "TR_managetitles_update".Translate()))
            {
                Current.Game.GetComponent<GameComponent_TitularRoyalty>().ManageTitleLoc();
                Messages.Message("TR_managetitles_update_notif".Translate(), MessageTypeDefOf.NeutralEvent);
                DoTitleList();
            }
            if (Widgets.ButtonText(RightButtonRect, "TR_managetitles_resetcustom".Translate()))
            {
                Current.Game.GetComponent<GameComponent_TitularRoyalty>().ResetTitles();
                Messages.Message("TR_managetitles_resetcustom_notif".Translate(), MessageTypeDefOf.NeutralEvent);
            }
            TooltipHandler.TipRegion(LeftButtonRect, "TR_managetitles_update_tooltip".Translate());
            TooltipHandler.TipRegion(RightButtonRect, "TR_managetitles_resetcustom_tooltip".Translate());
            //Widgets.EndGroup();

        }
    }
}
