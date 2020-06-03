using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Placeable
{
    class PortalFrame : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 20;
            item.maxStack = 99;
            item.value = 750;
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = TileType<Tiles.PortalFrameTile>();
            item.placeStyle = 0;
        }
    }
}
