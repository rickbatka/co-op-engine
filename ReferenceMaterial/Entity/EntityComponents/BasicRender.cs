using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ReferenceMaterial.Entity.EntityComponents
{
	class BasicRender : RenderBase
	{
		public Texture2D texture;

		public BasicRender(GameObject owner, Texture2D tex)
			: base(owner)
		{
			texture = tex;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			//flat sprite, nothing special here
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, owner.PhysicsComponent.BoundryBox, Color.White);
		}
	}
}
