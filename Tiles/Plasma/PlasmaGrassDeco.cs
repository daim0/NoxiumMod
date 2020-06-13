using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NoxiumMod.Tiles.Plasma
{
    class PlasmaGrassDeco : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
        }
    }
}
