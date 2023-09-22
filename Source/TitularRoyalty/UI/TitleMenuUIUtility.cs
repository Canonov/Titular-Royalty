using System;
using RimWorld;
using TitularRoyalty.Extensions;
using UnityEngine;
using Verse;

namespace TitularRoyalty.UI
{
    [StaticConstructorOnStartup]
    public static class TitleMenuUIUtility
    {
        public static void DoTitleWindow(Rect fillRect)
        {
            Widgets.DrawBox(fillRect);
            PlayerTitleData mouseOverTitle = null;

            var titleListRect = new Rect(fillRect.x, fillRect.y, Mathf.FloorToInt(fillRect.width * 0.25f),
                fillRect.height);
            
            //todo draw title list
            Widgets.DrawBox(titleListRect);

            var titleDetailsRect = new Rect(titleListRect.xMax, fillRect.y, fillRect.width - titleListRect.width,
                fillRect.height);
            
            Widgets.DrawBox(titleDetailsRect);

            var titleDetailsContentRect = titleDetailsRect.ContractedBy(17f);
            titleDetailsContentRect.yMax += 8f;
        }
    }
}