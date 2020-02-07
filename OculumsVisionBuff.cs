using NoxiumMod.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Items.Buffs
{
	public class OculumsVisionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Oculum's Vision");
			Description.SetDefault("The bouncing eyeball will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<OculumsEyeProj>()] > 0)
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