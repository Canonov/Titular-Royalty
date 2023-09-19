using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace TitularRoyalty
{
    /// <summary>
    /// Not really suppose to have any fields, just a way to link the Comp to the properties and make sure it's only on pawns
    /// </summary>
    [UsedImplicitly]
    public class CompProperties_PlayerRoyaltyTracker : CompProperties
    {
        public CompProperties_PlayerRoyaltyTracker()
        {
            compClass = typeof(Comp_PlayerRoyaltyTracker);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            // Must be Pawn or Subclass of (just incase)
            var thingClass = parentDef.thingClass;
            if (thingClass != typeof(Pawn) && !thingClass.IsSubclassOf(typeof(Pawn)))
            {
                yield return "Comp_PlayerRoyaltyTracker can only be applied to Pawns";
            }
        }
    }
}