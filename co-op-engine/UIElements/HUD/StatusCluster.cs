using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.UIElements.HUD
{
    class StatusCluster : Control
    {
        private float health;
        private float maxHealth;

        public override event EventHandler OnMouseEnter;
        public override event EventHandler OnMouseLeave;
        public override event EventHandler OnLeftClick;
        public override event EventHandler OnRightClick;
        public override event EventHandler OnSelected;
        public override event EventHandler OnInteracted;
        
        private GameObject Observing;

        public StatusCluster(GameObject watched)
        {
            Observing = watched;
        }

        private void HandleHealthChange(object sender, ConstrainedValueEventArgs e)
        {
            health = e.NewValue;
        }

        private void HandleHealthMaxChange(object sender, ConstrainedValueEventArgs e)
        {
        }
        private void HandleMinHealthChange(object sender, ConstrainedValueEventArgs e)
        {
        }
        private void HandleFilledHealth(object sender, ConstrainedValueEventArgs e)
        {
            //maybe shoot some sparkles or something....
        }
        private void HandleEmptyHealth(object sender, ConstrainedValueEventArgs e)
        {
            //
        }

        public override void Update(GameTime gameTime)
        {
            //just a listener, maybe we could have it react or animate here or something
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw the 2 bars
        }
    }
}
