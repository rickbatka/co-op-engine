using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.UIElements.HUD
{
    class UIBar
    {
        private Rectangle BackgroundDrawRectangle;
        private Rectangle BackgroundSourceRectangle;

        private Rectangle ForegroundDrawRectangle;
        private Rectangle ForegroundSourceRectangle;
        private Rectangle ForeGroundSourceRectangleAdjusted;

        private Texture2D Texture;

        public UIBar(Texture2D texture, Rectangle screenDrawRectangle, Rectangle foregroundSourceRect, Rectangle backgroundSourceRect)
        {
            Texture = texture;
            BackgroundDrawRectangle = ForegroundDrawRectangle = screenDrawRectangle;
            ForegroundSourceRectangle = ForeGroundSourceRectangleAdjusted = foregroundSourceRect;
            BackgroundSourceRectangle = backgroundSourceRect;
        }

        public void UpdatePercentage(float percentage)
        {
            //set draw here
            ForegroundDrawRectangle = new Rectangle(
                BackgroundDrawRectangle.X,
                BackgroundDrawRectangle.Y,
                (int)(((float)BackgroundDrawRectangle.Width) * percentage),
                BackgroundDrawRectangle.Height);


            //bar scrolls left (maybe add stretch option later) (had to draw it out...)
            // x                  aw        w
            // |-----------------)|         |
            //
            //s:    x=w-aw+x   w=aw         w
            // |---------|-----------------)|
            int adjustedWidth = (int)(((float)ForegroundSourceRectangle.Width) * percentage);
            int x = ForegroundSourceRectangle.Width - adjustedWidth + ForegroundSourceRectangle.X;

            ForeGroundSourceRectangleAdjusted = new Rectangle(
                x,
                ForegroundSourceRectangle.Y,
                adjustedWidth,
                ForegroundSourceRectangle.Height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, BackgroundDrawRectangle, BackgroundSourceRectangle, Color.White);
            spriteBatch.Draw(Texture, ForegroundDrawRectangle, ForeGroundSourceRectangleAdjusted, Color.White);
        }
    }
}
