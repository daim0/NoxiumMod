using System;
using Terraria;
using Terraria.ModLoader;
using NoxiumMod.Projectiles.Pets;

namespace NoxiumMod.Items.Buffs
{
    class PotionPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Potion Pet");
            Description.SetDefault("ae");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<NoxiumPlayer>().PotionPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PotionHead>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
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
        }
    }
}
