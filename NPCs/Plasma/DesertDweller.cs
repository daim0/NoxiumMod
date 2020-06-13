using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NoxiumMod.NPCs.Plasma
{
        [AutoloadHead]
    class DesertDweller : ModNPC
    {
            public override void SetStaticDefaults()
            {
                Main.npcFrameCount[npc.type] = 26;
                NPCID.Sets.ExtraFramesCount[npc.type] = 9;
                NPCID.Sets.AttackFrameCount[npc.type] = 5;
                NPCID.Sets.DangerDetectRange[npc.type] = 700;
                NPCID.Sets.AttackType[npc.type] = 0;
                NPCID.Sets.AttackTime[npc.type] = 90;
                NPCID.Sets.AttackAverageChance[npc.type] = 30;
            }
            public override void SetDefaults()
            {
                npc.townNPC = true;
                npc.friendly = true;
                npc.width = 18;
                npc.height = 40;
                npc.aiStyle = 7;
                npc.damage = 10;
                npc.defense = 15;
                npc.lifeMax = 250;
                npc.HitSound = SoundID.NPCHit1;
                npc.DeathSound = SoundID.NPCDeath1;
                npc.knockBackResist = 0.5f;
                animationType = NPCID.Guide;
                
            }
        public override void AI()
        {
            
        }
    }
}
