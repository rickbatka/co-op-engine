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
        public ElasticQuadTree CurrentQuad;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 InputMovementVector;
        public int ID;
        public bool UnShovable = false;
        public string DisplayName { get { return "ID: " + ID; } }

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

        public event EventHandler OnDeath;

        public GameObject()
        {
            CurrentState = Constants.ACTOR_STATE_IDLE;
            Health = 100;
            MaxHealth = 100;
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
            Brain.Update(gameTime);
            Physics.Update(gameTime);
            Renderer.Update(gameTime);

            if (Combat != null)
            {
                Combat.Update(gameTime);
            }

            if(Weapon != null)
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
                Weapon.DebugDraw(spriteBatch);
            }

            //@TODO DEBUGDRAW DEBUG DRAW
            //Renderer.DebugDraw(spriteBatch);
            Physics.DebugDraw(spriteBatch);
        }

        public void HandleHitByWeapon(WeaponBase hitByWeapon, int hitCooldownDurationMS)
        {
            if (Combat != null)
            {
                Combat.HandleHitByWeapon(hitByWeapon, hitCooldownDurationMS);
            }
        }

        //#########################  recent merge wiped these out, have to fix the =m anothe day #################

        public UpdateParameters BuildUpdateParams()
        {
            return new UpdateParameters()
            {
                //CurrentActorState = this.currentActorState,
                //FacingDirection = this.facingDirection,
                ID = this.ID,
                //Position = this.position,
                Velocity = this.Velocity,
            };
        }

        public CreateParameters BuildCreateParams()
        {
            return new CreateParameters()
            {
                ID = this.ID,
                ObjectTypeEnumerationPossiblyChangeThisLaterLetsTalkAboutIt = "",
                Brain = this.Brain == null ? null : this.Brain.GetType(),
                Combat = this.Combat == null ? null : this.Combat.GetType(),
                Physics = this.Physics == null ? null : this.Physics.GetType(),
                Renderer = this.Renderer == null ? null : this.Renderer.GetType(),
                Weapon = this.Weapon == null ? null : this.Weapon.GetType()
            };
        }

        public DeleteParameters BuildDeleteParams()
        {
            return new DeleteParameters()
            {
                ID = this.ID,
            };
        }

        public void UpdateFromNetworkParams(UpdateParameters parameters)
        {
            //this.currentActorState = parameters.CurrentActorState;
            //this.facingDirection = parameters.FacingDirection;
            //this.position = parameters.Position;
            this.Velocity = parameters.Velocity;
            CurrentQuad.NotfyOfMovement(this);
        }
    }
}
