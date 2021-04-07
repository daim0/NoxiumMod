namespace NoxiumMod.Utilities
{
	// This is mainly used to counteract the scuffed drawing Terraria/tML do for backgrounds on subworlds with peculiar heights
	public interface IOffsetableBg
	{
		int CloseXOffset { get; }
		
		int CloseYOffset { get; }

		int MiddleXOffset { get; }

		int MiddleYOffset { get; }

		int FarXOffset { get; }

		int FarYOffset { get; }
	}
}
