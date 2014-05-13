using co_op_engine.Collections;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Combat;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Weapons;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using co_op_engine.Networking.Commands;
using co_op_engine.Components.Weapons.Effects;
using System.Collections.Generic;
using co_op_engine.World.Level;

namespace co_op_engine.Components
{
    public class GameObject : IRenderable
    {
        public PhysicsBase Physics;
        public RenderBase Renderer;
        public BrainBase Brain;
        public Weapon Weapon;
        public CombatBase Combat;

        public Level CurrentLevel;

        public Rectangle BoundingBox;
        public SpacialBase CurrentQuad;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 InputMovementVector;
        public int ID;
        public bool UsedInPathing = false;
        public bool Friendly = false;
        public bool Visible { get; set; }
        public string DisplayName { get { return "ID: " + ID; } }
        public float SpeedAccel = 75f;

        public Texture2D Texture { get; set; }

        private Vector2 _pos;
        public Vector2 Position
        {
            get { return _pos; }
            set
            {
                _pos = new Vector2(
                    x: MathHelper.Clamp(value.X, CurrentLevel.Bounds.Left + (BoundingBox.Width / 2) + Level.BorderWallSize, CurrentLevel.Bounds.Right - (BoundingBox.Width / 2) - Level.BorderWallSize) ,
                    y: MathHelper.Clamp(value.Y, CurrentLevel.Bounds.Top + (BoundingBox.Height / 2) + Level.BorderWallSize, CurrentLevel.Bounds.Bottom - (BoundingBox.Width / 2) - Level.BorderWallSize)
                );
            } 
        }

        public int CurrentState { get; set; }
        public ActorState CurrentStateProperties { get { return ActorStates.States[CurrentState]; } }
        public int FacingDirection { get; set; }
        public float RotationTowardFacingDirectionRadians { get; set; }
        public bool FullyRotatable { get { return false; } }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Frame CurrentFrame { get; set; }
        public string ConstructionStamp { get; set; }

        public event EventHandler OnDeath;

        public GameObject(Level currentLevel)
        {
            CurrentLevel = currentLevel;
            CurrentState = Constants.ACTOR_STATE_IDLE;
            Health = 100;
            MaxHealth = 100;
            Visible = true;
        }

        public void SetPhysics(PhysicsBase physics)
        {
            this.Physics = physics;
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.Renderer = renderer;
        }

        public void SetBrain(BrainBase brain)
        {
            this.Brain = brain;
        }

        public void SetCombat(CombatBase combat)
        {
            this.Combat = combat;
        }

        public void EquipWeapon(Weapon weapon)
        {
            this.Weapon = weapon;
        }

        public void Update(GameTime gameTime)
        {
            if(Brain != null)
            {
                Brain.BeforeUpdate();
                Brain.Update(gameTime);
                Brain.AfterUpdate();
            }
            
            if(Physics != null)
            {
                Physics.Update(gameTime);
            }

            if(Renderer != null)
            {
                Renderer.Update(gameTime);
            }
            
            if (Combat != null)
            {
                Combat.Update(gameTime);
            }

            if (Weapon != null)
            {
                Weapon.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(Renderer != null)
            {
                Renderer.Draw(spriteBatch);
            }
            
            if(Physics != null)
            {
                Physics.Draw(spriteBatch);
            }

            if(Brain != null)
            {
                Brain.Draw(spriteBatch);
            }
            
            if (Combat != null)
            {
                Combat.Draw(spriteBatch);
            }

            if (Weapon != null)
            {
                Weapon.Draw(spriteBatch);
                //Weapon.DebugDraw(spriteBatch);
            }

            //@TODO DEBUGDRAW DEBUG DRAW
            //Renderer.DebugDraw(spriteBatch);
            //Physics.DebugDraw(spriteBatch);
        }

        public void HandleHitByWeapon(Weapon weapon)
        {
            if (Combat != null)
            {
                Combat.HandleHitByWeapon(weapon);
            }
        }

        public void UpdateFromNetworkParams(GameObjectCommand command)
        {
            switch (command.ReceivingComponent)
            {
                case GameObjectComponentType.Brain:
                    {
                        Brain.ReceiveCommand(command);
                    }
                    break;
                case GameObjectComponentType.Combat:
                    {
                        Combat.ReceiveCommand(command);
                    }
                    break;
                case GameObjectComponentType.Physics:
                    {
                        Physics.ReceiveCommand(command);
                    }
                    break;
                case GameObjectComponentType.Renderer:
                    {
                        Renderer.ReceiveCommand(command);
                    }
                    break;
                default:
                    {
                        throw new NotImplementedException("No Component By This Enumeration");
                    }
            }
        }

        internal CreateParameters BuildCreateParams()
        {
            return new CreateParameters()
            {
                ConstructorId = ConstructionStamp,
                ID = ID,
                Position = Position
            };
        }
    }
}
