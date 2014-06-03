using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Particles.Decorators
{
    class FadeDecorator : ParticleDecorator
    {
        private float endAlpha;
        private float initialAlpha;
        private bool fading;
        private bool done;
        private int start;
        private int duration;
        private TimeSpan timer;

        public FadeDecorator(int start, int duration, float endAlpha, IParticle particle)
            : base(particle)
        {
            fading = false;
            done = false;

            this.endAlpha = endAlpha;
            this.duration = duration;
            this.start = start;
            this.timer = TimeSpan.Zero;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!done)
            {
                timer += gameTime.ElapsedGameTime;

                if (!fading)
                {
                    if (timer.TotalMilliseconds > start)
                    {
                        //trigger
                        fading = true;
                        initialAlpha = Transparency;
                    }
                }
                else
                {
                    float progress = (float)((timer.TotalMilliseconds - start) / duration);
                    float transparency = MathHelper.Lerp(initialAlpha, endAlpha, progress);

                    Transparency = transparency;

                    if (timer.TotalMilliseconds > start + duration)
                    {
                        //end
                        fading = false;
                        done = true;
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
