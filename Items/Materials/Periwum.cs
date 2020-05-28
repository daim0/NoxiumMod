using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Materials
{
    class Periwum : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.maxStack = 999;
            item.value = Item.buyPrice(silver: 5);
        }
    }
}
