using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public class EmptyEmitter : Emitter
    {
        public EmptyEmitter()
        {
            this.duration = TimeSpan.FromMilliseconds(40000);
            this.frequency = 25;
        }

        protected override void EmitParticle()
        {
            var particle = new Particle();
            particle.Lifetime = TimeSpan.FromMilliseconds(2550);
            particle.Position = new Vector2(50, 50);
            particle.Velocity = new Vector2(0, 5);
            particle.DrawColor = Color.Black;

            ParticleEngine.Instance.Add(particle);
        }
    }
}
