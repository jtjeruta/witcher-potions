using HarmonyLib;
using RimWorld;
using Verse;

namespace WitcherPotions.BlackBlood
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.PostApplyDamage))]
    internal static class Patch_Pawn_PostApplyDamage_BlackBlood
    {
        public static void Postfix(Pawn __instance, DamageInfo dinfo, float totalDamageDealt)
        {
            if (!BlackBloodUtility.BiotechActive || totalDamageDealt <= 0f || !BlackBloodUtility.HasBlackBlood(__instance))
            {
                return;
            }

            if (dinfo.Instigator is not Pawn attacker || attacker == __instance)
            {
                return;
            }

            if (!IsMeleeDamage(dinfo))
            {
                return;
            }

            BlackBloodBurnUtility.TryBurnBloodfeeder(attacker, __instance);
        }

        private static bool IsMeleeDamage(DamageInfo dinfo)
        {
            if (dinfo.Weapon?.IsMeleeWeapon == true)
            {
                return true;
            }

            return dinfo.Tool != null;
        }
    }
}
