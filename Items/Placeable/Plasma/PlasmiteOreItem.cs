using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Tiles.Plasma;

namespace NoxiumMod.Items.Placeable.Plasma
{
    class PlasmiteOreItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasmite Ore");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
            item.createTile = TileType<Tiles.Plasma.PlasmiteOre>();
            item.width = 18;
            item.height = 18;
            item.value = 300;
        }
    }
}
