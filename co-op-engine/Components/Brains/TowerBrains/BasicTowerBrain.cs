﻿using co_op_engine.Components.Input;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.TowerBrains
{
	public class BasicTowerBrain : BrainBase
	{
		private TowerPlacingInput placingInput;

		public BasicTowerBrain(GameObject owner, TowerPlacingInput placingInput)
			: base(owner)
		{
			this.placingInput = placingInput;
		}

		override public void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			HandleInput();
		}

		override public void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		private void HandleInput()
		{
			if (owner.CurrentState == Constants.ACTOR_STATE_PLACING)
			{
				owner.Position = placingInput.GetCoords();
				if (placingInput.DidPressBuildButton())
				{
					owner.CurrentState = Constants.ACTOR_STATE_IDLE;
				}
			}
		}
	}
}
