using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Engines
{
    public class WalkerEngine : EngineBase
    {
        public WalkerEngine(GameObject owner)
            :base(owner)
        {
            this.Owner.CurrentState = Constants.ACTOR_STATE_IDLE;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SetState();
        }

        public override void DebugDraw(SpriteBatch spriteBatch)
        {
            base.DebugDraw(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected void ChangeState(int newState)
        {
            if (newState != Owner.CurrentState)
            {
                var oldState = Owner.CurrentState;
                Owner.CurrentState = newState;
            }
        }

        private void SetState()
        {
            var newPlayerState = Owner.CurrentState;

            if ((Owner.InputMovementVector.X != 0 || Owner.InputMovementVector.Y != 0)
                && Owner.CurrentStateProperties.CanInitiateWalkingState)
            {
                newPlayerState = Constants.ACTOR_STATE_WALKING;
            }
            else if (Owner.CurrentStateProperties.CanInitiateIdleState)
            {
                newPlayerState = Constants.ACTOR_STATE_IDLE;
            }

            if (newPlayerState != Owner.CurrentState)
            {
                ChangeState(newPlayerState);
            }
        }
    }
}
