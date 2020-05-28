using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.NPCs.Boss.AncientHealingMachine
{
    class AncientHealingCrystalLeft : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Healing Crystal");
            Main.npcFrameCount[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 800;
            npc.damage = 20;
            npc.defense = 12;
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 80;
            npc.value = Item.buyPrice(0, 4, 0, 0);
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[24] = true;
			bossBag = ItemType<AncientHealingBossBag>();
			npc.boss = true;
			music = MusicID.Boss2;
        }

        private const int State_Idle = 0;
        private const int State_Circle = 1;

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
		
		private float TimerBangBang = 120;
		private float TimerBang = 290;
		
		public override void NPCLoot()
		{
			if (!NPC.AnyNPCs(mod.NPCType("AncientHealingCrystalRight")))
			{
				if(Main.expertMode)
					npc.DropBossBags();
				
				if (!NoxiumWorld.downedAHM) 
				{
					NoxiumWorld.downedAHM = true;

					if (Main.netMode == NetmodeID.Server) 
						NetMessage.SendData(MessageID.WorldData); // Tells code stuff that the boss has been killed in this world
				}
			}
		}

        public override void AI()
        {
			Player player = Main.player[npc.target];
            npc.TargetClosest(true);
			
			
            if (npc.HasValidTarget)
            {
                if (State == State_Idle)
                {		
					if (npc.position.Y > player.position.Y - 220f)
					{
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y = npc.velocity.Y * 0.85f;
						}
						npc.velocity.Y = npc.velocity.Y - 0.1f;
						if (npc.velocity.Y > 2f)
						{
							npc.velocity.Y = 2f;
						}
					}
					else if (npc.position.Y < player.position.Y - 255f)
					{
						if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y = npc.velocity.Y * 0.85f;
						}
						npc.velocity.Y = npc.velocity.Y + 0.1f;
						if (npc.velocity.Y < -2f)
						{
							npc.velocity.Y = -2f;
						}
					}
					if (npc.position.X + npc.width / 2 > player.position.X + player.width / 2 + 145f)
					{
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X = npc.velocity.X * 0.35f;
						}
						npc.velocity.X = npc.velocity.X - 0.1f;
						if (npc.velocity.X > 2f)
						{
							npc.velocity.X = 2f;
						}
					}
					if (npc.position.X + npc.width / 2 < player.position.X + player.width / 2 + 60f)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X = npc.velocity.X * 0.35f;
						}
						npc.velocity.X = npc.velocity.X + 0.1f;
						if (npc.velocity.X < -2f)
						{
							npc.velocity.X = -2f;
						}
					}
					TimerBang--;
					if(TimerBang <= 10)
					{
						Projectile.NewProjectile(npc.Center, new Vector2(3.5f), mod.ProjectileType("AncientHomingCrystal"), 50, 2.5f, 255, 0f, 0f);
						TimerBang = 290;
					}
					
					TimerBangBang--;
					if (TimerBangBang <= 30)
					{
						float Speed = 4.5f;
						Vector2 vector = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + 32 + (npc.height / 2));
						int damage = 15; //nice
						int type = 576; //Projectile goes here boomer
						Main.PlaySound(SoundID.Item8); //Idk what kind of sound effect should there be tbh
						float rotation = (float)Math.Atan2(vector.Y - (player.position.Y + (player.height * 0.5f)), vector.X - (player.position.X + (player.width * 0.5f)));
						Projectile.NewProjectile(vector.X, vector.Y, (float)((Math.Cos(rotation) * Speed) * -2), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
						
						TimerBangBang = 80;
					}
					
                    Timer++;
                    if (Timer >= 460)
                    {
                        Timer = 0;
                        State = State_Circle;
						TimerBangBang = 120;
						TimerBang = 290;
                    }

                }
                else if (State == State_Circle)
                {		
					Timer++;
					if (Timer <= 200)
					{
						double distance = 250; //how far the npc circle is from the player
						double degree = (double)npc.ai[1] + (360 * npc.ai[0]);
						double radius = degree * (Math.PI / 180);
						
						npc.position.X = player.Center.X - (int)(Math.Cos(radius) * distance) - npc.width / 2;
						npc.position.Y = player.Center.Y - (int)(Math.Sin(radius) * distance) - npc.height / 2;
						npc.ai[1] += 4f; // How fast it circles the player
					}
					if (Timer >= 200)
					{
						
						float x = player.position.X + player.width / 2 - (npc.position.X + npc.width / 2);
						float y = player.position.Y + player.height / 2 - (npc.position.Y + npc.height / 2);
						npc.velocity = new Vector2(x, y) * (14 / (float)Math.Sqrt(x * x + y * y));

						
					}
					if (Timer >= 210)
					{
						Timer = 0;
						State = State_Idle;
					}
                }
            }
			else
			{
				npc.velocity = new Vector2(8f, -12f);
				if (npc.timeLeft > 80)
				{
					npc.timeLeft = 80;
				}
			}
        }
    }
}