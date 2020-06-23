using System;
using Terraria.ModLoader.IO;

namespace NoxiumMod.Systems
{
	public struct TurtleInfo : TagSerializable
	{
        public ushort Width;
        public ushort Height;
        public byte Direction;
        public int PickaxeType;
        public int PickaxePower;
        public int PickaxeSpeed;

        public static readonly Func<TagCompound, TurtleInfo> DESERIALIZER = Load;

        public TurtleInfo(ushort width, ushort height, byte direction, int pickaxeType, int pickaxePower, int pickaxeSpeed)
		{
            Width = width;
            Height = height;
            Direction = direction;
            PickaxeType = pickaxeType;
            PickaxePower = pickaxePower;
            PickaxeSpeed = pickaxeSpeed;
		}

		public TagCompound SerializeData()
		{
            return new TagCompound()
            {
                { "Width", Width },
                { "Height", Height },
                { "Direction", Direction },
                { "PickaxeType", PickaxeType },
                { "PickaxePower", PickaxePower },
                { "PickaxeSpeed", PickaxeSpeed }
            };
        }

        public static TurtleInfo Load(TagCompound tag)
		{
            return new TurtleInfo(
                tag.Get<ushort>("Width"),
                tag.Get<ushort>("Height"),
                tag.GetByte("Direction"),
                tag.GetInt("PickaxeType"),
                tag.GetInt("PickaxePower"),
                tag.GetInt("PickaxeSpeed")
                );
        }
	}
}