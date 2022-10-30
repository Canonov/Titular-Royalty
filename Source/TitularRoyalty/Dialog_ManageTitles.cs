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

        public Dictionary<RoyalTitleDef, int> TitlesBySeniority
        {
            get
            {
                Dictionary<RoyalTitleDef, int> titledict = new Dictionary<RoyalTitleDef,int>();

                for (int i = 0; i < Titles.Count; i++)
                {
                    titledict.Add(Titles[i], Titles[i].seniority);
                }

                return titledict;
            }
        } 


        public Dialog_ManageTitles()
        {
            //optionalTitle = "TR_managetitles_title".Translate();
            doCloseX = true;
            forcePause = true;
            //doCloseButton = true;


        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            var buttonSize = CloseButSize;
            int columnCount = 1;

            Rect rect = new Rect(inRect).ContractedBy(18f);
            

            //Window Title
            var titleRect = new Rect(0f, 0f, inRect.width, 30f);
            GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
            Widgets.Label(titleRect, "TR_managetitles_title".Translate());
            GenUI.ResetLabelAlign();

            var viewRect = new Rect(0f, 46f, rect.width - 16f, (TitlesBySeniority.Count / 4) * 128f);
            Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);
            


            TooltipHandler.TipRegion(viewRect, $"{nameof(viewRect)}:\n" + $"Bounds: {{x:{viewRect.x} y:{viewRect.y} w:{viewRect.width} h:{viewRect.height}}}");
            TooltipHandler.TipRegion(titleRect, $"{nameof(titleRect)}:\n" + $"Bounds: {{x:{titleRect.x} y:{titleRect.y} w:{titleRect.width} h:{titleRect.height}}}");
            TooltipHandler.TipRegion(inRect, $"{nameof(inRect)}:\n" + $"Bounds: {{x:{inRect.x} y:{inRect.y} w:{inRect.width} h:{inRect.height}}}");
            TooltipHandler.TipRegion(rect, $"{nameof(rect)}:\n" + $"Bounds: {{x:{rect.x} y:{rect.y} w:{rect.width} h:{rect.height}}}");



            //List
            int foreachI = 0;
            foreach (var pair in TitlesBySeniority.OrderBy(p => p.Value))
            {
                RoyalTitleDef title = pair.Key;
                if (title != null)
                {
                    Rect rectIcon = new Rect(
                        (32 * (foreachI % columnCount)) + 32f,
                        (32 * (foreachI / columnCount)) + 32f,
                        125f, 32f);

                    if (Widgets.ButtonText(rectIcon, title.label, drawBackground: true))
                    {

                    }
                }

                foreachI++;
            }

     
            Widgets.EndScrollView();

        }
    }
}
