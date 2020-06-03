using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NoxiumMod.Projectiles.Summons;
using NoxiumMod.Items.Buffs;

namespace NoxiumMod.Items.Weapons.Moonless
{
    class SentientTetherRemote : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.value = 1000;
            item.useStyle = 4;
            item.rare = 3;
            item.rare = 6;
            item.crit = 4;
            item.mana = 7;
            item.damage = 35;
            item.knockBack = 0;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("SentientTether");
            item.buffType = mod.BuffType("SentientTetherBuff");
            item.buffTime = 3600;
            item.UseSound = SoundID.Item44;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.altFunctionUse != 2;
            position = Main.MouseWorld;
            speedX = speedY = 0;
            return true;
        }
    }
}
