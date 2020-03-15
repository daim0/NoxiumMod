using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Weapons.Throwing
{
    class TinyStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Star");
            Tooltip.SetDefault("Andrew is a boomer");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 14;
            item.height = 14;
            item.rare = 1;
            item.scale = 1.3f;
        }
        public override bool OnPickup(Player player)
        {
            NoxiumPlayer modPlayer = player.GetModPlayer<NoxiumPlayer>();
            item.active = false;
            Main.PlaySound(SoundID.Item3, player.Center);
            Player p = Main.player[item.owner];
            p.ManaEffect(2);
            return false;
        }
        public override bool ItemSpace(Player player)
        {
            return true;
        }
    }
}
