using Terraria;
using Terraria.ModLoader;

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
            if (player.ownedProjectileCounts[mod.ProjectileType("Crawlerock")] > 0)
            {
                player.GetModPlayer<NoxiumPlayer>().SentientTetherMinion = false;
            }

            if (!player.GetModPlayer<NoxiumPlayer>().SentientTetherMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }

            player.buffTime[buffIndex] = 18000;
        }
    }
}
