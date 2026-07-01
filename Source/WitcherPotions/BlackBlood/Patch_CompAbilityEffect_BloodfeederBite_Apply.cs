using HarmonyLib;
using RimWorld;
using Verse;

namespace WitcherPotions.BlackBlood
{
    [HarmonyPatch(typeof(CompAbilityEffect_BloodfeederBite), nameof(CompAbilityEffect_BloodfeederBite.Apply))]
    internal static class Patch_CompAbilityEffect_BloodfeederBite_Apply
    {
        public static void Postfix(CompAbilityEffect_BloodfeederBite __instance, LocalTargetInfo target)
        {
            if (!BlackBloodUtility.BiotechActive || !target.HasThing)
            {
                return;
            }

            if (target.Thing is not Pawn victim || !BlackBloodUtility.HasBlackBlood(victim))
            {
                return;
            }

            if (__instance.parent?.pawn is Pawn caster)
            {
                BlackBloodBurnUtility.TryBurnBloodfeeder(caster, victim);
            }
        }
    }
}
