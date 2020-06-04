using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.UI.Dimensions
{
    public class TestItem : ModItem
    {
        public override string Texture => "Terraria/Item_" + ItemID.GoldWatch;

        public override void SetDefaults()
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool UseItem(Player player)
        {
            NoxiumMod.noxiumInstance.ToggleDimensionalUI();
            return true;
        }
    }
}
