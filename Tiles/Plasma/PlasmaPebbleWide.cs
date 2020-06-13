using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Tiles.Plasma
{
    class PlasmaPebbleWide : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);
        }
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }
    }
}
