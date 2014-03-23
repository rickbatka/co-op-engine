using co_op_engine.Components.Input;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.TowerBrains
{
	enum State
	{
		Placing,
		Building,
		Built
	}
	class BasicTowerBrain : BrainBase
	{
		private State currentState;

		private TowerPlacingInput placingInput;
		private Texture2D placingZoneTexture;
		private Color placingColor = Color.Green;
		private const float placingColorOpacity = 0.5f;

		public BasicTowerBrain(GameObject owner, Texture2D placingZoneTexture, TowerPlacingInput placingInput, State startState = State.Placing)
			: base(owner)
		{
			this.placingInput = placingInput;
			this.placingZoneTexture = placingZoneTexture;
			this.placingColor = new Color(placingColor, placingColorOpacity);

			this.currentState = startState;
		}

		override public void Update(GameTime gameTime)
		{
			HandleInput();
		}

		override public void Draw(SpriteBatch spriteBatch)
		{
			if (currentState == State.Placing)
			{
				spriteBatch.Draw(
					texture: placingZoneTexture,
					rectangle: new Rectangle((int)(owner.Position.X - owner.CurrentFrame.SourceRectangle.Width / 2), (int)(owner.Position.Y - owner.CurrentFrame.SourceRectangle.Height / 2), owner.CurrentFrame.SourceRectangle.Width, owner.CurrentFrame.SourceRectangle.Height),
					color: placingColor
				);
			}
		}

		private void HandleInput()
		{
			if (currentState == State.Placing)
			{
				owner.Position = placingInput.GetCoords();
				if (placingInput.DidPressBuildButton())
				{
					currentState = State.Built;
				}
			}
		}
	}
}
