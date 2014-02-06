using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ReferenceMaterial.Entity.EntityComponents
{
	class KeyboardBrain : BrainBase
	{
		float speed;

		Vector2 lastMoveDirection;

		public KeyboardBrain(GameObject owner, float speed)
			: base(owner)
		{
			this.speed = speed;
		}

		public override void Update(GameTime gameTime)
		{
			/*MoveDirection = Vector2.Zero;
			if (InputHandler.KeyDown(Keys.S))
				MoveDirection.Y += speed;
			if (InputHandler.KeyDown(Keys.W))
				MoveDirection.Y -= speed;
			if (InputHandler.KeyDown(Keys.A))
				MoveDirection.X -= speed;
			if (InputHandler.KeyDown(Keys.D))
				MoveDirection.X += speed;

			if (MoveDirection != Vector2.Zero && lastMoveDirection == Vector2.Zero)
			{
				FireAttemptMoveEvent();
			}
			else if (MoveDirection == Vector2.Zero && lastMoveDirection != Vector2.Zero)
			{
				FireStopAttemptingToMoveEvent();
			}
			lastMoveDirection = MoveDirection;*/
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			//debug render info
		}
	}
}
