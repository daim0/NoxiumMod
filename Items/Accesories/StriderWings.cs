using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;


namespace NoxiumMod.Items.Accesories
{
    [AutoloadEquip(EquipType.Wings)]
    class StriderWings : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 24;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 180;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.135f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 7f;
            acceleration *= 1.2f;
        }
        public override void UpdateVanity(Player player, EquipType type)
        {
            NoxiumPlayer sgaplayer = player.GetModPlayer(mod, typeof(NoxiumPlayer).Name) as NoxiumPlayer;
            if (!Main.dedServ)
            {
                sgaplayer.armorglowmasks[5] = "NoxiumMod/Items/Accesories/StriderWings_Wings_Glow";
            }
        }
    }
}
