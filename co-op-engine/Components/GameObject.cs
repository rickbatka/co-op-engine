using co_op_engine.Collections;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components
{
    public class GameObject 
    {
        public PhysicsBase physics;
        public RenderBase renderer;
        public BrainBase brain;

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

        public void Update(GameTime gameTime)
        {
            brain.Update(gameTime);
            physics.Update(gameTime);
            
            renderer.Update(gameTime);
            
        }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            renderer.Draw(spriteBatch);
            physics.Draw(spriteBatch);
            brain.Draw(spriteBatch);
        }

        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}
