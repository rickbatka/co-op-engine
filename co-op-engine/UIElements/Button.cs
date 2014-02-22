using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.UIElements
{
    class Button : Control
    {
        string Text;
        Texture2D BackgroundTexture;
        Texture2D HoverTexture;
        Texture2D PressedTexture;
        bool hovering = false;
        SpriteFont textFont;

        public override event EventHandler OnMouseEnter;
        public override event EventHandler OnMouseLeave;
        public override event EventHandler OnLeftClick;
        public override event EventHandler OnRightClick;
        public override event EventHandler OnSelected;
        public override event EventHandler OnInteracted;

        public Button(Texture2D background, string text, Texture2D hover, Texture2D pressed, Point position, SpriteFont font)
        {
            this.Text = text;
            this.BackgroundTexture = background;
            this.HoverTexture = hover;
            this.PressedTexture = pressed;
            this.textFont = font;

            //have button stretch to fit text width
            this.Bounds = new Rectangle(position.X, position.Y, 32, 90);
        }

        public override void Update(GameTime gameTime)
        {
            //check mouse movements
            if (this.Bounds.Contains(InputHandler.MousePositionPoint()))
            {
                if (!hovering)
                {
                    hovering = true;
                    if (OnMouseEnter != null)
                    {

                    }
                }
            }
            //check button presses
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        
    }
}
