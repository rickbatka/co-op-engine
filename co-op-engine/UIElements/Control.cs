using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.UIElements
{
    /// <summary>
    /// base for any control object meant to be controlled by the control manager
    /// </summary>
    abstract public class Control
    {
        //mouse events
        abstract public event EventHandler OnMouseEnter;
        abstract public event EventHandler OnMouseLeave;
        abstract public event EventHandler OnLeftClick;
        abstract public event EventHandler OnRightClick;

        //button and gamepad events
        abstract public event EventHandler OnSelected;
        abstract public event EventHandler OnInteracted; //this name sucks... can't think of a good you pressed something while it was selected

        public int TabIndex; //couldn't think of a good name so I used the winform name we'd recognize
        public bool Selectable;

        public bool Enabled = true; //if it gets updates
        public bool Clickable = false; //if it fires or checks mouse events
        public bool Visible = true; //if it is included in the draw loop
        public Rectangle Bounds; //the 'selectable' region it occupies on screen

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
