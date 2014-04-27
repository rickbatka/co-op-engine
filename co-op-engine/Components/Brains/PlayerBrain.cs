using System;
using co_op_engine.Components.Input;
using co_op_engine.Factories;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using co_op_engine.Components.Weapons;
using co_op_engine.Utility.Camera;

namespace co_op_engine.Components.Brains
{
    
    class PlayerBrain : BrainBase
    {
        private PlayerControlInput input;


        private Vector2 previousMovementVector;
        private int previousState;
        private WeaponBase previousWeapon;
        private Vector2 boostStartMovementVector;

        public PlayerBrain(GameObject owner, PlayerControlInput input)
            : base(owner)
        {
            this.input = input;
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        override public void BeforeUpdate()
        {
            input.BeforeUpdate();
        }

        override public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            HandleAiming();
            HandleWeaponToggle();
            HandleActions();
            HandleMovement();
            SetState();
        }

        override public void AfterUpdate()
        {
            input.AfterUpdate();
            if ((previousMovementVector != null && previousMovementVector != owner.InputMovementVector)
                || (previousState != owner.CurrentState)
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

            previousMovementVector = owner.InputMovementVector;
            previousState = owner.CurrentState;
            previousWeapon = owner.Weapon;
        }

        private void HandleWeaponToggle()
        {
            if (InputHandler.KeyPressed(Keys.D1))
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
            if (input.IsPressingAttackButton())
            {
                owner.Weapon.TryInitiateAttack();
            }

            /*
             * TODO
             * Pretty much everything below here is for debugging / testing.
             * */
            if (InputHandler.KeyPressed(Keys.T))
            {
                TowerFactory.Instance.GetDoNothingTower();
            }

            if (InputHandler.KeyPressed(Keys.E))
            {
                PlayerFactory.Instance.GetEnemy();
            }

            if (InputHandler.KeyPressed(Keys.C))
            {
                Camera.Instance.Shake();
            }

            if (InputHandler.KeyPressed(Keys.L)) // L is for teleport, duh
            {
                owner.Position = InputHandler.MousePositionVectorCameraAdjusted();
            }

            if (InputHandler.KeyPressed(Keys.B))
            {
                SetBoosting();
            }
        }

        private void SetBoosting()
        {
            boostStartMovementVector = owner.InputMovementVector;
            owner.CurrentState = Constants.ACTOR_STATE_BOOSTING;
            GameTimerManager.Instance.SetTimer(
                time: 250,
                updateCallback: (t) => { },
                endCallback: (t) =>
                {
                    owner.CurrentState = Constants.ACTOR_STATE_IDLE;
                }
            );
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
            if (owner.CurrentStateProperties.CanChangeMovementVector)
            {
                owner.InputMovementVector = input.GetMovement();
            }
        }

        private void HandleAiming()
        {
            owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(
                input.GetFacingDirection(owner.Position)
            );
        }

        private void SetState()
        {
            var newPlayerState = owner.CurrentState;

            if ((owner.InputMovementVector.X != 0 || owner.InputMovementVector.Y != 0)
                && owner.CurrentStateProperties.CanInitiateWalkingState)
            {
                newPlayerState = Constants.ACTOR_STATE_WALKING;
            }
            else if (owner.CurrentStateProperties.CanInitiateIdleState)
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
