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

		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
            return true;
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
