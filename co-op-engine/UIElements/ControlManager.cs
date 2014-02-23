using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.UIElements
{
    public class ControlManager
    {
        List<Control> controls;

        public ControlManager()
        {
            controls = new List<Control>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var control in controls)
            {
                if (control.Enabled)
                {
                    control.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var control in controls)
            {
                if (control.Visible)
                {
                    control.Draw(spriteBatch);
                }
            }
        }
    }
}
