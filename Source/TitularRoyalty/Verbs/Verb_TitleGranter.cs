//using System;
using RimWorld;
using Verse;
using Verse.AI;


namespace TitularRoyalty
{

	public class Verb_TitleGranter : Verb_CastBase
	{
		//public const float Points = 2500f;

		protected override bool TryCastShot()
		{
			Log.Message($"Trying to Print Info");
			Log.Message($"{currentTarget.GetType()}");
			Log.Message($"{currentTarget.Thing is Pawn} | {currentTarget.HasThing} | {currentTarget.Cell}");
			Log.Message($"end");
			if (Faction.OfMechanoids == null)
			{
				Messages.Message("MessageNoFactionForVerbMechCluster".Translate(), caster, MessageTypeDefOf.RejectInput, null, historical: false);
				return false;
			}
			if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map)
			{
				return false;
			}
			if (currentTarget.HasThing && currentTarget.Thing is Pawn)
            {
				Log.Message("Target is pawn, wow");
				Log.Message($"Target: {currentTarget.Pawn.Name}");
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
