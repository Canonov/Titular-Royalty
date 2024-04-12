using UnityEngine;

namespace TitularRoyalty
{
    public class PlayerTitlePermitWorker_BestowPsycast : RoyalTitlePermitWorker_Targeted
    {
        //private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer");

        public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
        {
            Action action = null;
            string description = def.LabelCap + ": ";
            if (FillAidOption(pawn, faction, ref description, out var free))
            {
                action = delegate
                {
                    BeginCallAid(pawn, map, faction, free);
                };
            }
            yield return new FloatMenuOption(description, action, faction.def.FactionIcon, faction.Color);
        }

        private void BeginCallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
        {
            targetingParameters = TargetingParameters.ForColonist();
            targetingParameters.validator = delegate (TargetInfo target)
            {
                if (target.Thing is Pawn pawn)
                {
                    if (ModLister.HasActiveModWithName("Vanilla Psycasts Expanded"))
                    {
                        return true;
                    }
                    return pawn.GetPsylinkLevel() < HediffDefOf.PsychicAmplifier.maxSeverity;
                }
                return false;
            };

            base.caller = caller;
            base.map = map;
            base.free = free;

            Find.Targeter.BeginTargeting(this);
        }

        public override void OrderForceTarget(LocalTargetInfo target)
        {
            base.OrderForceTarget(target);
            target.Pawn.ChangePsylinkLevel(1, true);
            caller.royalty.GetPermit(def, Faction.OfPlayer).Notify_Used();
        }

        public override void DrawHighlight(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(caller.Position, def.royalAid.targetingRange, Color.white);
            //GenDraw
            if (target.IsValid)
            {
                GenDraw.DrawTargetHighlight(target);
            }
        }

        public override void OnGUI(LocalTargetInfo target)
        {
            Texture2D icon = ((!target.IsValid) ? TexCommand.CannotShoot : ((!(UIIcon != null) || !(UIIcon != BaseContent.BadTex)) ? TexCommand.Attack : UIIcon));
            GenUI.DrawMouseAttachment(icon);
        }

    }
}
