using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.StatusEffects
{
    public class SimplePoison : StatusEffectBase
    {
        private float Damage;
        private TimeSpan TickTimer;
        private int TickInterval = 1000;

        public SimplePoison(GameObject applicant, TimeSpan duration, float damage)
            :base(applicant, duration)
        {
            Damage = damage;
            TickTimer = TimeSpan.FromMilliseconds(TickInterval);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            TickTimer -= gameTime.ElapsedGameTime;
            if (TickTimer <= TimeSpan.Zero)
            {
                TickTimer = TimeSpan.FromMilliseconds(TickInterval);
                ObjectReference.Health.Value -= Damage;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            //no draw
        }

        protected override void OnApplication()
        {
            //nothing
        }

        protected override void OnRemoval()
        {
            base.OnRemoval();
        }
    }
}
