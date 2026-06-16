# Witcher Potions — Developer Notes

Internal documentation for contributors. **Spoils gameplay mechanics** — keep this out of the player-facing README and the Steam description.

## Core design

Two craftable ingestible drugs apply a beneficial hediff plus scaled toxic buildup. Toxicity uses the vanilla `ToxicResistance` stat — no hard dependency on Witcher Base. Baseline humans die from one dose; Witcher Base's graduated toxin-tolerance genes reduce the dose but intentionally do not grant full immunity.

Weapon oils / blade coatings are **deferred** (phase 2). They need a custom weapon comp and are not an Anomaly DLC feature.

## Toxicity math

`IngestionOutcomeDoer_ToxicByResistance` runs on drink:

```
effectiveToxicity = toxicAmount * (1 - ToxicResistance) * ingestedCount
HealthUtility.AdjustSeverity(pawn, ToxicBuildup, effectiveToxicity)
```

Default `toxicAmount = 1.0` per dose. `ToxicBuildup` (the vanilla toxic-fallout/pollution hediff) is **fatal at severity 1.0** (its `maxSeverity`), so a 0% resistance pawn reaches the lethal cap from a single potion and dies. `ToxicResistance` linearly reduces the applied amount. `AdjustSeverity` is additive and clamps at `maxSeverity`, so stacked doses on a partially-resistant pawn accumulate toward the lethal cap.

Witcher Base currently supplies:

| Gene | Witcher stage | ToxicResistance | Buildup per potion | Rapid-dose lethal point |
|------|---------------|-----------------|--------------------|-------------------------|
| `Witcher_ToxTolerance_I` | initiate | 40% | +0.60 | 2nd potion |
| `Witcher_ToxTolerance_II` | witcher | 60% | +0.40 | 3rd potion |
| `Witcher_ToxTolerance_III` | master witcher | 75% | +0.25 | 4th potion |
| `Witcher_ToxTolerance_IV` | mutated witcher | 85% | +0.15 | 7th potion |

These thresholds assume no recovery time between drinks. Toxic buildup recovers at the vanilla rate (~8%/day) once no new toxicity is added.

There is intentionally no custom "is this pawn a witcher?" check — `ToxicResistance` is the universal in-game gauge for "can handle toxins," shared with toxic fallout, venom, and poisoned weapons. Any high-tox-resistance source qualifies.

## Hediffs

| Hediff | Duration | Behavior |
|--------|----------|----------|
| `WitcherPotion_SwallowHediff` (swallow, green) | 7500 ticks (~3 h) | `HediffComp_HealOverTime` every 120 ticks: heals 0.6 severity on worst injury, bleeding wounds first |
| `WitcherPotion_ThunderboltHediff` (thunderbolt, yellow) | 5000 ticks (~2 h) | Stat buffs only (no custom C#) |

`HediffComp_HealOverTime` cannot assign `Hediff.Bleeding` (read-only in 1.6). Bleeding is mitigated indirectly by prioritizing bleeding injuries for severity reduction.

## Repository layout

```
About/                 Mod metadata
Assemblies/            Compiled WitcherPotions.dll
Defs/
  ThingDefs/           Drinkable potion items (recipes auto-generated via recipeMaker)
  HediffDefs/          Buff / heal hediffs
  ResearchProjectDefs/ Witcher Alchemy research
Source/
  WitcherPotions/      Ingestion doer, heal comp, DefOf
  build.sh             Offline build script
Textures/Things/Item/  Potion bottle icons
LoadFolders.xml
```

## Building

```bash
./Source/build.sh
```

Outputs `Assemblies/WitcherPotions.dll`. Override paths with `RIMWORLD_MANAGED`, `DOTNET`, and `CSC_DLL` env vars if auto-detection fails.

## Dev workflow

Symlink the repo into RimWorld's `Mods/` folder (`Mods/WitcherPotions`) so XML and DLL changes load on game restart.

## Conventions / gotchas

- C# namespace is `WitcherPotions`; XML `Class=` references use the `WitcherPotions.` prefix.
- Use `System.Math` rather than `UnityEngine.Mathf` (netstandard reference error under game Mono).
- Vanilla drug textures live in asset bundles — ship custom `texPath` icons under `Textures/`.
- Use `Graphic_Single` for a single icon file. `Graphic_StackCount`/`Graphic_Random` are `Graphic_Collection` types that scan a *folder* of textures and throw "Collection cannot init: No textures found at path" when pointed at one file.
- Icons should be square RGBA with a transparent background; an opaque RGB image renders as a solid rectangle.
- `ParentName="MakeableDrugBase"` for craftable drugs; it auto-generates a `Make_<defName>` recipe from the ThingDef `costList` and its inherited `recipeMaker`. Do NOT also declare an explicit `Make_<defName>` RecipeDef (duplicate def error), and do provide a `costList` (else "has a recipeMaker but no costList or costStuffCount"). Gate the recipe by adding `<recipeMaker><researchPrerequisite>...</researchPrerequisite></recipeMaker>`.
- `CompProperties_Drug` with no chemical keeps them non-addictive.

## Tuning knobs

- `toxicAmount` in ThingDef outcome doers
- `disappearsAfterTicks` on hediff comps
- `ticksInterval` / `healAmount` on `HediffCompProperties_HealOverTime`
- Attack hediff `statFactors` / `statOffsets` in XML
