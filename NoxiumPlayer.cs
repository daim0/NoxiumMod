
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.GameInput;
using System.Linq;
using Terraria.ModLoader.IO;
using ReLogic.Graphics;
using Terraria.Graphics.Effects;
using Terraria.GameContent.Achievements;


namespace NoxiumMod
{
    class NoxiumPlayer : ModPlayer
    {
        public bool fireMinion;
        public bool KatanaDash = false;
        public int dashTimer = 0;


        public override void ResetEffects()
        {
            this.fireMinion = false;
        }
        public override void PostUpdateMiscEffects()
        {

            if (KatanaDash)
            {
                for (int l = 0; l < 1; l++)
                {
                    int dust;
                    dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + (float)(player.height / 2) - 8f), player.width, 49, 31, 0f, 0f, 100, Colors.RarityPurple, 1.4f);
                    
                    Main.dust[dust].velocity *= 0.1f;
                    Main.dust[dust].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                    Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                }
                dashTimer--;
                if (dashTimer <= 0)
                {
                    KatanaDash = false;
                }
            }
        }
    }
}
