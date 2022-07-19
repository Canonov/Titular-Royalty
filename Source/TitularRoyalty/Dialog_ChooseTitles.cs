using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
    public class Dialog_ChooseTitles : Window
    {

        public Pawn thingToChange;
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 4;

        Dictionary<RoyalTitleDef, int> seniorityTitles = new Dictionary<RoyalTitleDef, int>();

        public Dialog_ChooseTitles(Pawn targPawn)
        {
            this.thingToChange = targPawn;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;
            foreach (RoyalTitleDef v in DefDatabase<RoyalTitleDef>.AllDefs)
            {
                //Log.Message($"Defname: {v.ToString()} Label: {v.label}");
                foreach (var li in v.tags)
                {
                    if (li.Contains("PlayerTitle"))
                    {
                        seniorityTitles.Add(v, v.seniority);
                    }
                }

            }

        }

        public override Vector2 InitialSize => new Vector2(620f, 500f);
        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 30f;
            outRect.yMax -= 40f;


            if (seniorityTitles.Count > 0)
            {

                Widgets.Label(new Rect(0, 10, 300f, 30f), "TR_choosetitle".Translate());

                var viewRect = new Rect(0f, 30f, outRect.width - 16f, (seniorityTitles.Count / 4) * 128f + 256f);

                /*Color color = thingToChange.Graphic.Color;
                if (thingToChange.Stuff != null)
                {
                    color = thingToChange.def.GetColorForStuff(thingToChange.Stuff);
                }*/

                Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);


                Rect rectIconFirst = new Rect(10, 20f, 128f, 128f);

                //GUI.DrawTexture(rectIconFirst, thingToChange.DefaultGraphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, color, 0f, 0f);
                Widgets.LabelFit(rectIconFirst, "what".Translate());
                /*if (Widgets.ButtonInvisible(rectIconFirst))
                {
                    thingToChange.StyleDef = null;
                    thingToChange.DirtyMapMesh(thingToChange.Map);
                    Close();
                }*/
                TooltipHandler.TipRegion(rectIconFirst, "TR_CurrentTitle".Translate());

                int foreachI = 0;
                foreach (var pair in seniorityTitles.OrderBy(p => p.Value))
                {
                    // work with pair.Key and pair.Value
                    RoyalTitleDef title = pair.Key;
                    if (title != null)
                    {
                        Rect rectIcon = new Rect((128 * (foreachI % columnCount)) + 10, (128 * (foreachI / columnCount)) + 128f, 128f, 128f);
                        //GUI.DrawTexture(rectIcon, style.Graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, color, 0f, 0f);
                        Widgets.LabelFit(rectIcon, title.LabelCap);
                        if (Widgets.ButtonInvisible(rectIcon))
                        {
                            thingToChange.royalty.TryUpdateTitle(Faction.OfPlayer, true, title);
                            //thingToChange.DirtyMapMesh(thingToChange.Map);
                            Close();
                        }
                        TooltipHandler.TipRegion(rectIcon, title.LabelCap);
                    }
                    foreachI++;
                }

                Widgets.EndScrollView();

            }
            else
            {
                Widgets.Label(new Rect(0, 10, 300f, 30f), "TR_NoTitles".Translate());
            }


        }
    }
}