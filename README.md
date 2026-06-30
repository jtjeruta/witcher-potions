# Witcher Potions

Brew and drink witcher potions in **RimWorld**. This companion mod to the Witcher suite adds alchemical draughts brewed from **foraged witcher herbs** — powerful enough to turn the tide of a fight or stitch a witcher back together, but lethally toxic to ordinary humans.

> **Standalone.** No DLC required (Anomaly optional for Black Blood shard recipe). Works alongside [Witcher Base](../witcher).

## What it does

Research **witcher traditions** in [Witcher Base](../witcher), then **witcher alchemy** (also requires vanilla **drug production**). Forage wild herbs on the map, gather healroot and beer, then brew at a **campfire** or **drug lab**.

### Forageable herbs

Wild-only plants — cannot be sown in growing zones. Harvest with the Plants skill.

| Herb | Use | Rarity | Biomes |
|------|-----|--------|--------|
| **Bryorin** | Combat stimulant (Thunderbolt) | Uncommon | Arid shrubland, desert, temperate forest |
| **Myrleaf** | Purification (Swallow, White Honey) | Uncommon–rare | Boreal forest, temperate forest; rare in tundra |
| **Hellevine** | Toxic brews (Black Blood) | Rare | Temperate swamp, tropical swamp, cold bog |

Herbs only appear on **newly generated map tiles** — existing explored areas won't gain them mid-save.

Also gather **herbal medicine** (wild or cultivated healroot), **beer** (or other alcohol in the recipe filter), **insect jelly** (hives), and optionally an Anomaly **shard** for Black Blood.

### Potions

| Potion | Effect | Ingredients |
|--------|--------|-------------|
| **Swallow** (green) | Heals injuries (~3 h); auto-tends wounds; prioritizes bleeding | 2 herbal medicine, 1 myrleaf, 1 alcohol |
| **Thunderbolt** (yellow) | +30% melee damage, +4 hit/accuracy, faster aim (~2 h) | 1 herbal medicine, 2 bryorin, 1 alcohol |
| **White Honey** (orange) | Instantly purges all toxicity from a witcher | 2 myrleaf, 2 insect jelly, 1 alcohol |
| **Black Blood** | Revenant counter; poisons attackers (~6 h) | 3 hellevine + 1 alcohol *(or 2 hellevine + 1 shard + 1 alcohol with Anomaly)* |

Potions are **drunk** from the item menu like other ingestible drugs.

### Witcher trials (with Witcher Base)

When both mods are enabled, trial operations in the Health tab use foraged herbs and brewed potions instead of vanilla psychoid and smokeleaf:

| Trial | Ingredients |
|-------|-------------|
| **Trial of Grasses** | 3 herbal medicine, 5 myrleaf, 2 bryorin, 1 alcohol |
| **Trial of Dreams** | 3 herbal medicine, 2 thunderbolt, 1 alcohol |
| **Additional Mutagens** | 2 swallow, 2 thunderbolt, 3 hellevine |

Typical loop: forage herbs → conduct Grasses on a child → research witcher alchemy and brew potions → Dreams on the initiate → forage hellevine from swamps for Additional Mutagens on a master witcher.

## Toxicity

Witcher potions are mutagenic poison. Whether they help or kill depends entirely on the drinker's **toxic resistance** stat:

- **Ordinary humans** (no toxin tolerance) cannot survive them. Drinking **any** witcher potion inflicts *witcher toxins* — a fatal poisoning that worsens over roughly **one in-game day** until the pawn dies. There is no cure.
- **Witchers** (Witcher Base's toxin-tolerance genes, or any other source of toxic resistance) can drink safely. Instead of poisoning them, potions add ordinary **toxic buildup**, scaled down by their resistance. Just like in the games, downing too many in a row will eventually **knock a witcher unconscious** — but it will never kill them. They sleep it off as the buildup decays.
- Higher witcher tiers shrug off more before fainting: an initiate gets woozy after a few quick doses, while a mutated witcher can drink many.
- **White Honey** lets a witcher cheat the limit: it instantly wipes their toxic buildup to zero, so they can immediately drink fresh potions. It is *not* a rescue for poisoned humans, though — to a non-witcher it is just another lethal dose.

There is no addiction or chemical dependency. A witcher's toxic buildup decays slowly (~8%/day) once they stop drinking, or instantly with white honey.

## Compatibility

- Built for **RimWorld 1.6**.
- Core content uses vanilla reagents plus mod herbs; **Anomaly** optional (shard substitutes into Black Blood recipe when installed).
- Safe to add mid-save. `loadAfter` Witcher Base when both mods are enabled.

## Credits

A fan-made tribute. The Witcher universe and its lore belong to CD Projekt Red and Andrzej Sapkowski.
