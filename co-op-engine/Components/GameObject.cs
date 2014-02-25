using co_op_engine.Collections;
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
        public PhysicsBase physics;
        public RenderBase renderer;
        public BrainBase brain;
        public WeaponBase Weapon;

        public Texture2D Texture;
        public Rectangle BoundingBox;
        public ElasticQuadTree CurrentQuad;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 InputMovementVector;
        public Vector2 Position;
        public int Width;
        public int Height;
        public int ID;
        public int FacingDirection;
        public ActorState currentActorState = ActorState.Idle;
        public bool UnShovable = false;

        public event EventHandler OnDeath;

        public GameObject()
        {
        }

        public void SetPhysics(PhysicsBase physics)
        {
            this.physics = physics;
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.renderer = renderer;
        }

        public void SetBrain(BrainBase brain)
        {
            this.brain = brain;
        }

        public void EquipWeapon(WeaponBase weapon)
        {
            this.Weapon = weapon;
        }

        public void Update(GameTime gameTime)
        {
            brain.Update(gameTime);
            physics.Update(gameTime);
            renderer.Update(gameTime);

            if(Weapon != null)
            {
                Weapon.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            renderer.Draw(spriteBatch);
            physics.Draw(spriteBatch);
            brain.Draw(spriteBatch);

            if (Weapon != null)
            {
                Weapon.Draw(spriteBatch);
                //Weapon.DebugDraw(spriteBatch);
            }

            //@TODO DEBUGDRAW DEBUG DRAW
            //renderer.DebugDraw(spriteBatch);
            physics.DebugDraw(spriteBatch);
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public Texture2D TextureProp { get { return Texture; } set { Texture = value; } }
        public Vector2 PositionProp { get { return Position; } set { Position = value; } }
        public int WidthProp { get { return Width; } set { Width = value; } }
        public int HeightProp { get { return Height; } set { Height = value; } }
        public ActorState CurrentActorStateProp { get { return currentActorState; } }
        public int FacingDirectionProp { get { return FacingDirection; } }
    }
}
