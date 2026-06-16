using System;
using RimWorld;
using Verse;

namespace WitcherPotions
{
    public class IngestionOutcomeDoer_ToxicByResistance : IngestionOutcomeDoer
    {
        public float toxicAmount = 1.0f;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            if (pawn == null || pawn.Dead || toxicAmount <= 0f)
            {
                return;
            }

            float resistance = pawn.GetStatValue(StatDefOf.ToxicResistance);
            float effectiveToxicity = toxicAmount * (1f - resistance) * ingestedCount;
            if (effectiveToxicity <= 0f)
            {
                return;
            }

            HealthUtility.AdjustSeverity(pawn, HediffDefOf.ToxicBuildup, effectiveToxicity);
        }
    }
}
