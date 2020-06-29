using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoxiumMod.Items.Turtles;
using NoxiumMod.Systems.Turtles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace NoxiumMod.Projectiles.Turtles
{
	public class TurtleProjectile : ModProjectile
	{
		// stop TurtleProjectile from autoloading but still allow its children
		public override bool Autoload(ref string name) => GetType() != typeof(TurtleProjectile);

		public TurtleInfo Turtle;
		public int WidthMoved;
		public int HeightMoved;

		public int TileX => (int)(projectile.Center.X / 16);
		public int TileY => (int)(projectile.Center.Y / 16);

		public int Direction => (int)Turtle.Direction;

		public float MaxHitTimer => 100 / (Turtle.PickaxePower / GetTileMineResist(Main.tile[TileX, TileY])) * Turtle.PickaxeSpeed;

		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.aiStyle = -1;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			projectile.timeLeft = 100; // projectile not allowed to time out
			projectile.netUpdate = true;

			projectile.spriteDirection = (int)Turtle.Direction;

			while (!Main.tile[(int)(projectile.position.X / 16), (int)((projectile.position.Y + projectile.height + 1) / 16)].active() && projectile.ai[0] == 0)
				projectile.position.Y++;

			projectile.position.X = (int)Math.Floor(projectile.position.X);

			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;
			}

			if (projectile.ai[1] <= 0)
			{
				projectile.ai[1] = MaxHitTimer;
				Move();
			}

			projectile.ai[1]--;
		}

		public void Move()
		{
			if (HeightMoved >= Turtle.Height)
				DropTurtle();

			projectile.position.X += 16 * Direction;
			WidthMoved++;

			if (WidthMoved >= Turtle.Width)
			{
				WidthMoved = 0;
				projectile.position.Y += 16;
				projectile.position.X -= Turtle.Width * 16 * (int)Turtle.Direction;
				HeightMoved++;
			}

			if (CanMineTile(Main.tile[TileX, TileY]))
				WorldGen.KillTile(TileX, TileY);
		}

		public void DropTurtle()
		{
			int turtleType = ModContent.ItemType<SmallTurtleItem>();

			if (this is MediumTurtleProjectile)
				turtleType = ModContent.ItemType<MediumTurtleItem>();
			else if (this is LargeTurtleProjectile)
				turtleType = ModContent.ItemType<LargeTurtleItem>();

			int itemIndex = Item.NewItem(projectile.getRect(), turtleType);
			(Main.item[itemIndex].modItem as TurtleItem).TurtleInfo = Turtle;
			projectile.Kill();
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects pickaxeDirection = Direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D pickaxeTexture = Main.itemTexture[Turtle.PickaxeType];
			Vector2 pickaxeOrigin = new Vector2(Direction == -1 ? pickaxeTexture.Width : 0, pickaxeTexture.Height);
			Vector2 pickaxePosition = projectile.position - Main.screenPosition + new Vector2(0, pickaxeTexture.Height) + (Direction == -1 ? new Vector2(projectile.width, 0) : Vector2.Zero);
			spriteBatch.Draw(pickaxeTexture, pickaxePosition, null, Lighting.GetColor(TileX, TileY), projectile.ai[1] / MaxHitTimer - 0.5f, pickaxeOrigin, 1f, pickaxeDirection, 1f);
		}

		// warning: this is eldritch vanilla code that i repurposed so i can get a tile's mineresist
		// go back up, nothing to see here
		private float GetTileMineResist(Tile tile)
		{
			if (Main.tileNoFail[tile.type])
				return 0;

			if (Main.tileDungeon[tile.type] || tile.type == 25 || tile.type == 58 || tile.type == 117 || tile.type == 203)
				return 2;
			else if (tile.type == 48 || tile.type == 232 || tile.type == 226 || tile.type == 111 || tile.type == 223)
				return 4;
			else if (tile.type == 107 || tile.type == 221)
				return 2;
			else if (tile.type == 108 || tile.type == 222)
				return 3;
			else if (tile.type == 211)
				return 5;
			else
			{
				ModTile modTile = TileLoader.GetTile(tile.type);

				if (modTile != null)
					return modTile.mineResist;
			}

			return 1f;
		}

		// look away
		private bool CanMineTile(Tile tile)
		{
			if (tile.type == 211 && Turtle.PickaxePower < 200)
				return false;
			if ((tile.type == 25 || tile.type == 203) && Turtle.PickaxePower < 65)
				return false;
			else if (tile.type == 117 && Turtle.PickaxePower < 65)
				return false;
			else if (tile.type == 37 && Turtle.PickaxePower < 50)
				return false;
			else if (tile.type == 404 && Turtle.PickaxePower < 65)
				return false;
			else if ((tile.type == 22 || tile.type == 204) && TileY > Main.worldSurface && Turtle.PickaxePower < 55)
				return false;
			else if (tile.type == 56 && Turtle.PickaxePower < 65)
				return false;
			else if (tile.type == 58 && Turtle.PickaxePower < 65)
				return false;
			else if ((tile.type == 226 || tile.type == 237) && Turtle.PickaxePower < 210)
				return false;
			else if (Main.tileDungeon[tile.type] && Turtle.PickaxePower < 65)
			{
				if (TileX < Main.maxTilesX * 0.35 || TileX > Main.maxTilesX * 0.65)
				{
					return false;
				}
			}
			else if (tile.type == 107 && Turtle.PickaxePower < 100)
				return false;
			else if (tile.type == 108 && Turtle.PickaxePower < 110)
				return false;
			else if (tile.type == 111 && Turtle.PickaxePower < 150)
				return false;
			else if (tile.type == 221 && Turtle.PickaxePower < 100)
				return false;
			else if (tile.type == 222 && Turtle.PickaxePower < 110)
				return false;
			else if (tile.type == 223 && Turtle.PickaxePower < 150)
				return false;
			else
			{
				ModTile modTile = TileLoader.GetTile(tile.type);

				if (modTile != null)
					return Turtle.PickaxePower >= modTile.minPick;
			}

			return true;
		}
	}
}