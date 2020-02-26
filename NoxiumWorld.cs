using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;


namespace NoxiumMod
{
    class NoxiumWorld : ModWorld
    {
		public static bool oculumOreSpawn;
		public static bool downedAHM;
		
		public override void Initialize()
		{
			NoxiumWorld.oculumOreSpawn = false;
			NoxiumWorld.downedAHM = false;
		}
		
		public override TagCompound Save()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			
			if(NoxiumWorld.oculumOreSpawn)
			{
				list.Add("oculumOreSpawn");
			}
			
			if(NoxiumWorld.downedAHM)
			{
				list.Add("downedAHM");
			}
			
			TagCompound tagCompound = new TagCompound();
			tagCompound.Add("spawned", list);
			tagCompound.Add("downed", list2);
			return tagCompound;
		}
		
		public override void Load(TagCompound tag)
		{
			IList<string> list = tag.GetList<string>("spawned");
			IList<string> list2 = tag.GetList<string>("downed");
			
			NoxiumWorld.oculumOreSpawn = list.Contains("oculumOreSpawn");
			
			NoxiumWorld.downedAHM = list2.Contains("downedAHM");
		}
		
		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			
			if(loadVersion == 0)
			{
				BitsByte flag = reader.ReadByte();
				
				NoxiumWorld.oculumOreSpawn = flag[0];
				NoxiumWorld.downedAHM = flag[1];
			}
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flag = new BitsByte();
			
			flag[0] = NoxiumWorld.oculumOreSpawn;
			
			flag[1] = NoxiumWorld.downedAHM;
			
			writer.Write(flag);
		}
		
		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flag = reader.ReadByte();
			
			NoxiumWorld.oculumOreSpawn = flag[0];
			NoxiumWorld.downedAHM = flag[1];
		}
	}
}
