using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
	class AncientHealingCore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Healing Core");
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 1000;
			npc.damage = 50;
			npc.defense = 45;
			npc.knockBackResist = 0f;
			npc.width = 36;
			npc.height = 80;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			npc.immortal = true;
			npc.dontTakeDamage = true;

			npc.velocity = new Vector2(0f, -14f);
			if (npc.timeLeft > 269)
			{
				npc.timeLeft = 269;
			}
			int flyAwaylol = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 61, 0f, 0f, 100, default(Color), 1f);
			Main.dust[flyAwaylol].noGravity = true;
			Main.dust[flyAwaylol].scale = 1.15f;
		}
	}
}