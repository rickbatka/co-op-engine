﻿using co_op_engine.Collections;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Input;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace co_op_engine.Components
{
    public class GameObject : IRenderable, IPhysical, IMovable, ISentient
    {
        IMovable mover;
        IPhysical physics;
        IRenderable renderer;
        ISentient brain;

        public event EventHandler OnDeath;

        public GameObject()
        {

        }

        public void SetBrain(ISentient brain)
        {
            this.brain = brain;
        }

        public void SetupDevTempComponents(Texture2D tex)
        {
            /////////////////////////////////////////
            //@TODO set these up in factory probably
            if (mover == null)
            {
                mover = new MoverBase(this);
            }

            if (renderer == null)
            {
                renderer = new BasicRenderer(this, tex);
            }

            if (physics == null)
            {
                physics = new NonCollidingPhysics(this);
            }

            if (brain == null)
            {
                this.brain = new PlayerBrain(this, new PlayerControlInput());
            }

            //@END temp setup code
            /////////////////////////////////////////
        }

        public void Update(GameTime gameTime)
        {
            brain.Update(gameTime);
            physics.Update(gameTime);
   
            mover.Update(gameTime);
            renderer.Update(gameTime);
            
        }

        //Interface pass-thru
        #region IRenderable

        public Texture2D Texture { get { return renderer.Texture; } }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            renderer.Draw(spriteBatch);
        }

        #endregion

        #region IPhysical

        public Rectangle BoundingBox { get { return physics.BoundingBox; } }
        public ElasticQuadTree CurrentQuad { get { return physics.CurrentQuad; } }

        #endregion

        #region IMovable

        public Vector2 Velocity { get { return mover.Velocity; } }
        public Vector2 Acceleration { get { return mover.Acceleration; } }
        public Vector2 InputMovementVector { set { mover.InputMovementVector = value; } }
        public Vector2 Position { get { return mover.Position; } }
        public bool IsBoosting { set { mover.IsBoosting = value; } }
        public int Width { get { return mover.Width; } }
        public int Height { get { return mover.Height; } }

        #endregion

    }
}
