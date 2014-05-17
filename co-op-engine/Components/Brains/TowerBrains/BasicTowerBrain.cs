﻿using co_op_engine.Components.Input;
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
		protected int Radius = 250;
		private RectangleFloat DrawArea;

		public BasicTowerBrain(GameObject owner, TowerPlacingInput placingInput)
			: base(owner, false)
		{
			this.placingInput = placingInput;
			DrawArea = new RectangleFloat(owner.Position.X - Radius, owner.Position.Y - Radius, Radius * 2, Radius * 2);
		}

		override public void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			HandleInput();
			DrawArea.X = Owner.Position.X - Radius;
			DrawArea.Y = Owner.Position.Y - Radius;

            if (Owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
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
			var colliders = Owner.CurrentQuad.MasterQuery(DrawArea);
			foreach (var collider in colliders)
			{
				if (collider != Owner
					&& IsWithinRadius(collider))
				{
                    if(collider.Friendly)
                    {
                        HandleFriendlyInRange(collider);
                    }
                    else
                    {
                        HandleNonFriendlyInRange(collider);
                    }
				}
			}
		}

        protected virtual void HandleFriendlyInRange(GameObject collider) { }
        protected virtual void HandleNonFriendlyInRange(GameObject collider) { }

		private bool IsWithinRadius(GameObject collider)
		{
			return (collider.Position - Owner.Position).Length() <= Radius;
		}
	}
}
