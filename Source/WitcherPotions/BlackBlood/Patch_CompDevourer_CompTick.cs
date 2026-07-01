using HarmonyLib;
using RimWorld;
using Verse;

namespace WitcherPotions.BlackBlood
{
    [HarmonyPatch(typeof(CompDevourer), nameof(CompDevourer.CompTick))]
    internal static class Patch_CompDevourer_CompTick
    {
        public static void Postfix(CompDevourer __instance)
        {
            if (__instance.DigestingPawn == null)
            {
                BlackBloodDevourerUtility.ClearTracking(__instance);
                return;
            }

            BlackBloodDevourerUtility.TriggerRegurgitation(__instance);
        }
    }
}
