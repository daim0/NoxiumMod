using Terraria;
using Terraria.ModLoader;
using NoxiumMod.Projectiles.Summons;

namespace NoxiumMod.Items.Buffs
{
    class SentientTetherBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sentient Tether Minion");
            Description.SetDefault("A Sentient Tether fights for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SentientTether>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
