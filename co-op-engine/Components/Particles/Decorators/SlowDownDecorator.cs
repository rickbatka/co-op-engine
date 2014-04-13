using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles.Decorators
{
	class SlowDownDecorator : ParticleDecorator
	{
		private TimeSpan totalDuration;
		private Vector2 startVelocity;
		private float amountToChangeX;
		private float amountToChangeY;

		public SlowDownDecorator(IParticle particle)
			: base(particle) { }

		public override void Begin()
		{
			totalDuration = particle.Lifetime;
			startVelocity = particle.Velocity;
			amountToChangeX = -startVelocity.X;
			amountToChangeY = -startVelocity.Y;
			base.Begin();
		}

		public override void Update(GameTime gameTime)
		{
			float elapsedMS = (float)((totalDuration - particle.Lifetime).TotalMilliseconds);
			
			float newX = particle.Velocity.X;
			newX = DrawingUtility.EaseInOutLinear(startVelocity.X, amountToChangeX, (float)totalDuration.TotalMilliseconds, elapsedMS);

			float newY = particle.Velocity.Y;
			newY = DrawingUtility.EaseInOutLinear(startVelocity.Y, amountToChangeY, (float)totalDuration.TotalMilliseconds, elapsedMS);

			particle.Velocity = new Vector2(newX, newY);

			base.Update(gameTime);
		}

		

	}
}
