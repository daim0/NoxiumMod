using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Tiles.Plasma
{
    class PlasmaSandstoneWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(44, 65, 102));
        }
    }
}
