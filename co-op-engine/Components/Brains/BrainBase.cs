using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components.Brains
{
    public delegate void ActorChangeStateEventHandler(BrainBase sender, ActorStateChangedEventArgs data);
    public struct ActorStateChangedEventArgs
    {
        public int OldState;
        public int NewState;
    }

    public class BrainBase
    {
        protected GameObject owner;
        public event ActorChangeStateEventHandler OnActorStateChanged;

        public BrainBase(GameObject owner)
        {
            this.owner = owner;
            this.owner.CurrentActorState = Constants.STATE_IDLE;
        }

        virtual public void Update(GameTime gameTime) { }
        virtual public void Draw(SpriteBatch spriteBatch) { }

        protected void ChangeState(int newState)
        {
            if(newState != owner.CurrentActorState)
            {
                var oldState = owner.CurrentActorState;
                owner.CurrentActorState = newState;

                if (OnActorStateChanged != null)
                {
                    OnActorStateChanged(this, new ActorStateChangedEventArgs() { OldState = oldState, NewState = newState });
                }
            }
            
        }

    }
}
