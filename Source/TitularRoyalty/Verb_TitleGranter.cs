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

			Log.Message(loadID);
			Log.Message($"{CasterPawn.Name} tried to cast");
			try
            {
				Log.Message($"Trying to Print Info");
				Log.Message($"CurrentTarget Type: {currentTarget.GetType()}");
				Log.Message($"IsPawn: {currentTarget.Thing is Pawn} | HasThing: {currentTarget.HasThing} | Cell: {currentTarget.Cell}");
				Log.Message($"{currentTarget}");
				Log.Message($"end");
			}
            catch
            {
				Log.Message("Failed to Load");
            }
			if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map)
			{
				return false;
			}
			if (currentTarget.HasThing && currentTarget.Thing is Pawn)
            {
				Log.Message("Target is pawn, wow");
				Log.Message($"Target: {currentTarget.Pawn.Name}");

				Dialog_ChooseTitles window = new Dialog_ChooseTitles(currentTarget.Pawn);
				Find.WindowStack.Add(window);

				//currentTarget.Pawn.royalty.TryUpdateTitle(Faction.OfPlayer, sendLetter: true, RoyalTitleDef);
			}
			//MechClusterUtility.SpawnCluster(currentTarget.Cell, caster.Map, MechClusterGenerator.GenerateClusterSketch(2500f, caster.Map, startDormant: true, forceNoConditionCauser: true));
			return true;
		}

		/*public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}*/
	}

}
