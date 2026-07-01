using System;
using HarmonyLib;
using Verse;

namespace WitcherPotions.BlackBlood
{
    [StaticConstructorOnStartup]
    internal static class BlackBloodPatches
    {
        private const string HarmonyId = "witcher.potions.blackblood";

        static BlackBloodPatches()
        {
            Harmony harmony = new Harmony(HarmonyId);

            if (BlackBloodUtility.AnomalyActive)
            {
                TryPatch(() => new PatchClassProcessor(harmony, typeof(Patch_CompDevourer_CompTick)).Patch());
            }

            if (BlackBloodUtility.BiotechActive)
            {
                TryPatch(() => new PatchClassProcessor(harmony, typeof(Patch_CompAbilityEffect_BloodfeederBite_Apply)).Patch());
                TryPatch(() => new PatchClassProcessor(harmony, typeof(Patch_Pawn_PostApplyDamage_BlackBlood)).Patch());
            }
        }

        private static void TryPatch(Action patchAction)
        {
            try
            {
                patchAction();
            }
            catch (Exception ex)
            {
                Log.Warning("[WitcherPotions] Black blood patch failed: " + ex);
            }
        }
    }
}
