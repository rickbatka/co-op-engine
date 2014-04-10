using System;
using co_op_engine.Components.Input;
using co_op_engine.Factories;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using co_op_engine.Components.Weapons;

namespace co_op_engine.Components.Brains
{
    enum Device { Mouse, Joystick }
    class PlayerBrain : BrainBase
    {
        private PlayerControlInput input;
        private Device currentAimingDevice = Device.Mouse;
        
        private Vector2 previousMouseVector;
        private Vector2 previousMovementVector;
        private int previousState;
        private WeaponBase previousWeapon;

        public PlayerBrain(GameObject owner, PlayerControlInput input)
            : base(owner)
        {
            this.input = input;
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        override public void BeforeUpdate()
        { 
        }

        override public void Update(GameTime gameTime)
        {
#warning hack here for positioning camera, may want to move elsewhere
            //Camera.Instance.CenterCameraOnPosition(owner.Position);
            HandleAiming();
            HandleWeaponToggle();
            HandleActions();
            HandleMovement();
            SetState();
        }

        override public void AfterUpdate()
        {
            if ((previousMovementVector != null && previousMovementVector != owner.InputMovementVector)
                || (previousState != null && previousState != owner.CurrentState)
                || previousWeapon != null && previousWeapon != owner.Weapon)
            {
                SendUpdate(new PlayerBrainUpdateParams()
                {
                    InputMovementVector = owner.InputMovementVector,
                    Position = owner.Position,
                    RotationTowardFacingDirectionRadians = owner.RotationTowardFacingDirectionRadians,
                    CurrentState = owner.CurrentState
                });
            }

            previousMouseVector = InputHandler.MousePositionVector();
            previousMovementVector = owner.InputMovementVector;
            previousState = owner.CurrentState;
            previousWeapon = owner.Weapon;
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

            if (InputHandler.KeyPressed(Keys.E))
            {
                PlayerFactory.Instance.GetEnemy();
            }

            if (InputHandler.KeyPressed(Keys.Space) || InputHandler.MouseLeftPressed())
            {
                owner.Weapon.TryInitiateAttack();
            }
        }

        [Serializable]
        public struct PlayerBrainUpdateParams
        {
            public S_Vector2 InputMovementVector;
            public S_Vector2 Position;
            public float RotationTowardFacingDirectionRadians;
            public int CurrentState;
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
            
        }

        private void SetState()
        {
            var newPlayerState = owner.CurrentState;

            if ((owner.InputMovementVector.X != 0 || owner.InputMovementVector.Y != 0)
                && owner.CurrentStateProperties.CanInitiateWalkingState)
            {
                newPlayerState = Constants.ACTOR_STATE_WALKING;
            }
            else if(owner.CurrentStateProperties.CanInitiateIdleState)
            {
                newPlayerState = Constants.ACTOR_STATE_IDLE;
            }

            if (newPlayerState != owner.CurrentState)
            {
                ChangeState(newPlayerState);
            }
        }
    }
}
