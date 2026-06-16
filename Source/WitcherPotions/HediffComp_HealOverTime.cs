using System.Linq;
using Verse;

namespace WitcherPotions
{
    public class HediffComp_HealOverTime : HediffComp
    {
        private int ticksUntilHeal;

        public HediffCompProperties_HealOverTime Props => (HediffCompProperties_HealOverTime)props;

        public override void CompPostMake()
        {
            base.CompPostMake();
            ticksUntilHeal = Props.ticksInterval;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            Pawn pawn = parent.pawn;
            if (pawn == null || pawn.Dead || !pawn.Spawned)
            {
                return;
            }

            ticksUntilHeal--;
            if (ticksUntilHeal > 0)
            {
                return;
            }

            ticksUntilHeal = Props.ticksInterval;
            TryHeal(pawn);
        }

        private void TryHeal(Pawn pawn)
        {
            var healableInjuries = pawn.health.hediffSet.hediffs
                .OfType<Hediff_Injury>()
                .Where(injury => injury.CanHealNaturally() && injury.Severity > 0.01f)
                .OrderByDescending(injury => injury.Bleeding)
                .ToList();

            if (healableInjuries.Count == 0)
            {
                return;
            }

            Hediff_Injury target = healableInjuries.First();
            target.Severity = System.Math.Max(0f, target.Severity - Props.healAmount);
            if (target.Severity <= 0.01f)
            {
                pawn.health.RemoveHediff(target);
            }
        }
    }
}
