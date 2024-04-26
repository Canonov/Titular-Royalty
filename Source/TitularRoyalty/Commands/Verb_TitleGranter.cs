//using System;

using Verse.AI;
using UnityEngine;

namespace TitularRoyalty;

[UsedImplicitly] // Used by Staff
public class Verb_TitleGranter : Verb_CastBase
{
	public override Texture2D UIIcon => BaseContent.BadTex;

	public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) => true;

	protected override bool TryCastShot()
	{
		if (currentTarget.Thing is not Pawn || currentTarget.Pawn.royalty == null) 
			return false;

		Log.Warning("Not Yet Readded"); // todo 
		//var window = new Dialog_ChooseTitles(currentTarget.Pawn);
		//Find.WindowStack.Add(window);

		return true;
	}
}