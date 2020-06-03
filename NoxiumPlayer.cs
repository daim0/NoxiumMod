using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.IO;

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
        public override void ResetEffects()
        {
            fireMinion = false;
            PotionPet = false;
            SentientTetherMinion = false;
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
            zonePlasma = NoxiumWorld.plasmaSandTiles > 50;
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
    }
}