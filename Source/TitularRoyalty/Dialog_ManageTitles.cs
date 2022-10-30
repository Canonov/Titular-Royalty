using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{

    public class Dialog_ManageTitles : Window
    {
        private Vector2 scrollPosition = new Vector2(0, 0);
        public override Vector2 InitialSize => new Vector2(400, 700);


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


        public Dialog_ManageTitles()
        {
            //optionalTitle = "TR_managetitles_title".Translate();
            doCloseX = true;
            forcePause = true;
            //doCloseButton = true;


        }

        private static void DoRow(Rect rect, RoyalTitleDef def)
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
            widgetRow.Icon(ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon"));
            widgetRow.Gap(4f);

            float width = rect.width - widgetRow.FinalX - 4f - Text.CalcSize("TR_managetitles_rename".Translate()).x - 16f - 4f - Text.CalcSize("TR_managetitles_grant".Translate()).x - 16f - 4f;
            widgetRow.Label(def.GetLabelCapForBothGenders(), width);

            if (widgetRow.ButtonText("TR_managetitles_rename".Translate()))
            {
                Find.WindowStack.Add(new Dialog_TitleRenamer(def));
            }
            if (widgetRow.ButtonText("TR_managetitles_grant".Translate()))
            {
                
            }
           
            Widgets.EndGroup();
        }
        

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            var buttonSize = CloseButSize;
            //int columnCount = 1;

            //Widgets.DrawRectFast(inRect, Color.green);

            var titleRect = new Rect(4, 17, inRect.width - 8, 40);
            //Widgets.DrawRectFast(titleRect, Color.red);
            //Widgets.DrawBox(titleRect);
            GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
            Widgets.LabelFit(titleRect, "TR_managetitles_title".Translate());
            GenUI.ResetLabelAlign();

            var contentRect = new Rect(4, 57 + 7, inRect.width - 8, (30 * Titles.Count) + 6); //old height inRect.height - (57 + 7) - 40
            //Widgets.DrawRectFast(contentRect, Color.gray);
            Widgets.DrawTitleBG(contentRect);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.ColumnWidth = contentRect.width;
            listing_Standard.Begin(contentRect);
            listing_Standard.Gap(6f);

            int i = 0;
            for (int j = 0; j < TitlesBySeniority.Count; j++)
            {
                DoRow(listing_Standard.GetRect(24f), TitlesBySeniority[j]);
                listing_Standard.Gap(6f);
                i++;
            }

            listing_Standard.End();

        }
    }
}
