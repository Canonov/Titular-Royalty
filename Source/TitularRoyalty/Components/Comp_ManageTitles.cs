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
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }

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

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            RoyalTitleDef selPawnTitle;

            //Manage Titles Dialog
            yield return new FloatMenuOption("TR_Command_managetitles_label".Translate(), delegate
            {
                Dialog_ManageTitles window = new Dialog_ManageTitles();
                Find.WindowStack.Add(window);
            }, itemIcon: Resources.CrownIcon, iconColor: Color.white);

            //Set Heir Option
            if (TitularRoyaltyMod.Settings.inheritanceEnabled && (selPawnTitle = selPawn.royalty?.GetCurrentTitleInFaction(Faction.OfPlayer)?.def) != null && selPawnTitle.canBeInherited )
            {
                //Define action to run when selected
                Action<LocalTargetInfo> action = delegate (LocalTargetInfo targetinfo)
                {
                    RoyalTitleDef targetPawnTitle = targetinfo.Pawn.royalty.GetCurrentTitle(Faction.OfPlayer);

                    if (targetPawnTitle == null || targetPawnTitle.seniority < selPawnTitle.seniority)
                    {
                        selPawn.royalty.SetHeir(targetinfo.Pawn, Faction.OfPlayer);
                        Messages.Message("TR_setheir_success".Translate(selPawn.Name.ToStringShort, targetinfo.Pawn.Name.ToStringShort), MessageTypeDefOf.NeutralEvent);
                    }
                    else
                    {
                        Messages.Message("TR_setheir_failed_sameorhighertitle".Translate(targetinfo.Pawn.Name.ToStringShort), MessageTypeDefOf.RejectInput);
                    }
                };

                //Target pawn for selection
                FloatMenuOption SetHeir = new FloatMenuOption("TR_Command_setheir_label".Translate(selPawn.Name.ToStringShort), delegate
                {
                    Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), action);
                });

                //Return the float option
                yield return SetHeir;
            }
        }
    }
}
