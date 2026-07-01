using RimWorld;
using Verse;

namespace WitcherPotions.BlackBlood
{
    public static class BlackBloodBurnUtility
    {
        private const float AttachFireChance = 0.35f;

        public static void TryBurnBloodfeeder(Pawn attacker, Pawn victim)
        {
            if (!BlackBloodUtility.BiotechActive || attacker == null || attacker.Dead)
            {
                return;
            }

            if (!GeneUtility.IsBloodfeeder(attacker))
            {
                return;
            }

            FireUtility.TryAttachFire(attacker, AttachFireChance, victim);
        }
    }
}
