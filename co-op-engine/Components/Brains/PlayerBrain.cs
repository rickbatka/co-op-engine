using System;
using co_op_engine.Components.Input;
using co_op_engine.Factories;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using co_op_engine.Utility.Camera;

namespace co_op_engine.Components.Brains
{
    
    class PlayerBrain : BrainBase
    {
        private PlayerControlInput input;


        private Vector2 previousMovementVector;
        private int previousState;
        private Vector2 boostStartMovementVector;

        public PlayerBrain(GameObject owner, PlayerControlInput input)
            : base(owner, false)
        {
            this.input = input;
        }

        override public void Draw(SpriteBatch spriteBatch) 
        {
            base.Draw(spriteBatch);
        }

        override public void BeforeUpdate()
        {
            input.BeforeUpdate();
            base.BeforeUpdate();
        }

        override public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            HandleAiming();
            HandleWeaponToggle();
            HandleActions();
            HandleMovement();
            base.Update(gameTime);
        }

        override public void AfterUpdate()
        {
            input.AfterUpdate();
            if ((previousMovementVector != null && previousMovementVector != Owner.InputMovementVector)
                || (previousState != Owner.CurrentState))
            {
                SendUpdate(new PlayerBrainUpdateParams()
                {
                    InputMovementVector = Owner.InputMovementVector,
                    Position = Owner.Position,
                    RotationTowardFacingDirectionRadians = Owner.RotationTowardFacingDirectionRadians,
                    CurrentState = Owner.CurrentState
                });
            }

            previousMovementVector = Owner.InputMovementVector;
            previousState = Owner.CurrentState;
            base.AfterUpdate();
        }

        private void HandleWeaponToggle()
        {
            if (InputHandler.KeyPressed(Keys.D1))
            {
                Owner.EquipWeapon(PlayerFactory.Instance.GetSword(Owner));
            }
            //if (InputHandler.KeyPressed(Keys.D2))
            //{
            //    Owner.EquipWeapon(PlayerFactory.Instance.GetAxe(Owner));
            //}
            //if (InputHandler.KeyPressed(Keys.D3))
            //{
            //    Owner.EquipWeapon(PlayerFactory.Instance.GetMace(Owner));
            //}
        }

        private void HandleActions()
        {
            if (input.IsPressingAttackButton())
            {
                Owner.Skills.TryInititateWeaponAttack();
            }

            if (input.IsPressingRageButton())
            {
                Owner.Skills.TryInitiateRage();
            }

            if (input.IsPressingBoostButton())
            {
                Owner.Skills.TryInitiateBoost();
            }

            /*
             * Pretty much everything below here is for debugging / testing.
             * */
            if (InputHandler.KeyPressed(Keys.T))
            {
                TowerFactory.Instance.GetFriendlyAOEHealingTower();
            }

            if (InputHandler.KeyPressed(Keys.Y))
            {
                TowerFactory.Instance.GetArrowTower();
            }

            if (InputHandler.KeyPressed(Keys.E))
            {
                if (MechanicSingleton.Instance.rand.Next(1, 10) > 5)
                {
                    PlayerFactory.Instance.GetEnemyFootSoldier();
                }
                else
                {
                    PlayerFactory.Instance.GetEnemySlime();
                }
            }

            if (InputHandler.KeyPressed(Keys.C))
            {
                Camera.Instance.Shake();
            }

            if (InputHandler.KeyPressed(Keys.L)) // L is for teleport, duh
            {
                Owner.Position = InputHandler.MousePositionVectorCameraAdjusted();
            }

            if (InputHandler.KeyPressed(Keys.B))
            {
                SetBoosting();
            }
        }

        private void SetBoosting()
        {
            boostStartMovementVector = Owner.InputMovementVector;
            Owner.CurrentState = Constants.ACTOR_STATE_BOOSTING;
            GameTimerManager.Instance.SetTimer(
                time: 250,
                updateCallback: (t) => { },
                endCallback: (t) =>
                {
                    Owner.CurrentState = Constants.ACTOR_STATE_IDLE;
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
            if (!Owner.CurrentStateProperties.CanMove)
            {
                Owner.InputMovementVector = Vector2.Zero;
            }
            else
            {
                Owner.InputMovementVector = input.GetMovement();
            }
        }

        private void HandleAiming()
        {
            Owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(
                input.GetFacingDirection(Owner.Position)
            );
        }

    }
}
