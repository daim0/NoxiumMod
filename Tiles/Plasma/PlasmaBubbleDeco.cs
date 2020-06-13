using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;

namespace NoxiumMod.Tiles.Plasma
{
    class PlasmaBubbleDeco : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);

            dustType = 63;
            soundType = SoundID.Waterfall;
            soundStyle = 1;
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }
    }
}
