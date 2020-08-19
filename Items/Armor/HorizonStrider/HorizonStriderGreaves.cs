using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace NoxiumMod.Items.Armor.HorizonStrider
{
    [AutoloadEquip(EquipType.Legs)]
    class HorizonStriderGreaves : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 45;
        }
    }
}
