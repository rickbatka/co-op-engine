﻿using co_op_engine.Collections;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Weapons;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components
{
    public class GameObject : IRenderable
    {
        public PhysicsBase Physics;
        public RenderBase Renderer;
        public BrainBase Brain;
        public WeaponBase Weapon;

        public Rectangle BoundingBox;
        public ElasticQuadTree CurrentQuad;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 InputMovementVector;
        public int ID;
        public bool UnShovable = false;
        public string DisplayName { get { return "ID: " + ID; } }

        private int width;
        private int height;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 facingDirectionRaw;
        private float rotationTowardFacingDirectionRadians;
        private int facingDirection;
        private int currentActorState = Constants.STATE_IDLE;
        private int health = 100;
        private int maxHealth = 100;

        public event EventHandler OnDeath;

        public GameObject()
        {
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

        public void EquipWeapon(WeaponBase weapon)
        {
            this.Weapon = weapon;
        }

        public void Update(GameTime gameTime)
        {
            Brain.Update(gameTime);
            Physics.Update(gameTime);
            Renderer.Update(gameTime);

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

            if (Weapon != null)
            {
                Weapon.Draw(spriteBatch);
                Weapon.DebugDraw(spriteBatch);
            }

            //@TODO DEBUGDRAW DEBUG DRAW
            //Renderer.DebugDraw(spriteBatch);
            Physics.DebugDraw(spriteBatch);
        }

        public StatePropertySet CurrentStateProperties { get { return StateProperties.Properties[currentActorState]; } }

        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int CurrentActorState { get { return currentActorState; } set { currentActorState = value; } }
        public int FacingDirection { get { return facingDirection; } set { facingDirection = value; } }
        public Vector2 FacingDirectionRaw { get { return facingDirectionRaw; } set { facingDirectionRaw = value; } }
        public float RotationTowardFacingDirectionRadians { get { return rotationTowardFacingDirectionRadians; } set { rotationTowardFacingDirectionRadians = value; } }
        public bool FullyRotatable { get { return false; } }
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    }
}
