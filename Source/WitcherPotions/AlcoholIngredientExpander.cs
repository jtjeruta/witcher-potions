using System.Collections.Generic;
using RimWorld;
using Verse;

namespace WitcherPotions
{
    // Recipe XML marks alcohol solvents with <customSummary>alcohol</customSummary> and Beer as seed.
    // At startup, expand those filters to every ingestible whose drug comp uses ChemicalDef Alcohol.
    [StaticConstructorOnStartup]
    public static class AlcoholIngredientExpander
    {
        private const string AlcoholSummary = "alcohol";

        static AlcoholIngredientExpander()
        {
            ChemicalDef alcohol = DefDatabase<ChemicalDef>.GetNamedSilentFail("Alcohol");
            if (alcohol == null)
            {
                return;
            }

            List<ThingDef> alcoholDrinks = FindAlcoholDrinks(alcohol);
            if (alcoholDrinks.Count == 0)
            {
                return;
            }

            foreach (RecipeDef recipe in DefDatabase<RecipeDef>.AllDefsListForReading)
            {
                bool hasAlcoholIngredient = false;
                if (recipe.ingredients != null)
                {
                    foreach (IngredientCount ing in recipe.ingredients)
                    {
                        if (IsAlcoholSolventFilter(ing.filter))
                        {
                            ExpandFilter(ing.filter, alcoholDrinks);
                            hasAlcoholIngredient = true;
                        }
                    }
                }

                if (hasAlcoholIngredient && recipe.fixedIngredientFilter != null)
                {
                    ExpandFilter(recipe.fixedIngredientFilter, alcoholDrinks);
                }
            }
        }

        private static bool IsAlcoholSolventFilter(ThingFilter filter)
        {
            return filter != null && filter.customSummary == AlcoholSummary;
        }

        private static void ExpandFilter(ThingFilter filter, List<ThingDef> alcoholDrinks)
        {
            for (int i = 0; i < alcoholDrinks.Count; i++)
            {
                filter.SetAllow(alcoholDrinks[i], true);
            }
        }

        private static List<ThingDef> FindAlcoholDrinks(ChemicalDef alcohol)
        {
            List<ThingDef> list = new List<ThingDef>();
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                CompProperties_Drug drug = def.GetCompProperties<CompProperties_Drug>();
                if (drug != null && drug.chemical == alcohol)
                {
                    list.Add(def);
                }
            }
            return list;
        }
    }
}
