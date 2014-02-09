using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReferenceMaterial.Entity;

namespace ReferenceMaterial.Components.Conduct
{
	public enum Action
	{
		Attack,
		Cast
	}

	public delegate void BrainFireEvent();
	public delegate void ActionFireEvent(Action action);

	abstract class BrainBase
	{
		protected readonly GameObject owner;
		public Vector2 MoveDirection;

		public event BrainFireEvent AttemptingToMove;
		public event BrainFireEvent StopAttemptingToMove;
		public event ActionFireEvent StartAction;
		public event BrainFireEvent CancelAction;

		public BrainBase(GameObject owner)
		{
			this.owner = owner;
		}

		abstract public void Update(GameTime gameTime);
		abstract public void Draw(SpriteBatch spriteBatch);

		protected void FireAttemptMoveEvent()
		{
			if (AttemptingToMove != null)
			{
				AttemptingToMove();
			}
		}

		protected void FireStopAttemptingToMoveEvent()
		{
			if (StopAttemptingToMove != null)
			{
				StopAttemptingToMove();
			}
		}

		protected void FireStartActionEvent(Action action)
		{
			if (StartAction != null)
			{
				StartAction(action);
			}
		}

		protected void FireCancelActionEvent()
		{
			if (CancelAction != null)
			{
				CancelAction();
			}
		}
	}
}
