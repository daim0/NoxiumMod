using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using NoxiumMod.Tiles.Plasma;

namespace NoxiumMod.Items.Placeable.Plasma
{
    class SealedMoonlessStoneItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sealed Moonless Stone");
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
            item.createTile = TileType<Tiles.Plasma.SealedMoonlessStone>();
            item.width = 14;
            item.height = 16;
            item.value = 300;
        }
    }
}
