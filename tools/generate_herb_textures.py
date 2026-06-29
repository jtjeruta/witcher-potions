#!/usr/bin/env python3
"""Generate witcher herb item and plant placeholder textures."""
from PIL import Image, ImageDraw, ImageFilter
import os

ROOT = "/Users/tristan/Projects/Personal/witcher-potions/Textures"
ITEM = os.path.join(ROOT, "Things/Item")
PLANT = os.path.join(ROOT, "Things/Plant")
os.makedirs(ITEM, exist_ok=True)
os.makedirs(PLANT, exist_ok=True)


def save_rgba(img, path, size=None):
    if size:
        img = img.resize((size, size), Image.LANCZOS)
    img.save(path, "PNG")


def herb_item(name, leaf_rgb, stem_rgb):
    img = Image.new("RGBA", (256, 256), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    cx, cy = 128, 140
    # stem
    d.line([(cx, cy + 40), (cx, cy - 30)], fill=(*stem_rgb, 255), width=8)
    # leaf cluster
    for dx, dy, w, h in [(-35, -10, 55, 90), (35, -10, 55, 90), (0, -45, 70, 55)]:
        d.ellipse([cx + dx - w // 2, cy + dy - h // 2, cx + dx + w // 2, cy + dy + h // 2], fill=(*leaf_rgb, 230))
    d.ellipse([cx - 18, cy - 55, cx + 18, cy - 20], fill=(*leaf_rgb, 255))
    img = img.filter(ImageFilter.SMOOTH)
    save_rgba(img, os.path.join(ITEM, f"WitcherHerb_{name}.png"))


def plant_sprite(name, leaf_rgb, stem_rgb, immature=False):
    img = Image.new("RGBA", (256, 256), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    scale = 0.65 if immature else 1.0
    cx, base = 128, 210
    h = int(90 * scale)
    # ground shadow
    d.ellipse([cx - 55, base - 8, cx + 55, base + 18], fill=(0, 0, 0, 45))
    # stems
    for ox in (-25, 0, 25):
        d.line([(cx + ox, base), (cx + ox // 2, base - h)], fill=(*stem_rgb, 255), width=max(3, int(6 * scale)))
        lw, lh = int(40 * scale), int(55 * scale)
        top = base - h
        d.ellipse([cx + ox - lw, top - lh, cx + ox + lw, top + lh // 3], fill=(*leaf_rgb, 220 if immature else 240))
    if name == "Hellevine":
        # vine tendrils
        d.arc([60, 80, 196, 220], 200, 340, fill=(*leaf_rgb, 200), width=4)
    img = img.filter(ImageFilter.SMOOTH)
    suffix = "_Immature" if immature else ""
    save_rgba(img, os.path.join(PLANT, f"Witcher{name}{suffix}.png"))


# Bryorin — dusty gold / brown (combat stimulant, arid)
herb_item("Bryorin", (180, 150, 60), (100, 75, 35))
plant_sprite("Bryorin", (190, 160, 70), (110, 85, 40))
plant_sprite("Bryorin", (150, 130, 55), (90, 70, 30), immature=True)

# Myrleaf — soft green (purification)
herb_item("Myrleaf", (90, 170, 110), (55, 110, 65))
plant_sprite("Myrleaf", (100, 180, 120), (60, 120, 70))
plant_sprite("Myrleaf", (80, 140, 95), (50, 95, 55), immature=True)

# Hellevine — dark purple-green (poison vine)
herb_item("Hellevine", (70, 120, 55), (45, 35, 60))
plant_sprite("Hellevine", (65, 110, 50), (40, 30, 55))
plant_sprite("Hellevine", (55, 90, 45), (35, 25, 45), immature=True)

print("Generated herb and plant textures in", ROOT)
