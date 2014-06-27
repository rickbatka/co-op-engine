﻿using co_op_engine.Collections;
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

namespace co_op_engine.Components
{
    public class GameObject : IRenderable
    {
        //componenets
        public PhysicsBase Physics;
        public RenderBase Renderer;
        public BrainBase Brain;
        public SkillsComponent Skills;
        public CombatBase Combat;
        public MoverBase Mover;
        public EngineBase Engine;

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
        public float BoostModifier = 2f;
        public float Scale { get; set; }

        public Texture2D Texture { get; set; }

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

        //character values, only specific to live entities?
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

        public void EquipWeapon(Weapon weapon)
        {
            this.Skills.SetWeapon(weapon);
        }

        public void EquipRage(Rage rage)
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

            //@TODO DEBUGDRAW DEBUG DRAW
            //Renderer.DebugDraw(spriteBatch);
            //Physics.DebugDraw(spriteBatch);
        }

        public void HandleHitBySkill(Skill skill)
        {
            if (Combat != null)
            {
                Combat.HandleHitBySkill(skill);
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

        public event EventHandler<EventArgs> OnDeath;
        public event EventHandler<EventArgs> OnDidAttackNonFriendly;
        public event EventHandler<EventArgs> OnWasAffectedByNonFriendlyWeapon;
        public event EventHandler<EventArgs> OnUsedWeaponEffectOnFriendly;
        public event EventHandler<EventArgs> OnWasAffectedByFriendlyWeapon;
        public event EventHandler<FireProjectileEventArgs> OnWasFiredAtFixedPoint;

        public void FireOnDeath(object sender, EventArgs args) { if (OnDeath != null) OnDeath(sender, args); }
        public void FireOnWasAffectedByFriendlyWeapon(object sender, EventArgs args) { if (OnWasAffectedByFriendlyWeapon != null) OnWasAffectedByFriendlyWeapon(sender, args); }
        public void FireOnWasAffectedByNonFriendlyWeapon(object sender, EventArgs args) { if (OnWasAffectedByNonFriendlyWeapon != null) OnWasAffectedByNonFriendlyWeapon(sender, args); }
        public void FireOnDidAttackNonFriendly(object sender, EventArgs args) { if (OnDidAttackNonFriendly != null) OnDidAttackNonFriendly(sender, args); }
        public void FireOnUsedWeaponEffectOnFriendly(object sender, EventArgs args) { if (OnUsedWeaponEffectOnFriendly != null) OnUsedWeaponEffectOnFriendly(sender, args); }
        public void FireOnWasFiredAtFixedPoint(object sender, FireProjectileEventArgs args) { if (OnWasFiredAtFixedPoint != null) OnWasFiredAtFixedPoint(sender, args); }
    }
}
