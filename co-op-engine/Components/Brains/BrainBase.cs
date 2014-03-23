using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components.Brains
{
    public class BrainBase
    {
        protected GameObject owner;

        public BrainBase(GameObject owner)
        {
            this.owner = owner;
            this.owner.CurrentState = Constants.ACTOR_STATE_IDLE;
        }

        virtual public void Update(GameTime gameTime) { }
        virtual public void Draw(SpriteBatch spriteBatch) { }

        protected void ChangeState(int newState)
        {
            if(newState != owner.CurrentState)
            {
                var oldState = owner.CurrentState;
                owner.CurrentState = newState;
            }
            
        }

    }
}
