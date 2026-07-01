using RimWorld;
using Verse;

namespace WitcherPotions
{
    public static class BlackBloodUtility
    {
        public static bool AnomalyActive =>
            ModsConfig.IsActive("Ludeon.RimWorld.Anomaly");

        public static bool BiotechActive =>
            ModsConfig.IsActive("Ludeon.RimWorld.Biotech");

        public static bool HasBlackBlood(Pawn pawn)
        {
            if (pawn?.health?.hediffSet == null)
            {
                return false;
            }

            HediffDef hediff = WitcherPotionsDefOf.WitcherPotion_BlackBloodHediff;
            return hediff != null && pawn.health.hediffSet.HasHediff(hediff);
        }
    }
}
