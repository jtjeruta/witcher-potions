# Witcher Potions

Brew and drink witcher potions in **RimWorld**. This companion mod to the Witcher suite adds alchemical draughts crafted from vanilla ingredients — powerful enough to turn the tide of a fight or stitch a witcher back together, but lethally toxic to ordinary humans.

> **Standalone.** No DLC required. Works alongside [Witcher Base](../witcher) but does not depend on it.

## What it does

Research **witcher alchemy**, then brew three potions at a **drug lab**:

| Potion | Effect | Ingredients |
|--------|--------|-------------|
| **Swallow** (green) | Heals injuries over time (~3 hours); prioritizes bleeding wounds | 4 herbal medicine, 8 psychoid leaves |
| **Thunderbolt** (yellow) | +30% melee damage, +4 hit chance, +4 shooting accuracy, faster aim (~2 hours) | 12 psychoid leaves, 6 smokeleaf leaves |
| **White Honey** (orange) | Instantly purges all toxicity from a witcher, letting them reset and keep drinking | 5 insect jelly, 6 psychoid leaves |

All ingredients are **grown or foraged** — no trade-only materials. Healroot, psychoid, and smokeleaf are sowable; insect jelly is harvested from insect hives / megaspiders. Potions are **drunk** from the item menu like other ingestible drugs.

## Toxicity

Witcher potions are mutagenic poison. Whether they help or kill depends entirely on the drinker's **toxic resistance** stat:

- **Ordinary humans** (no toxin tolerance) cannot survive them. Drinking **any** witcher potion inflicts *witcher toxins* — a fatal poisoning that worsens over roughly **one in-game day** until the pawn dies. There is no cure.
- **Witchers** (Witcher Base's toxin-tolerance genes, or any other source of toxic resistance) can drink safely. Instead of poisoning them, potions add ordinary **toxic buildup**, scaled down by their resistance. Just like in the games, downing too many in a row will eventually **knock a witcher unconscious** — but it will never kill them. They sleep it off as the buildup decays.
- Higher witcher tiers shrug off more before fainting: an initiate gets woozy after a few quick doses, while a mutated witcher can drink many.
- **White Honey** lets a witcher cheat the limit: it instantly wipes their toxic buildup to zero, so they can immediately drink fresh potions. It is *not* a rescue for poisoned humans, though — to a non-witcher it is just another lethal dose.

There is no addiction or chemical dependency. A witcher's toxic buildup decays slowly (~8%/day) once they stop drinking, or instantly with white honey.

## Compatibility

- Built for **RimWorld 1.6**.
- Vanilla ingredients only; no DLC required.
- Safe to add mid-save. `loadAfter` Witcher Base when both mods are enabled.

## Credits

A fan-made tribute. The Witcher universe and its lore belong to CD Projekt Red and Andrzej Sapkowski.
