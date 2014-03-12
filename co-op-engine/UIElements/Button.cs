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

            var dimensions = font.MeasureString(text);

            //have button stretch to fit text width
            this.Bounds = new Rectangle(position.X, position.Y, (int)dimensions.X, (int)dimensions.Y);
        }

        public override void Update(GameTime gameTime)
        {
            //check mouse movements
            if (this.Bounds.Contains(InputHandler.MousePositionPoint()))
            {
                if (InputHandler.MouseLeftPressed())
                {
                    if (OnInteracted != null)
                    {
                        OnInteracted(this, null);
                    }
                }
                else if (!hovering)
                {
                    hovering = true;
                    if (OnMouseEnter != null)
                    {
                        OnMouseEnter(this, null);
                    }
                }
            }
            else
            {
                if (hovering)
                {
                    hovering = false;
                    if (OnMouseLeave != null)
                    {
                        OnMouseLeave(this, null);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (hovering)
            {
                color = Color.Red;
            }
            spriteBatch.Draw(BackgroundTexture, Bounds, color);
            spriteBatch.DrawString(textFont, Text, new Vector2(Bounds.X, Bounds.Y), color);
        }

        
    }
}
