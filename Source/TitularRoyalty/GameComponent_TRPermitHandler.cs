using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TitularRoyalty
{
    public class GameComponent_TRPermitHandler : GameComponent
    {
        public GameComponent_TRPermitHandler(Game game)
        {

        }

        public void UpdatePermits(Pawn pawn)
        {
            var pawnTitle = pawn.royalty.GetCurrentTitle(Faction.OfPlayer);

            // Add new permits and Remove permits you no longer have permission to

            var pawnTitlePermits = pawnTitle.GetModExtension<TitlePlayerPermitsExtension>().permits;

            //pawn.royalty.factionPermits.Clear();
            pawn.royalty.RefundPermits(0, Faction.OfPlayer);

            foreach (RoyalTitlePermitDef item in pawnTitlePermits)
            {
                //pawn.royalty.AddPermit(item, Faction.OfPlayer);
            }
        }

        public override void LoadedGame()
        {
            foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
            {
                if (pawn.royalty != null && !pawn.Dead && pawn.royalty.HasAnyTitleIn(Faction.OfPlayer))
                {
                    UpdatePermits(pawn);
                }
            }
        }

        public override void StartedNewGame()
        {

        }

    }
}
