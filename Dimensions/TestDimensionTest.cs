using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;

namespace NoxiumMod.Dimensions
{
    class TestDimensionTest : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.value = 1000;
            item.useStyle = 4;
            item.rare = 3;
        }
        public override bool UseItem(Player player)
        {
            Subworld.Enter<PlasmaDesert>();
            return true;
        }
    }
}
