//using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace TitularRoyalty
{
    public class Comp_ManageTitles : ThingComp
    {
        public CompProperties_ManageTitles Props => (CompProperties_ManageTitles)props;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
	        foreach (var gizmo in base.CompGetGizmosExtra())
	        {
		        yield return gizmo;
	        }

	        var manageTitlesGizmo = new Command_Action
	        {
		        icon = ContentFinder<Texture2D>.Get(Props.managetitlesIconPath),
		        defaultLabel = "TR_Command_managetitles_label".Translate(),
		        defaultDesc = "TR_Command_managetitles_desc".Translate(),
		        action = delegate
		        {
			        //var window = new Dialog_ManageTitles();
			        //Find.WindowStack.Add(window);
		        }
	        };

	        yield return manageTitlesGizmo;
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            //RoyalTitleDef selPawnTitle;

            //Manage Titles Dialog
            yield return new FloatMenuOption("TR_Command_managetitles_label".Translate(), delegate
            {
                //var window = new Dialog_ManageTitles();
                //Find.WindowStack.Add(window);
            }, itemIcon: Resources.CrownIcon, iconColor: Color.white);
            
        }
    }
}
