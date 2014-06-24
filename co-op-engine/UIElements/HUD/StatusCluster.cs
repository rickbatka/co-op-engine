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
        private UIBar HealthBar;
        //private UIBar ResourceBar; //(blood?)

        public override event EventHandler OnMouseEnter;
        public override event EventHandler OnMouseLeave;
        public override event EventHandler OnLeftClick;
        public override event EventHandler OnRightClick;
        public override event EventHandler OnSelected;
        public override event EventHandler OnInteracted;
        
        private GameObject Observing;

        public StatusCluster(GameObject watched)
        {
            //TODO: invent definitions for ui spritesheets
            Observing = watched;
            HealthBar = new UIBar(AssetRepository.Instance.UIBars, new Rectangle(30,30, 256,32), new Rectangle(0,8,64,8), new Rectangle(0,0,64,8));

            watched.Health.OnValueChanged += HandleHealthChange;
        }

        private void HandleHealthChange(object sender, ConstrainedValueEventArgs e)
        {
            var bar = sender as ConstrainedValue;
            float ratio = bar.Value / bar.MaxValue;
            HealthBar.UpdatePercentage(ratio);
        }

        private void HandleHealthMaxChange(object sender, ConstrainedValueEventArgs e)
        {
            //dupe yes, but meh...
            var bar = sender as ConstrainedValue;
            float ratio = bar.Value / bar.MaxValue;
            HealthBar.UpdatePercentage(ratio);
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
            HealthBar.Draw(spriteBatch);
        }
    }
}
