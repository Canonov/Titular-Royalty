using System;
using System.Collections.Generic;
using RimWorld;
using TitularRoyalty.Titles;
using UnityEngine;
using Verse;

namespace TitularRoyalty.UI
{
    [StaticConstructorOnStartup]
    public static class TitleUIUtility
    {
        
        public static Action<Rect> CreateTitlePlateDrawer(Pawn pawn, PlayerTitle title) => delegate(Rect holderRect)
        {
            string titleLabel = title.GetLabelForHolder();

            var originalColor = GUI.color;
            var bgRect = new Rect(holderRect.x, holderRect.y, holderRect.width, holderRect.height);

            GUI.color = CharacterCardUtility.StackElementBackground;
            GUI.DrawTexture(bgRect, BaseContent.WhiteTex);
            GUI.color = originalColor;
            
            if (Mouse.IsOver(bgRect))
            {
                Widgets.DrawHighlight(bgRect);
            }
            
            var iconRect = new Rect(holderRect.x + 1f, holderRect.y + 1f, 20f, 20f);
            GUI.color = new Color(128, 86, 145);
            GUI.DrawTexture(iconRect, Faction.OfPlayer.def.FactionIcon);
            GUI.color = originalColor;
            
            var labelRect = new Rect(holderRect.x, holderRect.y, holderRect.width + 22f + 9f, holderRect.height);
            Widgets.Label(new Rect(labelRect.x + 22f + 5f, labelRect.y, labelRect.width - 10f, labelRect.height), titleLabel);
            
            // Clicking as a button
            if (Widgets.ButtonInvisible(bgRect))
            {
                Log.Error("Button Clicked");
                //Find.WindowStack.Add(new Dialog_InfoCard(localTitle.def, localTitle.faction, pawn));
            }
            if (Mouse.IsOver(bgRect))
            {
                var tip = new TipSignal("Tooltip for Title");
                TooltipHandler.TipRegion(bgRect, tip);
            }
            
        };
    }
}