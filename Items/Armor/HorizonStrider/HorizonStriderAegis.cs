using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.Items.Armor.HorizonStrider
{
    [AutoloadEquip(EquipType.Body)]
    class HorizonStriderAegis : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 1000;
            item.rare = 3;
            item.defense = 20;
        }
    }
}
