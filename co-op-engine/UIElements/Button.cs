using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.UIElements
{
    /// <summary>
    /// represents a UI button
    /// </summary>
    class Button : Control
    {
        private string Text;
        private Texture2D BackgroundTexture;
        private Texture2D HoverTexture;
        private Texture2D PressedTexture;
        private bool hovering = false;
        private SpriteFont textFont;

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
                    //fire click event
                    if (OnLeftClick != null)
                    {
                        OnLeftClick(this, null);
                    }
                }
                else if (!hovering)
                {
                    //change status to hovering if the mouse just entered the region
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
                    //change hovering on mouse leaving
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
