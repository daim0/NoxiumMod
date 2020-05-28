using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NoxiumMod.NPCs
{
	public class NoxiumGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public override void NPCLoot(NPC npc)
		{
			if (npc.type == NPCID.EyeofCthulhu)
			{
				if (!NoxiumWorld.oculumOreSpawn)
				{
					if (Main.netMode == NetmodeID.Server)
					{
						NetworkText networkText = NetworkText.FromKey("You sense visionary essence from below", new object[0]);
						NetMessage.BroadcastChatMessage(networkText, new Color(70, 140, 80), -1);
						NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
					}
					Main.NewText("You sense visionary essence from below", 70, 140, 80, false);

					for (int j = 0; j < (int)(Main.rockLayer * (double)Main.maxTilesY * 0.0015); j++) //the 0.0015 number basically changes how much of the ore spawns
					{
						int num3 = Main.rand.Next(0, Main.maxTilesX);
						int num4 = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY - 200);
						WorldGen.OreRunner(num3, num4, (double)Main.rand.Next(5, 6), Main.rand.Next(7, 8), (ushort)mod.TileType("OculumOre"));
					}
				}
				NoxiumWorld.oculumOreSpawn = true;
			}
		}
	}
}