using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ReferenceMaterial.Entity.EntityComponents
{
	class AnimatedCharacterRender : RenderBase
	{
		//CharacterAnimator animator;
		Texture2D spriteSheet;

		public AnimatedCharacterRender(GameObject owner, Texture2D spriteSheet)
			:base(owner)
		{
			//animator = new CharacterAnimator(animationData);
			this.spriteSheet = spriteSheet;

			owner.BrainComponent.AttemptingToMove += HandleAttemptToMoveEvent;
			owner.BrainComponent.StopAttemptingToMove += HandleStopAttemptingToMoveEvent;
			owner.BrainComponent.StartAction += HandleStartActionEvent;
			owner.BrainComponent.CancelAction += HandleCancelActionEvent;
		}

		private void HandleAttemptToMoveEvent()
		{
			//animator.SetAnimation(CharacterAnimation.Move);
		}

		private void HandleStopAttemptingToMoveEvent()
		{
			//animator.SetAnimation(CharacterAnimation.Idle);
		}

		private void HandleStartActionEvent(Action action)
		{
		}

		private void HandleCancelActionEvent()
		{
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			//animator.Update(gameTime);
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(spriteSheet, owner.PhysicsComponent.BoundryBox, animator.SourceRectangle, Color.White);
		}
	}
}
