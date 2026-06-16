# Witcher Potions

Brew and drink witcher potions in **RimWorld**. This companion mod to the Witcher suite adds alchemical draughts crafted from vanilla ingredients — powerful enough to turn the tide of a fight or stitch a witcher back together, but lethally toxic to ordinary humans.

> **Standalone.** No DLC required. Works alongside [Witcher Base](../witcher) but does not depend on it.

## What it does

Research **witcher alchemy**, then brew two potions at a **drug lab**:

| Potion | Effect | Ingredients |
|--------|--------|-------------|
| **Swallow** (green) | Heals injuries over time (~3 hours); prioritizes bleeding wounds | 4 herbal medicine, 2 neutroamine |
| **Thunderbolt** (yellow) | +30% melee damage, +4 hit chance, +4 shooting accuracy, faster aim (~2 hours) | 6 psychoid leaves, 2 neutroamine |

Both potions are **drunk** from the item menu (like other ingestible drugs).

## Toxicity

Every dose applies **toxic buildup** — the same hediff pawns get from toxic fallout — scaled by the drinker's **toxic resistance** stat:

- Normal colonists (0% resistance) take a full lethal dose: **a single potion drives them to 100% toxic buildup and kills them.**
- Witcher Base adds graduated witcher toxin tolerance genes: initiates take 60% buildup per dose, trained witchers take 40%, master witchers take 25%, and mutated witchers take 15%.
- Enough rapid potion use can still kill any witcher tier. A master witcher can handle several doses, while a mutated witcher can drink many more, but neither is fully immune.
- Other toxin-resistance sources also help because the potion uses RimWorld's vanilla `ToxicResistance` stat.

There is no addiction or chemical dependency — only the immediate toxic cost. Toxic buildup decays slowly (~8%/day) once a pawn stops drinking.

## Compatibility

- Built for **RimWorld 1.6**.
- Vanilla ingredients only; no DLC required.
- Safe to add mid-save. `loadAfter` Witcher Base when both mods are enabled.

## Credits

A fan-made tribute. The Witcher universe and its lore belong to CD Projekt Red and Andrzej Sapkowski.
