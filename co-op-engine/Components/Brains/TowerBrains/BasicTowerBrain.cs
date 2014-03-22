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

		private KeyMouseTowerPlacingInput placingInput;
		private Texture2D placingZoneTexture;
		private Color placingColor = Color.Green;
		private const float placingColorOpacity = 0.5f;

		public BasicTowerBrain(GameObject owner, Texture2D placingZoneTexture, KeyMouseTowerPlacingInput placingInput, State startState = State.Placing)
			: base(owner)
		{
			this.placingInput = placingInput;
			this.placingZoneTexture = placingZoneTexture;
			this.placingColor = new Color(placingColor, placingColorOpacity);

			this.currentState = startState;

			this.placingInput.OnPlacementAttempted += HandlePlacementAttempted;
			this.placingInput.OnCoordsUpdated += HandleCoordsUpdated;
		}

		override public void Update(GameTime gameTime)
		{
			placingInput.Update(gameTime);
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

		void HandlePlacementAttempted(object sender, EventArgs args)
		{
			currentState = State.Built;
		}

		void HandleCoordsUpdated(KeyMouseTowerPlacingInput sender, CoordEventArgs coordData)
		{
			if (currentState == State.Placing)
			{
				var screenLockedCoords = coordData.Coords;
				owner.Position = screenLockedCoords;
			}
		}

	}
}
