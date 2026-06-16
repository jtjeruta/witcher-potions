using RimWorld;
using Verse;

namespace WitcherPotions
{
    public class IngestionOutcomeDoer_ToxicByResistance : IngestionOutcomeDoer
    {
        // Toxic buildup added to toxin-hardened (witcher) drinkers per dose, before
        // ToxicResistance scaling.
        public float toxicAmount = 0.6f;

        // A drinker whose ToxicResistance is below this is treated as an ordinary
        // human: any potion is fatal. The witcher tolerance genes start at 0.4, so
        // every witcher tier clears this bar while baseline humans (0) do not.
        public float safeToxicResistance = 0.35f;

        // Potions never push a witcher's ToxicBuildup past this. Vanilla ToxicBuildup
        // has lethalSeverity=1.0, so we must stay below 1.0. This sits inside the
        // "extreme" stage (>=0.80), which caps consciousness at 10% - i.e. the witcher
        // passes out and sleeps it off, but is never killed by the potions themselves.
        public float witcherToxicCeiling = 0.85f;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            if (pawn == null || pawn.Dead)
            {
                return;
            }

            float resistance = pawn.GetStatValue(StatDefOf.ToxicResistance);

            if (resistance >= safeToxicResistance)
            {
                // Witcher-grade tolerance: accrue toxic buildup, but cap our
                // contribution below the vanilla lethal severity (1.0). Stacking
                // enough doses drives them into the "extreme" stage (downed, ~10%
                // consciousness) so they pass out and sleep it off, but the potions
                // alone can never reach the 100% that would kill them.
                Hediff existing = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup);
                float current = existing != null ? existing.Severity : 0f;
                float added = toxicAmount * (1f - resistance) * ingestedCount;
                float target = System.Math.Min(current + added, witcherToxicCeiling);
                if (target > current)
                {
                    HealthUtility.AdjustSeverity(pawn, HediffDefOf.ToxicBuildup, target - current);
                }
                return;
            }

            // Ordinary humans cannot process witcher toxins. Any dose is a death
            // sentence: WitcherPotion_Toxin ramps to its lethal severity over ~a day.
            if (!pawn.health.hediffSet.HasHediff(WitcherPotionsDefOf.WitcherPotion_Toxin))
            {
                pawn.health.AddHediff(WitcherPotionsDefOf.WitcherPotion_Toxin);
            }
        }
    }
}
