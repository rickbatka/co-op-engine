using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public class EmptyEmitter : Emitter
    {
        protected Texture2D Texture;
        public float yVelocity = -1f;

        public EmptyEmitter(Texture2D texture = null)
        {
            this.duration = TimeSpan.FromMilliseconds(1200000);
            this.frequency = 25;
            this.Texture = texture != null ? texture : AssetRepository.Instance.PlainWhiteTexture;
        }

        protected override void EmitParticle()
        {
            var particle = new Particle(Texture);
            particle.Lifetime = TimeSpan.FromMilliseconds(2550);
            particle.Position = new Vector2(50, 50);
            particle.Velocity = new Vector2(0, yVelocity);
            particle.DrawColor = Color.White;

            ParticleEngine.Instance.Add(particle);
        }
    }
}
