using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NoxiumMod.Projectiles.PlasmaStuff;


namespace NoxiumMod.NPCs.Plasma
{
    public class NPCBubble : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(" ");
        }
        public override void SetDefaults()
        {
            npc.lifeMax = 1;
            npc.damage = 0;
            npc.defense = 0;
            npc.knockBackResist = 0.3f;
            npc.width = 98;
            npc.height = 98;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 0, 0);
        }

        public override void AI()
        {
            Vector2 floatypos = new Vector2((float)Math.Cos(Main.GlobalTime / 1f) * .25f, (float)Math.Sin(Main.GlobalTime / 1.37f) * .25f);
            npc.velocity = floatypos;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D fella = ModContent.GetTexture("NoxiumMod/NPCs/Plasma/fella");

            Vector2 fellaPos = new Vector2(npc.Center.X, npc.Center.Y);

            if(Vector2.Distance(fellaPos, npc.Center) >= 10)
            {
                fellaPos = -fellaPos.RotatedByRandom(MathHelper.ToRadians(30));
            }

            Vector2 floatypos = new Vector2((float)Math.Cos(Main.GlobalTime / 1f) * 14f, (float)Math.Sin(Main.GlobalTime / 1.37f) * 14f);
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, lightColor * 1f, 0f, new Vector2(Main.npcTexture[npc.type].Width, Main.npcTexture[npc.type].Height) / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(fella, fellaPos - Main.screenPosition + floatypos + npc.velocity, null, lightColor, (npc.velocity.ToRotation() + (MathHelper.Pi / 2)) + ((fellaPos + floatypos).ToRotation() + (MathHelper.Pi / 2)), new Vector2(fella.Width, fella.Height) / 2f, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, lightColor * 0.75f, 0f, new Vector2(Main.npcTexture[npc.type].Width, Main.npcTexture[npc.type].Height) / 2f, 1f, SpriteEffects.None, 0f);
            
            return false;
        }
        public override void NPCLoot()
        {
        }
        public override bool CheckDead()
        {
            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<DesertDweller>());
            for(int i = 0; i < 6; i++)
            {
                ChooseBubble();
            }
            for (int i = 0; i < 36; i++)
            {
                float angle = MathHelper.ToRadians(10 * i);
                Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                Dust dust = Dust.NewDustPerfect(npc.Center + (vector * 40), 245, vector * 8f);
                dust.noGravity = true;
                
            }
            NoxiumWorld.desertDwellerSpawned = true;
            return true;
        }
        public void ChooseBubble()
        {
            int choice = Main.rand.Next(0, 2);
            Projectile.NewProjectile(new Vector2(npc.Center.X + Main.rand.Next(-50, 50), npc.Center.Y + Main.rand.Next(-50, 50)),
                new Vector2(npc.velocity.X + Main.rand.Next(5, 10), npc.velocity.Y + Main.rand.Next(5, 10)),
                choice == 0 ? ModContent.ProjectileType<MicroBubble>() : ModContent.ProjectileType<MicroBubble2>(), 0, 0f);
            
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<NoxiumPlayer>().zonePlasma && !NoxiumWorld.desertDwellerSpawned && NPC.CountNPCS(ModContent.NPCType<NPCBubble>()) < 1)
            {
                return .3f;
            }
            else 
                return 0;
        }
    }
}
