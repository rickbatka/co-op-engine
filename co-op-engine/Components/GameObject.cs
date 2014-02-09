using co_op_engine.Components.Conduct;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using co_op_engine.Collections;

namespace co_op_engine.Components
{
    class GameObject : IRenderable, IPhysical, IMovable, IIntelligent
    {
        MoverBase movementComponent;
        PhysicsBase physicsComponent;
        RenderBase renderComponent;

        public event EventHandler OnDeath;

        public GameObject(Texture2D tex)
        {
            //@TODO set these up in factory probably
            if (movementComponent == null)
            {
                movementComponent = new TempKeyboardMover(this);
            }

            if (renderComponent == null)
            {
                renderComponent = new BasicRenderer(this, tex);
            }

            if (physicsComponent == null)
            {
                physicsComponent = new NonCollidingPhysics(this);
            }
        }

        public void Update(GameTime gameTime)
        {
            movementComponent.Update(gameTime);
            physicsComponent.Update(gameTime);
            renderComponent.Update(gameTime);
        }

        //Interface pass-thru
        #region IRenderable

        public Texture2D Texture { get { return renderComponent.Texture; } }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            renderComponent.Draw(spriteBatch);
        }

        #endregion

        #region IPhysical

        public Rectangle BoundingBox { get { return physicsComponent.BoundingBox; } }
        public ElasticQuadTree CurrentQuad { get { return physicsComponent.CurrentQuad; } }

        #endregion

        #region IMovable

        public Vector2 Velocity { get { return movementComponent.Velocity; } }
        public Vector2 Acceleration { get { return movementComponent.Acceleration; } }
        public Vector2 InputMovementVector { get { return movementComponent.InputMovementVector; } }
        public Vector2 Position { get { return movementComponent.Position; } }
        public int Width { get { return movementComponent.Width; } }
        public int Height { get { return movementComponent.Height; } }

        #endregion

    }
}
