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

        public Pawn chosenPawn;
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 1;
        Dictionary<RoyalTitleDef, int> seniorityTitles = new Dictionary<RoyalTitleDef, int>();

        public Dialog_ChooseTitles(Pawn targPawn)
        {
            this.chosenPawn = targPawn;
            doCloseX = true;
            doCloseButton = true;
            closeOnClickedOutside = true;
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
                    seniorityTitles.Add(v, v.seniority);
                }
            }
            
            if(seniorityTitles.Count == 0)
            {
                Log.Error("Titular Royalty: Couldn't fill dialog titles");
            }
            else
            {
                Log.Message("Titular Royalty: Dialog titles filled");
            }

        }
        private string GetDisplayTitle(RoyalTitleDef title, Gender gender)
        {
            // Prince-Consort doesn't fit in the GUI and Queen would show up twice
            if (title.defName == "TitularRoyalty_T_RY_Consort")
            {
                return "Consort";
            }
            // Only women use different titles
            if (gender == Gender.Female && title.labelFemale != null)
            {
                return title.labelFemale;
            }
            return title.label;
        }
        public override Vector2 InitialSize => new Vector2(700f / 4f, 500f);
        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 30f;
            outRect.yMax -= 40f;

            if (seniorityTitles.Count > 0)
            {

                Widgets.Label(new Rect(15, 10, 300f, 30f), "TR_choosetitle".Translate());
                var viewRect = new Rect(0f, 30f, outRect.width - 16f, (seniorityTitles.Count / 4) * 128f + 256f);
                Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);


                Rect rectIconFirst = new Rect(10, 20f, 80f, 24f);
                TooltipHandler.TipRegion(rectIconFirst, "TR_CurrentTitle".Translate());

                int foreachI = 0;



                foreach (var pair in seniorityTitles.OrderBy(p => p.Value))
                {
                    // work with pair.Key and pair.Value
                    RoyalTitleDef title = pair.Key;
                    if (title != null)
                    {
                        Rect rectIcon = new Rect(
                            (32 * (foreachI % columnCount)) + 10,
                            (32 * (foreachI / columnCount)) + 32f,
                            125f, 32f);
                        //GUI.DrawTexture(rectIcon, style.Graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, color, 0f, 0f);
                        //Widgets.Label(rectIcon, title.LabelCap);
                        if (Widgets.ButtonText(rectIcon, GetDisplayTitle(title, chosenPawn.gender), drawBackground: true))
                        {
                            //Log.Message($"Fired {title.label} for pawn {chosenPawn.Name}");
                            if (chosenPawn != null && chosenPawn.royalty != null && chosenPawn.royalty.GetCurrentTitle(Faction.OfPlayer) != title)
                            {
                                chosenPawn.royalty.SetTitle(Faction.OfPlayer, title, grantRewards: true, sendLetter: true);
                            }
                            Close();
                        }
                        /*if (Widgets.ButtonInvisible(rectIcon))
                        {
                            Log.Message($"Fired {title.label} for pawn {chosenPawn.Name}");
                            if (chosenPawn != null && chosenPawn.royalty != null)
                            {
                                chosenPawn.royalty.SetTitle(Faction.OfPlayer, title, grantRewards: true, sendLetter: true);
                            }
                            Close();
                        }*/
                        TooltipHandler.TipRegion(rectIcon, GetDisplayTitle(title, chosenPawn.gender));
                    }
                    foreachI++;
                }


                Widgets.EndScrollView();
            }
            else
            {
                Widgets.Label(new Rect(0, 10, 300f, 30f), "TR_NoTitles".Translate());
                Log.Error($"Titular Royalty: Couldn't fill title dialog, relevant variable for author: {seniorityTitles.Count}");
            }


        }
    }
}