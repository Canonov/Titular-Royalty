using System;
using RimWorld;
using TitularRoyalty.Extensions;
using UnityEngine;
using Verse;

namespace TitularRoyalty.UI
{
    [StaticConstructorOnStartup]
    public static class MainWindowUIUtility
    {
        private static GameComponent_PlayerTitlesManager TitleManager => GameComponent_PlayerTitlesManager.Current;

        #region Titles Tab

        public static void DrawTitlesTab(Rect inRect, ref Vector2 scrollPosition_titleList)
        {
            inRect.yMin += 17f;
            
            var titleListRect = new Rect(inRect.x, inRect.y, Mathf.FloorToInt(inRect.width * 0.30f),
                inRect.height);
            
            DrawTitlesList(titleListRect, out var selTitle, ref scrollPosition_titleList);

            var titleInfoRect = new Rect((titleListRect.xMax + 12f), inRect.y, (inRect.width - titleListRect.width - 12f),
                inRect.height);
            
            DrawTitleInfo(titleInfoRect, selTitle);
        }

        private static void DrawTitleInfo(Rect titleInfoRect, PlayerTitleData selTitle)
        {
            Widgets.DrawBox(titleInfoRect);
            Widgets.Label(titleInfoRect, "Currently Selected: " + selTitle);
        }

        private static void DrawTitlesList(Rect titleListRect, out PlayerTitleData selTitle, ref Vector2 scrollPos)
        {
            var titleCount = TitleManager.Titles.Count;
            selTitle = null;

            const float rowSize = 64f;
            const float gap = 8f;

            // Scrollview
            var outRect = titleListRect;
            var viewRect = new Rect(outRect.x, outRect.y, outRect.width - 15, (rowSize * titleCount) + (titleCount + 1) * gap);
            
            Widgets.BeginScrollView(outRect, ref scrollPos, viewRect);
            
            // List the Titles
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(viewRect);
            listingStandard.Gap(gap);
            
            foreach (var title in TitleManager.Titles)
            {
                var row = listingStandard.GetRect(rowSize, 0.90f); // Get the row
                row.x += (viewRect.width * 0.05f); // Half of the remainder of the width
                
                DrawTitleRow(row, title, out bool selected); // Draw the row
                
                if (selected) // If the title was selected, set the selected title
                    selTitle = title;
                    
                listingStandard.Gap(gap);
            }
            
            listingStandard.End();
            
            Widgets.EndScrollView();
            
            // End
            Widgets.DrawBox(titleListRect);
        }

        private static void DrawTitleRow(Rect rect, PlayerTitleData titleData, out bool selected)
        {
            selected = false;
            
            Widgets.DrawBox(rect);
            Widgets.LabelFit(rect, titleData.label);
        }

        #endregion

        public static void DrawHierarchyTab(Rect inRect)
        {
        }

        public static void DrawLawsTab(Rect inRect)
        {
        }
    }
}