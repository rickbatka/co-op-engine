using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ReferenceMaterial.Entity
{
	class GameObject
	{
		//components here:
		public BrainBase BrainComponent;
		public PhysicsBase PhysicsComponent;
		public RenderBase RenderComponent;

		public GameObject()
		{}
		
		public void Update(GameTime gameTime)
		{
			BrainComponent.Update(gameTime);
			PhysicsComponent.Update(gameTime);
			RenderComponent.Update(gameTime);
		}

		//stats
		//rendering
		public void Draw(SpriteBatch spriteBatch)
		{
			BrainComponent.Draw(spriteBatch);
			PhysicsComponent.Draw(spriteBatch);
			RenderComponent.Draw(spriteBatch);
		}

		//logic/AI

		//physical
		
	}
}
