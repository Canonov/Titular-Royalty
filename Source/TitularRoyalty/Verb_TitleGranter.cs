//using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;


namespace TitularRoyalty
{

	public class Verb_TitleGranter : Verb_CastBase
	{
		protected override bool TryCastShot()
		{
			if (currentTarget.HasThing)
            {
				if (currentTarget.Thing.Map != caster.Map)
				{
					return false;
				}
				if (currentTarget.Thing is Pawn && currentTarget.Pawn.IsColonist && currentTarget.Pawn.royalty != null)
				{
					Log.Message($"Opening title GUI for: {currentTarget.Pawn.Name}");
					Dialog_ChooseTitles window = new Dialog_ChooseTitles(currentTarget.Pawn);
					Find.WindowStack.Add(window);

					return true;
				}
			}

			return false;
		}

		/*public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}*/
	}

}
