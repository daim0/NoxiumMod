using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Tiles.Plasma
{
    class SealedMoonlessStone : ModTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileValue[Type] = 410;
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Moonless Stone");
            AddMapEntry(new Color(107, 20, 86), name);

            dustType = 84;
            drop = ItemType<Items.Placeable.Plasma.SealedMoonlessStoneItem>();
            soundType = SoundID.Tink;
            soundStyle = 1;
            mineResist = 4f;
            minPick = 64;
        }
    }
}
