using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Materials
{
    class GoblinTechPiece : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 22;
            item.maxStack = 999;
            item.value = Item.buyPrice(silver: 5);
        }
    }
}
