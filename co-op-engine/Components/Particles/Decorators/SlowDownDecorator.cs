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
			float elapsedMS = (totalDuration - particle.Lifetime).Milliseconds;
			
			float newX = particle.Velocity.X;
			newX = linearTween(elapsedMS, startVelocity.X, amountToChangeX, totalDuration.Milliseconds);

			float newY = particle.Velocity.Y;
			newY = linearTween(elapsedMS, startVelocity.Y, amountToChangeY, totalDuration.Milliseconds);

			particle.Velocity = new Vector2(newX, newY);

			base.Update(gameTime);
		}

		private float linearTween(float currentTime, float startValue, float changeInValue, float duration) 
		{
			return changeInValue * currentTime / duration + startValue;
		}

	}
}
