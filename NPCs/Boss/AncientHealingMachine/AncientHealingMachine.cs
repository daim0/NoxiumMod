using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
	class AncientHealingMachine : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Healing Machine");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 4000;
			npc.damage = 100;
			npc.defense = 55;
			npc.knockBackResist = 0f;
			npc.width = 234;
			npc.height = 120;
			npc.value = Item.buyPrice(0, 20, 0, 0);
			npc.npcSlots = 15f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.buffImmune[24] = true;
			music = MusicID.Boss2;
		}

		private const int State_Idle = 0;
		private const int State_Moving = 1;
		private const int State_LaserShot = 2;
		private const int State_Dash = 3;
		private const int State_Spin = 4;

		private const int State_Transform = 5;

		private int TimerShoot = 120;
		private float transformHP;

		private float State
		{
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		private float Timer
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		public override void AI()
		{
            NoxiumWorld.ahmSpawned = true;
            npc.TargetClosest(true);
			if (npc.HasValidTarget)
			{
				if (State == State_Idle)
				{
					if (transformHP == 0 && (float)npc.life <= (float)npc.lifeMax * 0.7f)
					{
						State = State_Transform;
						Timer = 0;
					}

					npc.velocity *= 0.065f;
					npc.velocity.Y += (float)Math.Sin(Math.PI * (Timer / 18));
					if (Main.player[npc.target].Distance(npc.Center) > 350f)
					{
						State = State_Moving;
					}

					Timer++;
					if (Timer >= 120)
					{
						Timer = 0;
						int choice = Main.rand.Next(0, 3);
						if (choice == 0)
						{
							State = State_LaserShot;
						}
						if (choice == 1)
						{
							State = State_Spin;
						}
						if (choice == 2)
						{
							State = State_Dash;
						}
					}

				}
				else if (State == State_Moving)
				{

					Timer++;
					Player player = Main.player[npc.target];
					float x = player.position.X + player.width / 2 - (npc.position.X + npc.width / 2);
					float y = player.position.Y + player.height / 2 - (npc.position.Y + npc.height / 2) - 200;
					npc.velocity += new Vector2(x, y) * (0.1f / (float)Math.Sqrt(x * x + y * y));
					if (Main.player[npc.target].Distance(npc.Center) < 200f)
					{
						State = State_Idle;
					}
				}
				else if (State == State_LaserShot)
				{

					npc.velocity *= 0.055f;
					npc.velocity.Y += (float)Math.Sin(Math.PI * (Timer / 22));
					TimerShoot--;
					if (TimerShoot <= 30)
					{
						Player player = Main.player[npc.target];

						float Speed = 4.5f;
						Vector2 vectorUno = new Vector2(npc.position.X - 75 + (npc.width / 2), npc.position.Y + 32 + (npc.height / 2));
						Vector2 vectorDos = new Vector2(npc.position.X + 75 + (npc.width / 2), npc.position.Y + 32 + (npc.height / 2));
						int damage = 69; //nice
						int type = 576; //Projectile goes here boomer
						Main.PlaySound(SoundID.Item8); //Idk what kind of sound effect should there be tbh
						float rotationUno = (float)Math.Atan2(vectorUno.Y - (player.position.Y + (player.height * 0.5f)), vectorUno.X - (player.position.X + (player.width * 0.5f)));
						float rotationDos = (float)Math.Atan2(vectorDos.Y - (player.position.Y + (player.height * 0.5f)), vectorDos.X - (player.position.X + (player.width * 0.5f)));
						int leftCrystalLaser = Projectile.NewProjectile(vectorUno.X, vectorUno.Y, (float)((Math.Cos(rotationUno) * Speed) * -2), (float)((Math.Sin(rotationUno) * Speed) * -1), type, damage, 0f, 0);
						int rightCrystalLaser = Projectile.NewProjectile(vectorDos.X, vectorDos.Y, (float)((Math.Cos(rotationDos) * Speed) * -2), (float)((Math.Sin(rotationDos) * Speed) * -1), type, damage, 0f, 0);

						TimerShoot = 80;
					}
					Timer++;
					if (Timer > 300)
					{
						Timer = 0;
						State = State_Idle;
					}
				}
				else if (State == State_Dash)
				{

					if (Timer % 60 == 0)
					{
						Player player = Main.player[npc.target];
						float x = player.position.X + player.width / 2 - (npc.position.X + npc.width / 2);
						float y = player.position.Y + player.height / 2 - (npc.position.Y + npc.height / 2);
						npc.velocity = new Vector2(x, y) * (35 / (float)Math.Sqrt(x * x + y * y));

					}
					else npc.velocity *= 0.95f;
					Timer++;
					if (Timer > 300)
					{
						Timer = 0;
						State = State_Idle;
					}

				}
				else if (State == State_Spin)
				{

					Player player = Main.player[npc.target];
					npc.rotation += (float)Math.PI / 10f;
					float x = player.position.X + player.width / 2 - (npc.position.X + npc.width / 2);
					float y = player.position.Y + player.height / 2 - (npc.position.Y + npc.height / 2);
					npc.velocity = new Vector2(x, y) * (5 / (float)Math.Sqrt(x * x + y * y));
					Timer++;
					if (Timer > 600)
					{
						Timer = 0;
						State = State_Idle;
						npc.rotation = 0f;
					}
				}
				else if (State == State_Transform)
				{
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					Timer++;
					if (Timer >= 40)
					{
						NPC.NewNPC((int)(npc.Center.X), (int)npc.Center.Y + 54, mod.NPCType("AncientHealingCore"), 0, 0f, 0f, 0f, 0f, 255); //Right Crystal

						NPC.NewNPC((int)(npc.Center.X + 82f), (int)npc.Center.Y + 40, mod.NPCType("AncientHealingCrystalLeft"), 0, 0f, 0f, 0f, 0f, 255); //Left Crystal
						NPC.NewNPC((int)(npc.Center.X - 82f), (int)npc.Center.Y + 40, mod.NPCType("AncientHealingCrystalRight"), 0, 0f, 0f, 0f, 0f, 255); //Right Crystal

						Vector2 leftpos = new Vector2(npc.Center.X - 40, npc.Center.Y + 15);
						Vector2 rightpos = new Vector2(npc.Center.X + 40, npc.Center.Y + 15);
						Gore.NewGore(leftpos, npc.velocity, mod.GetGoreSlot("Gores/PipeLeft"), 1f);
						Gore.NewGore(rightpos, npc.velocity, mod.GetGoreSlot("Gores/PipeRight"), 1f);

						npc.active = false;
						npc.life = 0;
					}
				}
			}
		}
	}
}