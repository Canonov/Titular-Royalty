//using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace TitularRoyalty
{
    public class Comp_ManageTitles : ThingComp
    {
        public CompProperties_ManageTitles Props => (CompProperties_ManageTitles)props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }

            if (!Props.dontManageTitles)
            {
                Command_Action manage_titles = new Command_Action();
                manage_titles.icon = ContentFinder<Texture2D>.Get(Props.managetitlesIconPath);
                manage_titles.defaultLabel = "TR_Command_managetitles_label".Translate();
                manage_titles.defaultDesc = "TR_Command_managetitles_desc".Translate();

                manage_titles.action = delegate
                {
                    Dialog_ManageTitles window = new Dialog_ManageTitles();
                    Find.WindowStack.Add(window);
                };

                yield return manage_titles;
            }

        }
    }
}
