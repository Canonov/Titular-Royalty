/*using RimWorld;
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
            //pawn.royalty.RefundPermits(1453, Faction.OfPlayer);

            /*var pawnTitle = pawn.royalty.GetCurrentTitle(Faction.OfPlayer);
            var pawnTitlePermits = pawnTitle.GetModExtension<TitlePlayerPermitsExtension>().permits;

            //pawn.royalty.factionPermits.Clear();

            foreach (RoyalTitlePermitDef item in pawnTitlePermits)
            {
                //pawn.royalty.AddPermit(item, Faction.OfPlayer);
            }
        }

        public override void LoadedGame()
        {
            /*foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
            {
                if (pawn.royalty != null && !pawn.Dead && pawn.royalty.HasAnyTitleIn(Faction.OfPlayer))
                {
                    UpdatePermits(pawn);
                }
            }

            RoyalTitlePermitDef dummyPermit = DefDatabase<RoyalTitlePermitDef>.GetNamed("TR_dummypermit");

            foreach (Faction item in Find.FactionManager.AllFactionsListForReading)
            {
                if (!item.IsPlayer && !item.def.permanentEnemy && !item.temporary && item.def.defName != "Empire" )
                {
                    dummyPermit.faction = item.def;
                    Log.Message(item.def.defName);
                    break;
                }
            }

        }

        public override void StartedNewGame()
        {

        }

    }
}
*/