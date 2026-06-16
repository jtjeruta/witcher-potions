# Witcher Potions — Developer Notes

Internal documentation for contributors. **Spoils gameplay mechanics** — keep this out of the player-facing README and the Steam description.

## Core design

Two craftable ingestible drugs apply a beneficial hediff plus a toxicity effect that depends on the drinker's vanilla `ToxicResistance` stat — no hard dependency on Witcher Base. Toxin-hardened drinkers (witchers) can use potions; ordinary humans are poisoned to death.

Weapon oils / blade coatings are **deferred** (phase 2). They need a custom weapon comp and are not an Anomaly DLC feature.

## Toxicity model

`IngestionOutcomeDoer_ToxicByResistance` runs on drink and branches on `ToxicResistance` vs `safeToxicResistance` (default 0.35):

- **Tolerant drinker** (`ToxicResistance >= 0.35`, i.e. any witcher tier — the tolerance genes start at 0.40): add scaled vanilla `ToxicBuildup`, `toxicAmount * (1 - ToxicResistance) * ingestedCount` (`toxicAmount = 0.6`), **but clamped so our contribution never exceeds `witcherToxicCeiling` (0.85)**. This matters: vanilla `ToxicBuildup` has `<lethalSeverity>1.0</lethalSeverity>`, so reaching 100% is fatal. By capping at 0.85 we keep witchers inside the `extreme` stage (>=0.80, which `setMax`-caps consciousness at 10% → downed/asleep) without ever hitting the lethal 100%. So chugging potions makes a witcher **pass out and sleep it off**, mirroring the games, and the potions alone are never fatal for them.
- **Ordinary human** (`ToxicResistance < 0.35`): apply the `WitcherPotion_Toxin` hediff. This is a pure-vanilla lethal countdown — `HediffComp_SeverityPerDay severityPerDay = 1.0` ramps it to `lethalSeverity = 1.0` over ~1 in-game day, killing the pawn. There is no cure. Re-drinking does not stack (it is only added if absent).

This is deliberately binary for simplicity: witchers manage (capped) toxic buildup and can faint, humans who drink any potion die within a day.

**Implementation detail:** the clamp reads the pawn's current `ToxicBuildup` severity and only adds `min(current + added, 0.85) - current` (skipped if already at/above the ceiling). It never *reduces* existing buildup, and it only caps what *potions* add — external toxic fallout can still push a pawn to 100% and kill them, which is intended/vanilla.

Witcher tiers and roughly how many potions (in quick succession, from 0 buildup) it takes to faint — i.e. reach the `extreme` stage at 0.80, clamped to 0.85:

| Gene | Witcher stage | ToxicResistance | Buildup per potion | Faints around | Dies from potions? |
|------|---------------|-----------------|--------------------|---------------|--------------------|
| `Witcher_ToxTolerance_I` | initiate | 40% | +0.36 | 3rd potion | never (capped 0.85) |
| `Witcher_ToxTolerance_II` | witcher | 60% | +0.24 | 4th potion | never (capped 0.85) |
| `Witcher_ToxTolerance_III` | master witcher | 75% | +0.15 | 6th potion | never (capped 0.85) |
| `Witcher_ToxTolerance_IV` | mutated witcher | 85% | +0.09 | 9th potion | never (capped 0.85) |

Toxic buildup recovers at the vanilla rate (~8%/day) once no new toxicity is added, so spacing potions out prevents fainting. (Note: the `moderate`+ stages can still roll the vanilla dementia/carcinoma `hediffGivers`, a minor long-term cost of heavy potion use.)

There is intentionally no custom "is this pawn a witcher?" check — `ToxicResistance` is the universal in-game gauge for "can handle toxins," shared with toxic fallout, venom, and poisoned weapons. Any high-tox-resistance source (detoxifier kidney, tox gene, etc.) also clears the bar.

## Hediffs

| Hediff | Duration | Behavior |
|--------|----------|----------|
| `WitcherPotion_SwallowHediff` (swallow, green) | 7500 ticks (~3 h) | `HediffComp_HealOverTime` every 120 ticks: heals 0.6 severity on worst injury, bleeding wounds first |
| `WitcherPotion_ThunderboltHediff` (thunderbolt, yellow) | 5000 ticks (~2 h) | Stat buffs only (no custom C#) |
| `WitcherPotion_Toxin` (lethal, humans only) | ~1 in-game day to death | Vanilla `HediffComp_SeverityPerDay` (1.0/day) ramps to `lethalSeverity = 1.0`; staged decline (spreading → toxic shock → dying), no cure |

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

- `toxicAmount` / `safeToxicResistance` / `witcherToxicCeiling` in ThingDef outcome doers
- `severityPerDay` on `WitcherPotion_Toxin` (1.0 = ~1 day to death)
- `disappearsAfterTicks` on hediff comps
- `ticksInterval` / `healAmount` on `HediffCompProperties_HealOverTime`
- Attack hediff `statFactors` / `statOffsets` in XML
