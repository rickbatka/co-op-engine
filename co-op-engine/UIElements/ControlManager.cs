using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using co_op_engine.Components;
using co_op_engine.UIElements.HUD;

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
        private List<Control> controls;

        public void AddControl(Control control)
        {
            control.CMRef = this;
            controls.Add(control);
        }

        public ControlManager()
        {
            controls = new List<Control>();
        }

        public void SelectNext(Control selected, bool up)
        {
            controls = controls.OrderBy(c => c.TabIndex).ToList();

            int direction = up ? 1 : -1;
            int index = selected == null ? 0 : controls.IndexOf(selected);

            do
            {
                index = (index + direction + controls.Count) % controls.Count;
            }
            while (controls[index].Selectable);

            var newselect = controls[index];

            newselect.Select();
            selected.Deselect();
        }

        public void SelectSpecific(Control selected)
        {
            if (controls.Any(c => c.Selected))
            {
                controls.Where(c => c.Selected).ToList().ForEach((c) => { c.Deselect(); });
            }
            selected.Select();
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

        public void BuildHUDForPlayer(GameObject player)
        {
            var playerbars = new StatusCluster(player);
            AddControl(playerbars);
        }
    }
}
