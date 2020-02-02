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
		
		public override void Initialize()
		{
			NoxiumWorld.oculumOreSpawn = false;
		}
		
		public override TagCompound Save()
		{
			List<string> list = new List<string>();
			
			if(NoxiumWorld.oculumOreSpawn)
			{
				list.Add("oculumOreSpawn");
			}
			
			TagCompound tagCompound = new TagCompound();
			tagCompound.Add("spawned", list);
			return tagCompound;
		}
		
		public override void Load(TagCompound tag)
		{
			IList<string> list = tag.GetList<string>("spawned");
			
			NoxiumWorld.oculumOreSpawn = list.Contains("oculumOreSpawn");
		}
		
		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			
			if(loadVersion == 0)
			{
				BitsByte flag = reader.ReadByte();
				
				NoxiumWorld.oculumOreSpawn = flag[0];
			}
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flag = new BitsByte();
			
			flag[0] = NoxiumWorld.oculumOreSpawn;
			
			writer.Write(flag);
		}
		
		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flag = reader.ReadByte();
			
			NoxiumWorld.oculumOreSpawn = flag[0];
		}
}
