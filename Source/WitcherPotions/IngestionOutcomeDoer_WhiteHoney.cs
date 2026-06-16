using RimWorld;
using Verse;

namespace WitcherPotions
{
    // White honey purges potion toxicity from a toxin-hardened (witcher) drinker.
    // For an ordinary human it is just as fatal as any other witcher potion.
    public class IngestionOutcomeDoer_WhiteHoney : IngestionOutcomeDoer
    {
        // A drinker whose ToxicResistance is below this is treated as an ordinary
        // human: white honey poisons them like any other potion instead of curing.
        public float safeToxicResistance = 0.35f;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            if (pawn == null || pawn.Dead)
            {
                return;
            }

            float resistance = pawn.GetStatValue(StatDefOf.ToxicResistance);

            if (resistance >= safeToxicResistance)
            {
                // Witcher: flush all toxic buildup (and any lingering potion toxin).
                RemoveHediff(pawn, HediffDefOf.ToxicBuildup);
                RemoveHediff(pawn, WitcherPotionsDefOf.WitcherPotion_Toxin);
                return;
            }

            // Ordinary human: the mutagenic toxins are fatal, white honey or not.
            if (!pawn.health.hediffSet.HasHediff(WitcherPotionsDefOf.WitcherPotion_Toxin))
            {
                pawn.health.AddHediff(WitcherPotionsDefOf.WitcherPotion_Toxin);
            }
        }

        private static void RemoveHediff(Pawn pawn, HediffDef def)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(def);
            if (hediff != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
