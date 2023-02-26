//using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using UnityEngine;

namespace TitularRoyalty
{

	public class Verb_TitleGranter : Verb_CastBase
	{
        public override Texture2D UIIcon => Resources.CrownIcon;

        public override string ReportLabel => "Verb_TitleGranterReportLabel".Translate();

		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!currentTarget.HasThing || currentTarget.Thing.Map != caster.Map)
			{
				return false;
			}
            if (currentTarget.Pawn == null || !currentTarget.Pawn.IsColonist)
            {
                return false;
            }

            return base.ValidateTarget(target, showMessages);
        }

        protected override bool TryCastShot()
		{
            if (currentTarget.Thing is Pawn && currentTarget.Pawn.royalty != null)
            {
                //Log.Message($"Titular Royalty: Opening title GUI for: {currentTarget.Pawn.Name}");
                Dialog_ChooseTitles window = new Dialog_ChooseTitles(currentTarget.Pawn);
                Find.WindowStack.Add(window);

                return true;
            }

            return false;
		}
    }

}
