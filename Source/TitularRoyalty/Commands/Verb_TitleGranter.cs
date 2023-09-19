//using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace TitularRoyalty
{

	[UsedImplicitly] // Used by Staff
	public class Verb_TitleGranter : Verb_CastBase
	{
        public override Texture2D UIIcon => Resources.CrownIcon;

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) => true;

        public override bool TryCastShot()
		{
			if (!(currentTarget.Thing is Pawn) || currentTarget.Pawn.royalty == null) return false;
			
			//Log.Message($"Titular Royalty: Opening title GUI for: {currentTarget.Pawn.Name}");
			var window = new Dialog_ChooseTitles(currentTarget.Pawn);
			Find.WindowStack.Add(window);

			return true;
		}
    }

}
