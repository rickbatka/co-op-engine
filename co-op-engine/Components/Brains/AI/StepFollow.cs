using co_op_engine.Components.Input;
using co_op_engine.ServiceProviders;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.AI
{
    enum State
    {
        Neutral,
        CoolingDown,
        ChasingPlayer
    }
    class StepFollow : BrainBase, ISentient
    {
        private State currentState;
        private GameObject currentTarget;
        private IActorInformationProvider actorInfoService;

        private const int stepTime = 200;

        public StepFollow(GameObject owner)
            : base(owner)
        {
            actorInfoService = (IActorInformationProvider)GameServicesProvider.GetService(typeof(IActorInformationProvider));
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        override public void Update(GameTime gameTime)
        {
            SetState();
            SetMovement();
        }

        private void SetState()
        {
            if (currentState != State.Neutral) return;

            AttemptAquireTarget();
            if (currentTarget != null)
            {
                currentState = State.ChasingPlayer;
                GameTimerManager.Instance.SetTimer(stepTime, t =>  EnterCooldown(), owner);
            }

        }

        private void AttemptAquireTarget()
        {
            var players = actorInfoService.GetPlayers();
            int playerCount = players.Count;
            if (playerCount > 0)
            {
                currentTarget = players.First();
            }
        }

        private void SetMovement()
        {
            Vector2 desiredMovement = new Vector2(0, 0);

            if (currentState == State.ChasingPlayer && currentTarget != null)
            { 
                desiredMovement = MoveTowardTarget();
            }

            owner.InputMovementVector = desiredMovement;
        }

        private Vector2 MoveTowardTarget()
        { 
            float x = 0f;
            float y = 0f;
            x = currentTarget.Position.X < owner.Position.X ? -1 : x;
            x = currentTarget.Position.X > owner.Position.X ? 1 : x;
            y = currentTarget.Position.Y < owner.Position.Y ? -1 : y;
            y = currentTarget.Position.Y > owner.Position.Y ? 1 : y;
            return new Vector2(x, y);
        }

        private void EnterCooldown()
        {
            currentState = State.CoolingDown;
            GameTimerManager.Instance.SetTimer(stepTime, t => currentState = State.Neutral, owner);
        }
    }
}
