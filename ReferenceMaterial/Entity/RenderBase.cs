using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReferenceMaterial.Entity
{
	abstract class RenderBase
	{
		protected readonly GameObject owner;

		public RenderBase(GameObject owner)
		{
			this.owner = owner;
		}

		abstract public void Update(GameTime gameTime);
		abstract public void Draw(SpriteBatch spriteBatch);
	}
}
