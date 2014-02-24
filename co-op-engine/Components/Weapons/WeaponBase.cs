using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons
{
    public abstract class WeaponBase : IRenderable
    {
        protected GameObject owner;
        protected AnimatedRenderer renderer;

        protected Texture2D texture;
        public Texture2D TextureProp { get { return texture; } set { texture = value; } }
        public Vector2 PositionProp { get { return owner.Position; } }
        public int WidthProp { get { return owner.Width; } }
        public int HeightProp { get { return owner.Height; } }
        public ActorState CurrentActorStateProp { get { return owner.currentActorState; } }
        public int FacingDirectionProp { get { return owner.FacingDirection; } }

        public WeaponBase(GameObject owner)
        {
            this.owner = owner;
        }

        public void SetRenderer(AnimatedRenderer renderer)
        {
            this.renderer = renderer;
        }

        virtual public void Update(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            renderer.Draw(spriteBatch);
        }


    }
}
