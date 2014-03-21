using co_op_engine.Components.Input;
using co_op_engine.Factories;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace co_op_engine.Components.Brains
{
    enum Device { Mouse, Joystick }
    class PlayerBrain : BrainBase
    {
        private PlayerControlInput input;

        private Vector2 previousMouseVector;
        private Device currentAimingDevice = Device.Mouse;

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
            HandleAiming();
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

            if (InputHandler.KeyPressed(Keys.Space) || InputHandler.MouseLeftPressed())
            {
                owner.Weapon.TryInitiateAttack();
            }
        }

        private void HandleMovement()
        {
            owner.InputMovementVector = input.GetMovement();
        }

        private void HandleAiming()
        {
            var currentMouseVector = InputHandler.MousePositionVectorCameraAdjusted();
            if(previousMouseVector == null)
            {
                previousMouseVector = currentMouseVector;
            }
            
            if(currentAimingDevice == Device.Mouse)
            {
                owner.FacingDirectionRaw = InputHandler.MousePositionVectorCameraAdjusted() - owner.Position;
                owner.FacingDirectionRaw.Normalize();
            }
            else if(currentAimingDevice == Device.Joystick)
            {
            }

            owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(owner.FacingDirectionRaw);
            previousMouseVector = InputHandler.MousePositionVector();
        }

        private void SetState()
        {
            var newPlayerState = owner.CurrentActorState;

            if ((owner.InputMovementVector.X != 0 || owner.InputMovementVector.Y != 0)
                && owner.CurrentStateProperties.CanInitiateWalkingState)
            {
                newPlayerState = Constants.ACTOR_STATE_WALKING;
            }
            else if(owner.CurrentStateProperties.CanInitiateIdleState)
            {
                newPlayerState = Constants.ACTOR_STATE_IDLE;
            }

            if (newPlayerState != owner.CurrentActorState)
            {
                ChangeState(newPlayerState);
            }
        }
    }
}
