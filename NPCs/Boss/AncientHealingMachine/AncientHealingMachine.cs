using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


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
            npc.lifeMax = 2500;
            npc.damage = 30;
            npc.defense = 12;
            npc.knockBackResist = 0f;
            npc.width = 234;
            npc.height = 120;
            npc.value = Item.buyPrice(0, 20, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.buffImmune[24] = true;
            music = MusicID.Boss2;
        }

        private const int State_Idle = 0;
        private const int State_LaserShot = 1;
        private const int State_Dash = 2;
        private const int State_Spin = 3;
		
		private const int State_Transform = 5;
		
		private int TimerShoot = 120;

        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        private float transformHP = 0; //TODO why is this never assigned to?

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
        private int boolTimer;
		
        public void Move()
        {
                Player player = Main.player[npc.target];
                float x = player.position.X + player.width / 2 - (npc.position.X + npc.width / 2);
                float y = player.position.Y + player.height / 2 - (npc.position.Y + npc.height / 2);
                npc.velocity = new Vector2(x, y) * (4.52069f / (float)Math.Sqrt(x * x + y * y));
        }
        public override void AI()
        {
            boolTimer++;
            if (boolTimer == 1)
                NoxiumWorld.ahmSpawned = true;
            if(boolTimer >= 2)
            {
                boolTimer = 2;
            }
            npc.TargetClosest(true);
            if (npc.HasValidTarget)
            {
                if (State == State_Idle)
                {
                    Move();
					if (transformHP == 0 && npc.life <= npc.lifeMax * 0.5f)
					{
						State = State_Transform;
						Timer = 0;
					}
					
                    npc.velocity *= 0.065f;
                    npc.velocity.Y += (float)Math.Sin(Math.PI * (Timer / 18));

                    Timer++;
                    if (Timer >= 60)
                    {
                        Timer = 0;
                        int choice = Main.rand.Next(1,4);
                        if (choice == 1)
                        {
                            State = State_LaserShot;
                        }
                        if (choice == 2)
                        {
                            State = State_Dash;
                        }
                        if (choice == 3)
                        {
                            State = State_Spin;
                        }
                    }

                }
                else if (State == State_LaserShot)
                {
                    if (transformHP == 0 && npc.life <= npc.lifeMax * 0.5f)
                    {
                        State = State_Transform;
                        Timer = 0;
                    }

                    npc.velocity *= 0.055f;
                    npc.velocity.Y += (float)Math.Sin(Math.PI * (Timer / 22));
					TimerShoot--;
					if (TimerShoot <= 30)
					{
						//Player player = Main.player[npc.target];
						npc.TargetClosest(false);
						Vector2 vector = new Vector2(npc.position.X - 75 + (npc.width / 2), npc.position.Y + 32 + (npc.height / 2));
						Vector2 vectorSuEma = new Vector2(npc.position.X + 75 + (npc.width / 2), npc.position.Y + 32 + (npc.height / 2));
						Projectile.NewProjectile(vector.X, vector.Y, 0f, 0f, mod.ProjectileType("AncientLaser"), 50, 0f, Main.myPlayer, 5.25f / 425f, npc.whoAmI);
						Projectile.NewProjectile(vectorSuEma.X, vectorSuEma.Y, 0f, 0f, mod.ProjectileType("AncientLaser2"), 50, 0f, Main.myPlayer, -5.25f / 425f, npc.whoAmI);
						TimerShoot = 4000;
					}
					
                    Timer++;
                    if(Timer > 300)
                    {
                        Timer = 0;
						TimerShoot = 120;
                        State = State_Idle;
                    }
                }
                else if (State == State_Dash)
                {
                    if (transformHP == 0 && npc.life <= npc.lifeMax * 0.5f)
                    {
                        State = State_Transform;
                        Timer = 0;
                    }

                    TimerShoot--;
					if(TimerShoot <= 50)
					{
						
						npc.velocity = npc.velocity * 0.45f;
						Vector2 vector = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float num = (float)Math.Atan2(vector.Y - (Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f), vector.X - (Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f));
						npc.velocity.X = (float)(Math.Cos(num) * 20.0) * -1f;
						npc.velocity.Y = (float)(Math.Sin(num) * 20.0) * -1f;
						new Vector2((float)Math.Cos(npc.ai[0]), (float)Math.Sin(npc.ai[0]));
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 20, 1f, 0f);
						new Rectangle((int)npc.position.X, (int)(npc.position.Y + (npc.height - npc.width) / 2), npc.width, npc.width);
						
						int num2 = 25;
						for (int i = 1; i <= num2; i++)
						{
							int num3 = Dust.NewDust(npc.position, npc.width, npc.height, 219, 0f, 0f, 0, default, 1f);
							Main.dust[num3].noGravity = false;
							Main.dust[num3].scale = 0.8f;
							if (Main.rand.Next(1) == 0)
							{
								int num9 = Dust.NewDust(npc.position, npc.width, npc.height, 74, 0f, 0f, 0, default, 1f);
								Main.dust[num9].noGravity = true;
								Main.dust[num9].scale = 0.95f;
							}
						}
						TimerShoot = 90;
					}
					else npc.velocity *= 0.95f;
				
                    Timer++;
                    if(Timer > 300)
                    {
                        Timer = 0;
						TimerShoot = 120;
                        State = State_Idle;
                    }

                }
                else if (State == State_Spin)
                {
                    if (transformHP == 0 && npc.life <= npc.lifeMax * 0.5f)
                    {
                        State = State_Transform;
                        Timer = 0;
                    }

                    Player player = Main.player[npc.target];
                    npc.rotation += (float)Math.PI / 10f;
                    float x = player.position.X + player.width / 2 - (npc.position.X + npc.width / 2);
                    float y = player.position.Y + player.height / 2 - (npc.position.Y + npc.height / 2);
                    npc.velocity = new Vector2(x, y) * (5 / (float)Math.Sqrt(x * x + y * y));
					
					TimerShoot--;
					if(TimerShoot <= 10)
					{
						double num7 = Math.Atan2(npc.velocity.X, npc.velocity.Y) - 0.783f / 2f;
						double num8 = 0.783f / 8f;
						for (int j = 0; j < 3; j++)
						{
							Vector2 vector = new Vector2(npc.Center.X, npc.Center.Y);
							double num9 = num7 + num8 * (j + j * j) / 2.0 + 32f * j;
							Projectile.NewProjectile(vector.X, vector.Y, (float)(Math.Sin(num9) * 5.0), (float)(Math.Cos(num9) * 5.0), ProjectileID.NebulaLaser, npc.damage / 3, 1.4f, player.whoAmI, 0f, 0f);
							Projectile.NewProjectile(vector.X, vector.Y, (float)(-(float)Math.Sin(num9) * 5.0), (float)(-(float)Math.Cos(num9) * 5.0), ProjectileID.NebulaLaser, npc.damage / 3, 1.4f, player.whoAmI, 0f, 0f);
						}
						TimerShoot = 70;
					}
                    Timer++;
                    if (Timer > 350)
                    {
                        Timer = 0;
						TimerShoot = 120;
                        State = State_Idle;
                        npc.rotation = 0f;
                    }
                }
				else if (State == State_Transform)
				{
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					Timer++;
					if(Timer >= 40)
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
			else
			{
				npc.rotation = 0f;
				npc.velocity = new Vector2(0, -14f);
				if (npc.timeLeft > 69)
				{
					npc.timeLeft = 69;
				}
			}
        }
    }
}