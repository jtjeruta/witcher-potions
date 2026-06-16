using RimWorld;
using Verse;

namespace WitcherPotions
{
    [DefOf]
    public static class WitcherPotionsDefOf
    {
        public static HediffDef WitcherPotion_SwallowHediff;
        public static HediffDef WitcherPotion_ThunderboltHediff;
        public static HediffDef WitcherPotion_Toxin;
        public static ThingDef WitcherPotion_Swallow;
        public static ThingDef WitcherPotion_Thunderbolt;
        public static ResearchProjectDef Witcher_Alchemy;

        static WitcherPotionsDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(WitcherPotionsDefOf));
        }
    }
}
