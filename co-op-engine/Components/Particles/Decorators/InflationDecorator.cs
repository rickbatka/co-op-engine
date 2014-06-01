using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Particles.Decorators
{
    class InflationDecorator : ParticleDecorator
    {
        private int startTime;
        private int endTime;
        private int duration;
        private TimeSpan timer;
        private float amount;
        private Vector2 initialSize;

        private bool expanding;
        private bool done;

        public InflationDecorator(int start, int duration, float inflationAmount, IParticle particle)
            : base(particle)
        {
            startTime = start;
            this.duration = duration;
            endTime = startTime + duration;
            amount = inflationAmount;

            expanding = false;
            done = false;
        }

        public override void Begin()
        {
            timer = TimeSpan.Zero;
            base.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            if (!done)
            {
                timer += gameTime.ElapsedGameTime;

                if (!expanding)
                {
                    if (timer.TotalMilliseconds > startTime)
                    {
                        expanding = true; //trigger to start expanding
                        initialSize = new Vector2(DrawRectangle.Width, DrawRectangle.Height);
                    }
                }
                else
                {
                    //expansion logic here
                    
                    //get diff, apply inflation amount compared to it, if zero, add to next time

                    var timeQuant = (gameTime.ElapsedGameTime.TotalMilliseconds / duration);
                    var expansionDimensions = timeQuant * amount;

                    var temp = DrawRectangle;
                    temp.Inflate((float)expansionDimensions, (float)expansionDimensions);
                    DrawRectangle = temp;
                    /*temp.Inflate((float)expansionDimensions, (float)expansionDimensions);
                    DrawRectangle = new Rectangle( (int)(DrawRectangle.X - expansionDimensions.X),
                        (int)(DrawRectangle.Y - expansionDimensions.Y),
                        (int)(DrawRectangle.Width + expansionDimensions.X * 2),
                        (int)(DrawRectangle.Height + expansionDimensions.Y * 2));*/

                    if (timer.TotalMilliseconds > endTime)
                    {
                        //stop expanding trigger
                        expanding = false;
                        done = true;
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
