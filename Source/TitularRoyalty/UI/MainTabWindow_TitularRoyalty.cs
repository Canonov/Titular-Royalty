using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace TitularRoyalty.UI
{
    
    public class MainTabWindow_TitularRoyalty : MainTabWindow
    {
        private Vector2 scrollPosition_titleList;
        private float scrollViewHeight_titleList;
        
        public enum RoyaltyTab : byte
        {
            Titles,
            Hierarchy,
            Laws
        }

        private List<TabRecord> tabs = new List<TabRecord>();
        private static RoyaltyTab curTab = RoyaltyTab.Titles;

        public override Vector2 RequestedTabSize => new Vector2(1010f, 640f);
        
        public override void PreOpen()
        {
            base.PreOpen();
            tabs.Clear();
            
            tabs.Add(new TabRecord("TR.MainWindow.Tab_Titles".Translate(), delegate
            {
                curTab = RoyaltyTab.Titles;
            }, () => curTab is RoyaltyTab.Titles));
            tabs.Add(new TabRecord("TR.MainWindow.Tab_Hierarchy".Translate(), delegate
            {
                curTab = RoyaltyTab.Hierarchy;
            }, () => curTab is RoyaltyTab.Hierarchy));
            tabs.Add(new TabRecord("TR.MainWindow.Tab_Laws".Translate(), delegate
            {
                curTab = RoyaltyTab.Laws;
            }, () => curTab is RoyaltyTab.Laws));
            
            scrollPosition_titleList = Vector2.zero;
            scrollViewHeight_titleList = 0f;
        }

        public override void PostClose()
        {
            base.PostClose();
        }

        public override void DoWindowContents(Rect inRect)
        {
            var baseRect = inRect;
            baseRect.yMin += 45f;
            TabDrawer.DrawTabs(baseRect, tabs);

            switch (curTab)
            {
                case RoyaltyTab.Titles:
                    MainWindowUIUtility.DrawTitlesTab(baseRect, ref scrollPosition_titleList);
                    break;
                case RoyaltyTab.Hierarchy:
                    MainWindowUIUtility.DrawHierarchyTab(baseRect);
                    break;
                case RoyaltyTab.Laws:
                    MainWindowUIUtility.DrawLawsTab(baseRect);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}