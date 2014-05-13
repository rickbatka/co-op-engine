using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using co_op_engine.Networking.Commands;

namespace co_op_engine.Components.Brains
{
    public class BrainBase
    {
        protected GameObject Owner;
        protected Pather Pather;

        public BrainBase(GameObject owner, bool usePathing = true)
        {
            this.Owner = owner;
            this.Owner.CurrentState = Constants.ACTOR_STATE_IDLE;
            if(usePathing)
            {
                this.Pather = new Pather(this, this.Owner);
            }
        }
        virtual public void BeforeUpdate() { }
        virtual public void Update(GameTime gameTime) 
        {
            if(Pather != null)
            {
                Pather.Update(gameTime);
            }
        }

        virtual public void AfterUpdate() { }
        virtual public void Draw(SpriteBatch spriteBatch) 
        { 
            if(Pather != null)
            {
                Pather.Draw(spriteBatch);
            }
        }

        protected void ChangeState(int newState)
        {
            if (newState != Owner.CurrentState)
            {
                var oldState = Owner.CurrentState;
                Owner.CurrentState = newState;
            }

        }

        virtual public void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
        }

        public void SendUpdate(object parameters)
        {
            NetCommander.SendCommand(new GameObjectCommand()
            {
                ID = Owner.ID,
                CommandType = GameObjectCommandType.Update,
                ReceivingComponent = GameObjectComponentType.Brain,
                Parameters = parameters,
            });
        }
    }
}
