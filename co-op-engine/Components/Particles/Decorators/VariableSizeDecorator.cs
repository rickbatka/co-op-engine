using co_op_engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Particles.Decorators
{
    class VariableSizeDecorator : ParticleDecorator
    {
        private int min;
        private int max;

        public VariableSizeDecorator(IParticle particle, int min, int max) 
        : base(particle) 
        {
            this.min = min;
            this.max = max;
        }

        public override void Begin()
        {
            int newSize = MechanicSingleton.Instance.rand.Next(min, max);
            particle.Width = newSize;
            particle.Height = newSize;
            base.Begin();
        }
    }
}
