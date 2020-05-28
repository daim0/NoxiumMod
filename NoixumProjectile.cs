using NoxiumMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod
{
	internal class NoixumProjectile : GlobalProjectile
	{
		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.owner == Main.myPlayer)
			{
				if (projectile.minion)
				{
					if (Main.player[projectile.owner].GetModPlayer<NoxiumPlayer>().fireMinion)
						target.AddBuff(BuffID.OnFire, new Time(2).Ticks);
				}
			}
		}
	}
}