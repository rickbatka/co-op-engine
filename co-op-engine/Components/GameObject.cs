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

namespace co_op_engine.Components
{
    public class GameObject : IRenderable
    {
        public PhysicsBase Physics;
        public RenderBase Renderer;
        public BrainBase Brain;
        public WeaponBase Weapon;
        public CombatBase Combat;

        public Rectangle BoundingBox;
        public QuadTree CurrentQuad;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 InputMovementVector;
        public int ID;
        public bool UnShovable = false;
        public string DisplayName { get { return "ID: " + ID; } }
        public List<GameObjectCommand> PendingCommands; // we can do whatever here, I don't reall care

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public int CurrentState { get; set; }
        public ActorState CurrentStateProperties { get { return ActorStates.States[CurrentState]; } }
        public int FacingDirection { get; set; }
        public Vector2 FacingDirectionRaw { get; set; }
        public float RotationTowardFacingDirectionRadians { get; set; }
        public bool FullyRotatable { get { return false; } }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Frame CurrentFrame { get; set; }
        public string ConstructionStamp { get; set; }

        public event EventHandler OnDeath;

        public GameObject()
        {
            CurrentState = Constants.ACTOR_STATE_IDLE;
            Health = 100;
            MaxHealth = 100;
            PendingCommands = new List<GameObjectCommand>();
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

        public void EquipWeapon(WeaponBase weapon)
        {
            this.Weapon = weapon;
        }

        public void Update(GameTime gameTime)
        {
            Brain.BeforeUpdate();
            Brain.Update(gameTime);
            Brain.AfterUpdate();

            Physics.Update(gameTime);
            Renderer.Update(gameTime);

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
            Renderer.Draw(spriteBatch);
            Physics.Draw(spriteBatch);
            Brain.Draw(spriteBatch);

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

        public void HandleHitByWeapon(int weaponId, List<EffectDefinition> effects, Vector2 hitRotation)
        {
            if (Combat != null)
            {
                Combat.HandleHitByWeapon(weaponId, effects, hitRotation);
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
