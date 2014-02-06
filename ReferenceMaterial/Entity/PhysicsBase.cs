using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReferenceMaterial.Entity
{
	abstract class PhysicsBase
	{
		public Vector2 Position;

        //needsreference to world
		//protected Level levelRef;

		protected Rectangle boundryBox;
		public Rectangle BoundryBox
		{
			get { return boundryBox; }
		}

		protected readonly GameObject owner;

		public PhysicsBase(GameObject owner)
		{
			this.owner = owner;
			boundryBox = new Rectangle(0, 0, 32, 32);
		}

		abstract public void Update(GameTime gameTime);
		abstract public void Draw(SpriteBatch spriteBatch);

		public void SyncBoundry()
		{
			boundryBox.X = (int)(Position.X - boundryBox.Width / 2);
			boundryBox.Y = (int)(Position.Y - boundryBox.Height / 2);
		}
	}
}
