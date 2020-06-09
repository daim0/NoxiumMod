using Microsoft.Xna.Framework;
using NoxiumMod.Items.Placeable.Plasma;
using NoxiumMod.Projectiles.PlasmaStuff;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace NoxiumMod.Tiles.Plasma
{
    class PlasmaSandNoFall : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSand[Type] = true;
            Main.tileMerge[Type][TileType<PlasmaSandstone>()] = true;
            TileID.Sets.TouchDamageSands[Type] = 15;
            TileID.Sets.Conversion.Sand[Type] = true; // Allows Clentaminator solutions to convert this tile to their respective Sand tiles.
            TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true; // Allows Sandshark enemies to "swim" in this sand.
            TileID.Sets.Falling[Type] = true;
            AddMapEntry(new Color(109, 207, 184));
            drop = ModContent.ItemType<PlasmaSandItem>();
            SetModTree(new PlasmaTree());
        }
    }
}
