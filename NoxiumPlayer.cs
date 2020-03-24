using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace NoxiumMod
{
	public class NoxiumPlayer : ModPlayer
	{
		public bool fireMinion;
		public bool KatanaDash { get; internal set; } = false;
		public int dashTimer = 0;

		public bool SeedKeyJustPressed = false;

		public override void ResetEffects()
		{
			fireMinion = false;
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			SeedKeyJustPressed = NoxiumMod.SeedHotkey.JustPressed;
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
	}
}