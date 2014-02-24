using co_op_engine.Components.Input;
using co_op_engine.Factories;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
#warning hack here for positioning camera, may want to move elsewhere
            Camera.Instance.CenterCameraOnPosition(owner.Position);
            HandleWeaponToggle();
            HandleActions();
            HandleMovement();
            SetState();
        }

        private void HandleWeaponToggle()
        {
            if(InputHandler.KeyPressed(Keys.D1))
            {
                owner.EquipWeapon(PlayerFactory.Instance.GetSword(owner));
            }
            if (InputHandler.KeyPressed(Keys.D2))
            {
                owner.EquipWeapon(PlayerFactory.Instance.GetAxe(owner));
            }
            if (InputHandler.KeyPressed(Keys.D3))
            {
                owner.EquipWeapon(PlayerFactory.Instance.GetMace(owner));
            }
        }

        private void HandleActions()
        {
            if (InputHandler.KeyPressed(Keys.T))
            {
                TowerFactory.Instance.GetDoNothingTower();
            }
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
