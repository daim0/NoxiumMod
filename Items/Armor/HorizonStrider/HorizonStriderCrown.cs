using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace NoxiumMod.Items.Armor.HorizonStrider
{
    [AutoloadEquip(EquipType.Head)]
    class HorizonStriderCrown : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 20;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 30;
        }
    }
}
