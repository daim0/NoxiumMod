namespace NoxiumMod.Utilities
{
	// This is mainly used to counteract the scuffed drawing Terraria/tML do for backgrounds
	public interface IOffsetable
	{
		int XOffset { get; }
		int YOffset { get; }
	}
}
