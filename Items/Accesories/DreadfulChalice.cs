using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Accesories
{
    class DreadfulChalice : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 40;
            item.value = 1000;
            item.useStyle = 4;
            item.rare = 3;
        }
    }
}
