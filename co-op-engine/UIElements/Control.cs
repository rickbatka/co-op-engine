using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.UIElements
{
    abstract public class Control
    {
        //mouse
        abstract public event EventHandler OnMouseEnter;
        abstract public event EventHandler OnMouseLeave;
        abstract public event EventHandler OnLeftClick;
        abstract public event EventHandler OnRightClick;

        //controller + keyboard
        abstract public event EventHandler OnSelected;
        abstract public event EventHandler OnInteracted; //this name sucks... can't think of a good you pressed something while it was selected

        public int TabIndex; //couldn't think of a good name so I used the winform name we'd recognize
        public bool Selectable;

        public bool Enabled = true;
        public bool Clickable = false;
        public bool Visible = true;
        public Rectangle Bounds;

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
