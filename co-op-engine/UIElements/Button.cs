using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private SpriteFont textFont;
        private bool lastSelectedState;

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
            if (InputHandler.MouseMoved())
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
                    else if (!Selected)
                    {
                        //change status to hovering if the mouse just entered the region
                        CMRef.SelectSpecific(this);//; Select();
                        if (OnMouseEnter != null)
                        {
                            OnMouseEnter(this, null);
                        }
                    }
                }/*
                else
                {
                    if (Selected)
                    {
                        //change hovering on mouse leaving
                        Deselect();
                        if (OnMouseLeave != null)
                        {
                            OnMouseLeave(this, null);
                        }
                    }
                }*/
            }

            if (Selected && lastSelectedState)
            {
                if (InputHandler.KeyReleased(Keys.S))
                {
                    CMRef.SelectNext(this, false);
                }
                else if (InputHandler.KeyReleased(Keys.W))
                {
                    CMRef.SelectNext(this, true);
                }
                else if (InputHandler.KeyReleased(Keys.Enter))
                {
                    if (OnInteracted != null)
                    {
                        OnInteracted(this, null);
                    }
                }
            }
            lastSelectedState = Selected;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (Selected)
            {
                color = Color.Red;
            }
            spriteBatch.Draw(BackgroundTexture, Bounds, color);
            spriteBatch.DrawString(textFont, Text, new Vector2(Bounds.X, Bounds.Y), color);
        }

        public override void Select()
        {
            Selected = true;
            if (OnSelected != null)
            {
                OnSelected(this, null);
            }
        }

        public override void Deselect()
        {
            Selected = false;
            if (OnSelected != null)
            {
                OnSelected(this, null);
            }
        }
    }
}
