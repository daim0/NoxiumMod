using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Armor.Oculum
{
    [AutoloadEquip(EquipType.Body)]
    class OculumChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+4 minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 10000;
            item.rare = 2;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 4;
        }
    }
}

