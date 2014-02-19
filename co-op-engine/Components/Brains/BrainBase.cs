using co_op_engine.Events;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components.Brains
{
    
    
    public class BrainBase
    {
        protected GameObject owner;
        public event EventHandler OnActorStateChanged;

        public BrainBase(GameObject owner)
        {
            this.owner = owner;
            this.owner.currentActorState = ActorState.Idle;
        }

        virtual public void Update(GameTime gameTime) { }
        virtual public void Draw(SpriteBatch spriteBatch) { }

        protected void ChangeState(ActorState newState)
        {
            if(newState != owner.currentActorState)
            {
                var oldState = owner.currentActorState;
                owner.currentActorState = newState;

                if (OnActorStateChanged != null)
                {
                    OnActorStateChanged(this, new ActorStateChangedEventArgs() { OldState = oldState, NewState = newState });
                }
            }
            
        }

    }
}
