using co_op_engine.Components.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    public class RenderBase
    {
        protected readonly IRenderable owner;

        protected Rectangle? currentDrawRectangle = null;

        public RenderBase(IRenderable owner, Texture2D texture)
        {
            this.owner = owner;
            this.owner.Texture = texture;
        }

        virtual public void Update(GameTime gameTime)
        { 
            
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: owner.Texture, 
                destinationRectangle: GetDrawTarget(), 
                sourceRectangle: currentDrawRectangle, 
                color: Color.White,
                rotation: owner.FullyRotatable ? owner.RotationTowardFacingDirectionRadians : 0f,
                origin: GetCenterOrigin(),
                effect: SpriteEffects.None,
                depth: 0f);
        }

        virtual public void DebugDraw(SpriteBatch spriteBatch)
        {

            //the full curent render box
            spriteBatch.Draw(
                texture: AssetRepository.Instance.PlainWhiteTexture,
                destinationRectangle: GetDrawTarget(),
                sourceRectangle: currentDrawRectangle,
                color: new Color(Color.Green, 0.01f),
                rotation: owner.FullyRotatable ? owner.RotationTowardFacingDirectionRadians : 0f,
                origin: GetCenterOrigin(),
                effect: SpriteEffects.None,
                depth: 0f);
        }

        private Rectangle GetDrawTarget()
        {
            return new Rectangle(
                x: (int)(owner.Position.X),
                y: (int)(owner.Position.Y),
                width: owner.Width,
                height: owner.Height
            );
        }

        protected Vector2 GetCenterOrigin()
        {
            var center = new Vector2(
                x: owner.Width/2f,
                y: owner.Height/2f
            );
            
            return center;
        }
    }
}
