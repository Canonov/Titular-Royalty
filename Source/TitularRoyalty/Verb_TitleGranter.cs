//using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;


namespace TitularRoyalty
{

	public class Verb_TitleGranter : Verb_CastBase
	{
		private void TitleGranter(Pawn pawn, Faction fact)
        {
			//var seniorityTitles = new Dictionary<int, RoyalTitleDef>();

			foreach (RoyalTitleDef v in DefDatabase<RoyalTitleDef>.AllDefs)
            {
				Log.Message($"Defname: {v.ToString()} Label: {v.label} \nTags: {v.tags}\n");
				
            }
        }

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

				TitleGranter(currentTarget.Pawn, Faction.OfPlayer);

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
