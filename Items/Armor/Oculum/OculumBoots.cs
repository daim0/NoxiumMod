using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Armor.Oculum
{
    [AutoloadEquip(EquipType.Legs)]
    class OculumBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+20 max mana");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 12;
            item.value = 10000;
            item.rare = 2;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 20;
        }
    }
}
