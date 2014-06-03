using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles.Decorators
{
    class SpinDecorator : ParticleDecorator
    {
        private int spinRate;
        private TimeSpan timer;

        public SpinDecorator(int spinrate, IParticle particle)
            : base(particle)
        {
            this.spinRate = spinrate;
            timer = TimeSpan.FromMilliseconds(spinrate);
        }

        public override void Update(GameTime gameTime)
        {
            float progress = (float)((gameTime.TotalGameTime.TotalMilliseconds % spinRate) / gameTime.TotalGameTime.TotalMilliseconds);
            Rotation = MathHelper.Lerp(0, MathHelper.TwoPi, progress);

            base.Update(gameTime);
        }
    }
}
