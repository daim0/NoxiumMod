using System;
using Microsoft.Xna.Framework;
using Terraria;
using NoxiumMod.Items.Buffs;
using NoxiumMod.Projectiles.Pets;
using NoxiumMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Other
{
    class GiganticHealingPotion : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 44;

            item.damage = 0;

            item.useStyle = 1;
            item.useAnimation = 20;
            item.useTime = 20;
            item.UseSound = SoundID.Item2;

            item.rare = 11;
            item.value = Item.sellPrice(0, 5, 0, 0);

            item.noMelee = true;

            item.shoot = ModContent.ProjectileType<ChartreumBladeMainProj>(); // shoot isnt run if its a pet projectile
            item.shootSpeed = 1f;
            item.buffType = ModContent.BuffType<PotionPetBuff>();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a Pet Healing Potion");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PotionHead>()] <= 0)
            {
                int current = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PotionHead>(), 0, 0f, Main.myPlayer);

                int previous = current;
                for (int k = 0; k < 8; k++)
                {
                    previous = current;
                    current = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PotionBody>(), 0, 0f, Main.myPlayer, previous);
                }
                previous = current;
                current = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<PotionTail>(), 0, 0f, Main.myPlayer, previous);
                Main.projectile[previous].localAI[1] = (float)current;
                Main.projectile[previous].netUpdate = true;

            }
            return false;
        }
        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
