using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.UIElements
{
    /// <summary>
    /// Handles the UI controls using a winform like
    /// event driven notification system
    /// </summary>
    public class ControlManager
    {
        /// <summary>
        /// list of all controls this manager controls
        /// </summary>
        public List<Control> controls;

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
