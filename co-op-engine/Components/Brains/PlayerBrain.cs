using co_op_engine.Components.Input;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Brains
{
    class PlayerBrain : BrainBase
    {
        private PlayerControlInput input;

        public PlayerBrain(GameObject owner, PlayerControlInput input)
            : base(owner)
        {
            this.input = input;
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        override public void Update(GameTime gameTime)
        {
            HandleActions();
            HandleMovement();
            SetState();
        }

        private void HandleActions()
        {

        }

        private void HandleMovement()
        {
            owner.InputMovementVector = input.GetMovement();
        }

        private void SetState()
        {
            var newPlayerState = owner.currentActorState;

            if (owner.InputMovementVector.X != 0 || owner.InputMovementVector.Y != 0)
            {
                newPlayerState = ActorState.Walking;
            }
            else 
            {
                newPlayerState = ActorState.Idle;
            }

            if (newPlayerState != owner.currentActorState)
            {
                ChangeState(newPlayerState);
            }
        }

    }
}
