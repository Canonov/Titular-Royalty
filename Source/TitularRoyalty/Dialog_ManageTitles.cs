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
        public override Vector2 InitialSize => new Vector2(600, 500);

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

            Rect rect = new Rect(inRect).ContractedBy(18f);

            GUI.BeginGroup(rect);
            
            //Window Title
            var titleRect = new Rect(0f, 0f, inRect.width, 30f);
            GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
            Widgets.Label(titleRect, "TR_managetitles_title".Translate());
            GenUI.ResetLabelAlign();

            GUI.EndGroup();
        }
    }
}
