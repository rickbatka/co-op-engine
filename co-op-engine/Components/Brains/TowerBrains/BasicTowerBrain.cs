using co_op_engine.Components.Input;
using co_op_engine.Components.Particles;
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
		private RadiusProximityChecker RadiusChecker;

		public BasicTowerBrain(GameObject owner, TowerPlacingInput placingInput)
			: base(owner, false)
		{
			this.placingInput = placingInput;
			RadiusChecker = new RadiusProximityChecker(owner);
		}

		override public void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			HandleInput();

			if (Owner.CurrentStateProperties.CanInitiateSkills)
			{
				QueryRange();
			}
		}

		override public void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		private void HandleInput()
		{
			if (Owner.CurrentState == Constants.ACTOR_STATE_PLACING)
			{
				Owner.Position = placingInput.GetCoords();
				if (placingInput.DidPressBuildButton())
				{
					Owner.CurrentState = Constants.ACTOR_STATE_IDLE;
				}
			}
		}

		private void QueryRange()
		{
			var colliders = RadiusChecker.QueryRange();
			foreach (var collider in colliders)
			{
				if(collider.Team == Owner.Team)
				{
					HandleFriendlyInRange(collider);
				}
				else
				{
					HandleNonFriendlyInRange(collider);
				}
			}
		}

		protected virtual void HandleFriendlyInRange(GameObject collider) { }
		protected virtual void HandleNonFriendlyInRange(GameObject collider) { }

	}
}
