using Terraria;
using Terraria.ModLoader;
namespace NoxiumMod.Items.Materials
{
    class EssenceOfMalice : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 999;
            item.value = Item.buyPrice(silver: 5);
        }
    }
}
