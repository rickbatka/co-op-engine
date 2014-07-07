using co_op_engine.Collections;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Combat;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using co_op_engine.Networking.Commands;
using System.Collections.Generic;
using co_op_engine.World.Level;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Engines;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.Weapons;
using co_op_engine.Components.Skills.Rages;

namespace co_op_engine.Components
{
    public class GameObject : IRenderable
    {
        public int ID;

        //components
        public PhysicsBase Physics;
        public RenderBase Renderer;
        public BrainBase Brain;
        public SkillsComponent Skills;
        public CombatBase Combat;
        public MoverBase Mover;
        public EngineBase Engine;

        public SpacialBase CurrentQuad;//RefactorNote: this is super specific to colliding physics, possibly move to better location
        public Vector2 Velocity;//RefactorNote: move to physics, add helper method called ApplyImpulse takes vector
        public Vector2 Acceleration;//RefactorNote: move to physics, no reason to ever change this
        public Vector2 InputMovementVector;//RefactorNote: hmm
        public bool UsedInPathing = false;
        
        /// <summary>
        /// Used for determining friend from foe, -1 is gaia or scenery
        /// </summary>
        public int Team;//RefactorNote: brain and or someplace accessable to the handle effect application method
        public bool Visible { get; set; }//RefactorNote: used only by the renderer
        public string DisplayName { get { return "ID: " + ID; } }
        public float SpeedAccel = 75f;//RefactorNote: this is specific to a dev boost
        public float BoostModifier = 2f;//RefactorNote: this is specific to a dev boost
        public float Scale { get; set; }//RefactorNote: purely used by renderer and skill? unsure why

        public Texture2D Texture { get; set; }//RefactorNote: used only in renderer

        //physics access stuff
        public Vector2 Position
        {
            get { return Physics.Position; }
            set { Physics.Position = value; }
        }
        public Rectangle PhysicsCollisionBox
        {
            get { return Physics.PhysicsCollisionBox; }
            set { Physics.PhysicsCollisionBox = value; }
        }

        //state info
        public int CurrentState { get; set; }
        public ActorState CurrentStateProperties { get { return ActorStates.States[CurrentState]; } }
        public int FacingDirection { get; set; }
        public float RotationTowardFacingDirectionRadians { get; set; }
        public bool FullyRotatable { get { return false; } }
        public Frame CurrentFrame { get; set; }
        public bool ShouldDelete = false;
        public string ConstructionStamp { get; set; }

        //RefactorNote: character values, only specific to live entities?
        public ConstrainedValue Health { get; set; }
        public ConstrainedValue Blood { get; set; }

        public GameObject()
        {
            CurrentState = Constants.ACTOR_STATE_IDLE;
            Health = new ConstrainedValue(0, 100, 100);
            Visible = true;
            Scale = 1f;
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

        public void SetSkills(SkillsComponent skills)
        {
            this.Skills = skills;
        }

        public void EquipWeapon(WeaponBase weapon)
        {
            this.Skills.SetWeapon(weapon);
        }

        public void EquipRage(RageBase rage)
        {
            this.Skills.SetRage(rage);
        }

        public void SetMover(MoverBase mover)
        {
            this.Mover = mover;
        }

        public void SetEngine(EngineBase engine)
        {
            this.Engine = engine;
        }

        public void Update(GameTime gameTime)
        {
            if (Brain != null)
            {
                Brain.BeforeUpdate();
                Brain.Update(gameTime);
                Brain.AfterUpdate();
            }

            if (Physics != null) { Physics.Update(gameTime); }
            if (Renderer != null) { Renderer.Update(gameTime); }
            if (Combat != null) { Combat.Update(gameTime); }
            if (Skills != null) { Skills.Update(gameTime); }
            if (Mover != null) { Mover.Update(gameTime); }
            if (Engine != null) { Engine.Update(gameTime); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Renderer != null)
            {
                Renderer.Draw(spriteBatch);
            }

            if (Physics != null)
            {
                Physics.Draw(spriteBatch);
            }

            if (Brain != null)
            {
                Brain.Draw(spriteBatch);
            }

            if (Combat != null)
            {
                Combat.Draw(spriteBatch);
                Combat.DebugDraw(spriteBatch);
            }

            if (Skills != null)
            {
                //Skills.DebugDraw(spriteBatch);
                Skills.Draw(spriteBatch);
            }

            //TODO DEBUGDRAW DEBUG DRAW
            //Renderer.DebugDraw(spriteBatch);
            //Physics.DebugDraw(spriteBatch);
        }

        //public void HandleHitBySkill(Skill skill)
        //{
        //    if (Combat != null)
        //    {
        //        Combat.HandleHitBySkill(skill);
        //    }
        //}

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

        //RefactorNote: move to static helper class for the network
        internal CreateParameters BuildCreateParams()
        {
            return new CreateParameters()
            {
                ConstructorId = ConstructionStamp,
                ID = ID,
                Position = Position
            };
        }

        public event EventHandler<EventArgs> OnDeath;
        public event EventHandler<EventArgs> OnDidAttackNonFriendly;
        public event EventHandler<EventArgs> OnWasAffectedByNonFriendlyWeapon;
        public event EventHandler<EventArgs> OnUsedWeaponEffectOnFriendly;
        public event EventHandler<EventArgs> OnWasAffectedByFriendlyWeapon;

        public void FireOnDeath(object sender, EventArgs args) { if (OnDeath != null) OnDeath(sender, args); }
        public void FireOnWasAffectedByFriendlyWeapon(object sender, EventArgs args) { if (OnWasAffectedByFriendlyWeapon != null) OnWasAffectedByFriendlyWeapon(sender, args); }
        public void FireOnWasAffectedByNonFriendlyWeapon(object sender, EventArgs args) { if (OnWasAffectedByNonFriendlyWeapon != null) OnWasAffectedByNonFriendlyWeapon(sender, args); }
        public void FireOnDidAttackNonFriendly(object sender, EventArgs args) { if (OnDidAttackNonFriendly != null) OnDidAttackNonFriendly(sender, args); }
        public void FireOnUsedWeaponEffectOnFriendly(object sender, EventArgs args) { if (OnUsedWeaponEffectOnFriendly != null) OnUsedWeaponEffectOnFriendly(sender, args); }
    }
}
