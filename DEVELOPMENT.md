# Witcher Potions — Developer Notes

Internal documentation for contributors. **Spoils gameplay mechanics** — keep this out of the player-facing README and the Steam description.

## Core design

Three craftable ingestible drugs branch on the drinker's vanilla `ToxicResistance` stat — no hard dependency on Witcher Base. Toxin-hardened drinkers (witchers) can use potions; ordinary humans are poisoned to death. Swallow/Thunderbolt apply a beneficial hediff plus capped toxic buildup; White Honey instead *clears* a witcher's toxicity (and still kills humans). **Black Blood** taints the bloodstream for ~6 h with heavier toxicity than other draughts; Witcher Monster Quests uses the active hediff to block revenant hypnosis and poison melee attackers.

Reagents come from **foraging** mod-added wild herbs (Bryorin, Myrleaf, Hellevine), vanilla healroot/herbal medicine, brewed alcohol, insect jelly, and optionally Anomaly shards. Potions brew at **campfire** (Cooking) or **drug lab** (Intellectual) via explicit `RecipeDef`s — not auto-generated `costList` recipes.

Weapon oils / blade coatings are **deferred** (phase 2). They need a custom weapon comp and are not an Anomaly DLC feature.

## Wild herbs

Defined in `Defs/ThingDefs/WitcherPotions_Herbs.xml`. Each herb is a harvested `ThingDef` plus a wild `Plant_*` def with **no `sowTags`** (forage-only).

Biome spawn weights are patched in `Patches/WitcherPotions_BiomeHerbs.xml`. Herbs only appear on newly generated map tiles.

| Plant | Item | Biomes (weight) |
|-------|------|-----------------|
| `Plant_WitcherBryorin` | `WitcherHerb_Bryorin` | AridShrubland 0.05, Desert 0.04, TemperateForest 0.05 |
| `Plant_WitcherMyrleaf` | `WitcherHerb_Myrleaf` | BorealForest 0.05, TemperateForest 0.05, Tundra 0.02 |
| `Plant_WitcherHellevine` | `WitcherHerb_Hellevine` | TemperateSwamp 0.03, TropicalSwamp 0.03, ColdBog 0.025 |

## Recipes

`Defs/RecipeDefs/WitcherPotions_Recipes.xml` — **8 recipes** (4 potions × campfire + drug lab). Potion ThingDefs parent `DrugBase` directly (not `MakeableDrugBase`) to avoid duplicate auto-recipes.

| Potion | Ingredients |
|--------|-------------|
| Swallow | 2 MedicineHerbal, 1 Myrleaf, 1 alcohol |
| Thunderbolt | 1 MedicineHerbal, 2 Bryorin, 1 alcohol |
| White Honey | 2 Myrleaf, 2 InsectJelly, 1 alcohol |
| Black Blood | 3 Hellevine, 1 alcohol — or **2 Hellevine + 1 Shard + 1 alcohol** when Anomaly loaded (`Patches/WitcherPotions_BlackBlood_Anomaly.xml`) |

Alcohol ingredient uses a `thingDefs` filter (Beer today; extend for wine mods). Campfire recipes use `DrugCookingSpeed` / Cooking; drug lab uses `DrugSynthesisSpeed` / Intellectual. Campfire has vanilla 0.5× work-table speed.

Regenerate herb/plant PNGs: `tools/generate_herb_textures.py` (requires Pillow).

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

### White Honey

`IngestionOutcomeDoer_WhiteHoney` uses the same `safeToxicResistance` split:

- **Witcher** (`>= 0.35`): removes the `ToxicBuildup` hediff entirely (and `WitcherPotion_Toxin` if somehow present), resetting toxicity to zero so they can immediately drink more potions. No beneficial hediff is applied — the purge *is* the effect.
- **Ordinary human** (`< 0.35`): identical to the other potions — applies the lethal `WitcherPotion_Toxin` hediff. White honey does not cure a poisoned human; it poisons them too.

## Hediffs

| Hediff | Duration | Behavior |
|--------|----------|----------|
| `WitcherPotion_SwallowHediff` (swallow, green) | 7500 ticks (~3 h) | `HediffComp_HealOverTime` on apply + every 120 ticks: auto-tends all injuries (self-tend quality, no medicine); heals 0.6 severity on worst injury, bleeding wounds first |
| `WitcherPotion_ThunderboltHediff` (thunderbolt, yellow) | 5000 ticks (~2 h) | Stat buffs only (no custom C#) |
| `WitcherPotion_BlackBloodHediff` (black blood) | 15000 ticks (~6 h) | Revenant counter; contact poison handled in Witcher Monster Quests via `BlackBloodUtility.HasBlackBlood`; `toxicAmount` 1.0 on drink (other potions 0.6) |
| `WitcherPotion_Toxin` (lethal, humans only) | ~1 in-game day to death | Vanilla `HediffComp_SeverityPerDay` (1.0/day) ramps to `lethalSeverity = 1.0`; staged decline (spreading → toxic shock → dying), no cure |

`HediffComp_HealOverTime` auto-tends all `Hediff_Injury` wounds while Swallow is active (immediate on drink, then each heal tick). Tend quality uses self-tend via `TendUtility.CalculateBaseTendQuality` — no colonist job or medicine required. Infections and other non-injury tendables are unaffected.

## Repository layout

```
About/                 Mod metadata
Assemblies/            Compiled WitcherPotions.dll
Defs/
  ThingDefs/           Potions, wild herbs/plants
  RecipeDefs/          Campfire + drug lab potion recipes
  HediffDefs/          Buff / heal hediffs
  ResearchProjectDefs/ Witcher Alchemy research
Patches/               Biome wildPlants, Anomaly Black Blood
Source/
  WitcherPotions/      Ingestion doers (toxicity / white honey), heal comp, DefOf
  build.sh             Offline build script
Textures/Things/       Potion bottles, herb items, plant sprites
tools/                 Texture generation script
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
- Potion ThingDefs parent `DrugBase` (not `MakeableDrugBase`). Recipes live in `Defs/RecipeDefs/` — do not add `costList` + `recipeMaker` on potions (duplicate recipe errors).
- `ParentName="MakeableDrugBase"` auto-generates recipes from `costList`; we use explicit `RecipeDef`s instead with `researchPrerequisite` `Witcher_Alchemy`. **Witcher Alchemy** requires **`Witcher_Traditions`** (Witcher Base) plus vanilla **Drug production**.
- `CompProperties_Drug` with no chemical keeps them non-addictive.

## Tuning knobs

- `workAmount` on potion RecipeDefs (900 — matches Wake-up)
- Biome `wildPlants` weights in `Patches/WitcherPotions_BiomeHerbs.xml`
- Plant `harvestYield` / `wildClusterWeight` in `WitcherPotions_Herbs.xml`
- `toxicAmount` / `safeToxicResistance` / `witcherToxicCeiling` in ThingDef outcome doers
- `severityPerDay` on `WitcherPotion_Toxin` (1.0 = ~1 day to death)
- `disappearsAfterTicks` on hediff comps
- `ticksInterval` / `healAmount` on `HediffCompProperties_HealOverTime`
- Attack hediff `statFactors` / `statOffsets` in XML
