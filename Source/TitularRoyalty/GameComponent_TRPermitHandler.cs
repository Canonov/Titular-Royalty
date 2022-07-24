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
            
            if (!pawnTitle.HasModExtension<TitlePlayerPermitsExtension>())
            {
                return;
            }

            foreach (RoyalTitlePermitDef item in pawnTitle.GetModExtension<TitlePlayerPermitsExtension>().permits)
            {
                pawn.royalty.AddPermit(item, Faction.OfPlayer);
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
            foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
            {
                UpdatePermits(pawn);
            }
        }

    }
}
