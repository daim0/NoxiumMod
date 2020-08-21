using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using NoxiumMod.Dimensions;
using System;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace NoxiumMod
{
    public class NoxiumPlayer : ModPlayer
    {
        public bool fireMinion;

        public bool KatanaDash { get; internal set; } = false;

        public int dashTimer = 0;

        public bool SeedKeyJustPressed = false;

        public bool SeedKeyDown { get; private set; }

        public int SeedStackSplit { get; set; }

        public int SeedStackCounter { get; private set; }

        public int SeedStackDelay { get; private set; }

        public int SeedSuperFastStack { get; private set; }

        public bool zonePlasma;

        public bool PotionPet = false;

        public bool SentientTetherMinion = false;

        public string[] armorglowmasks = new string[6];
        public Func<Player, int, Color>[] armorglowcolor = {delegate (Player player,int index)
        {
            return Color.White;
        },
            delegate (Player player,int index)
        {
            return Color.White;
        },
            delegate (Player player,int index)
        {
            return Color.White;
        },
            delegate (Player player,int index)
        {
            return Color.White;
        },
            delegate (Player player,int index)
        {
            return Color.White;
        },
            delegate (Player player,int index)
        {
            return Color.White;
        }
        };

        public override void ResetEffects()
        {
            fireMinion = false;
            PotionPet = false;
            SentientTetherMinion = false;
            for (int i = 0; i < armorglowmasks.Length; i += 1)
            {
                armorglowmasks[i] = null;
                armorglowcolor[i] = delegate (Player player, int index)
                {
                    return Color.White;
                };
            }   
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //Note to everyone from goodpro - I spent like a week trying to figure out why seed stacking wasen't working and then jopojelly 
            //saved my life telling me to not call the setting method in here, but in a new interface layer
            //SetSeedStackDelays();

            SeedKeyJustPressed = NoxiumMod.SeedHotkey.JustPressed;
            SeedKeyDown = NoxiumMod.SeedHotkey.Current;
        }

        internal void SetSeedStackDelays()
        {
            if (!SeedKeyDown)
                SeedStackSplit = 0;

            if (SeedStackSplit > 0)
                SeedStackSplit--;

            if (SeedStackSplit == 0)
            {
                SeedStackCounter = 0;
                SeedStackDelay = 7;
                SeedSuperFastStack = 0;
            }
            else
            {
                SeedStackCounter++;

                int num;
                switch (SeedStackDelay)
                {
                    case 6:
                        num = 25;
                        break;
                    case 5:
                        num = 20;
                        break;
                    case 4:
                        num = 15;
                        break;
                    case 3:
                        num = 10;
                        break;
                    default:
                        num = 5;
                        break;
                }

                if (SeedStackCounter >= num)
                {
                    SeedStackDelay--;

                    if (SeedStackDelay < 2)
                    {
                        SeedStackDelay = 2;
                        SeedSuperFastStack++;
                    }

                    SeedStackCounter = 0;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (KatanaDash)
            {
                Vector2 position = new Vector2(player.position.X, player.position.Y + (player.height / 2) - 8f);
                int dust = Dust.NewDust(position, player.width, 49, 31, 0f, 0f, 100, Colors.RarityPurple, 1.4f); //Use DustID instead of 31

                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
                Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);

                dashTimer--;

                if (dashTimer <= 0)
                    KatanaDash = false;
            }
        }

        public override void UpdateBiomes()
        {
            zonePlasma = NoxiumWorld.plasmaSandTiles > 50 || Subworld.IsActive<PlasmaDesert>();
        }

        public override bool CustomBiomesMatch(Player other)
        {
            NoxiumPlayer modOther = other.GetModPlayer<NoxiumPlayer>();
            return zonePlasma == modOther.zonePlasma;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            NoxiumPlayer modOther = other.GetModPlayer<NoxiumPlayer>();
            modOther.zonePlasma = zonePlasma;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = zonePlasma;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            zonePlasma = flags[0];
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (zonePlasma)
            {
                return mod.GetTexture("PlasmaDesertMapBackground");
            }
            return null;
        }

        // Armor glowmasks system from SGAMod, by IDGCaptainRussia
        public static readonly PlayerLayer HeadGlowmask = new PlayerLayer("NoxiumMod", "HeadGlowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NoxiumMod mod = ModContent.GetInstance<NoxiumMod>();
            NoxiumPlayer modply = drawPlayer.GetModPlayer<NoxiumPlayer>();
            Color GlowColor = modply.armorglowcolor[0](drawPlayer, 0);

            Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

            if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
                color = drawInfo.bodyColor * drawInfo.bodyColor.A;

            if (modply.armorglowmasks[0] != null && !drawPlayer.mount.Active)
            {
                Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[0]);

                int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
                int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                data.shader = (int)drawPlayer.dye[0].dye;
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer ChestplateGlowmask = new PlayerLayer("NoxiumMod", "ChestplateGlowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NoxiumMod mod = ModContent.GetInstance<NoxiumMod>();
            NoxiumPlayer modply = drawPlayer.GetModPlayer<NoxiumPlayer>();
            Color GlowColor = modply.armorglowcolor[1](drawPlayer, 1);

            Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

            if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
                color = drawInfo.bodyColor * drawInfo.bodyColor.A;

            if (modply.armorglowmasks[1] != null && !drawPlayer.mount.Active)
            {
                Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[1]);

                int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
                int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                data.shader = (int)drawPlayer.dye[1].dye;
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer ArmsGlowmask = new PlayerLayer("NoxiumMod", "ArmsGlowmask", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NoxiumMod mod = ModContent.GetInstance<NoxiumMod>();
            NoxiumPlayer modply = drawPlayer.GetModPlayer<NoxiumPlayer>();
            Color GlowColor = modply.armorglowcolor[2](drawPlayer, 2);

            //better version, from Qwerty's Mod
            Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

            if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
                color = drawInfo.bodyColor * drawInfo.bodyColor.A;

            if (modply.armorglowmasks[2] != null && !drawPlayer.mount.Active)
            {
                Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[2]);
                int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
                int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                data.shader = (int)drawPlayer.dye[1].dye;
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer LegsGlowmask = new PlayerLayer("NoxiumMod", "LegsGlowmask", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NoxiumMod mod = ModContent.GetInstance<NoxiumMod>();
            NoxiumPlayer modply = drawPlayer.GetModPlayer<NoxiumPlayer>();
            Color GlowColor = modply.armorglowcolor[3](drawPlayer, 3);

            Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

            if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
                color = drawInfo.bodyColor * drawInfo.bodyColor.A;

            if (modply.armorglowmasks[3] != null && !drawPlayer.mount.Active)
            {
                Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[3]);

                int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
                int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.legFrame.Y, drawPlayer.legFrame.Width, drawPlayer.legFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                data.shader = (int)drawPlayer.dye[2].dye;
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer ShoeGlowmask = new PlayerLayer("NoxiumMod", "ShoeGlowmask", PlayerLayer.ShoeAcc, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NoxiumMod mod = ModContent.GetInstance<NoxiumMod>();
            NoxiumPlayer modply = drawPlayer.GetModPlayer<NoxiumPlayer>();
            Color GlowColor = modply.armorglowcolor[4](drawPlayer, 4);

            Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

            if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
                color = drawInfo.bodyColor * drawInfo.bodyColor.A;

            if (modply.armorglowmasks[4] != null && !drawPlayer.mount.Active)
            {
                Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[4]);

                int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
                int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.legFrame.Y, drawPlayer.legFrame.Width, drawPlayer.legFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                Main.playerDrawData.Add(data);
            }
        });
        public static readonly PlayerLayer WingGlowmask = new PlayerLayer("NoxiumMod", "WingGlowmask", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NoxiumMod mod = ModContent.GetInstance<NoxiumMod>();
            NoxiumPlayer modply = drawPlayer.GetModPlayer<NoxiumPlayer>();
            Color GlowColor = modply.armorglowcolor[5](drawPlayer, 5);

            Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

            if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
                color = drawInfo.bodyColor * drawInfo.bodyColor.A;

            if (modply.armorglowmasks[5] != null && !drawPlayer.mount.Active)
            {
                Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[5]);

                int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + (drawPlayer.direction == -1 ? 20 : 0)) - Main.screenPosition.X);
                int drawY = (int)(((drawPlayer.bodyPosition.Y + 95) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.wingFrame * texture.Height / 4, texture.Width, texture.Height / 4), color, (float)drawPlayer.fullRotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
                data.shader = (int)drawPlayer.cWings;
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            NoxiumPlayer noxiumP = player.GetModPlayer<NoxiumPlayer>();

            string[] stringsz = { "Head", "Body", "Arms", "Legs", "ShoeAcc", "Wings" };
            PlayerLayer[] thelayer = { HeadGlowmask, ChestplateGlowmask, ArmsGlowmask, LegsGlowmask, ShoeGlowmask, WingGlowmask };

            for (int i = 0; i < armorglowmasks.Length; i += 1)
            {
                if (noxiumP.armorglowmasks[i] != null)
                {
                    int layer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals(stringsz[i])) + 1;
                    thelayer[i].visible = true;
                    layers.Insert(layer, thelayer[i]);
                }

            }
        }
    }
}