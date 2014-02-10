using co_op_engine.Components.Input;
using co_op_engine.Components.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains
{

    class PlayerBrain : BrainBase, ISentient
    {
        private IHumanInput input;

        public PlayerBrain(GameObject owner, IHumanInput input)
            : base(owner)
        {
            this.input = input;
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        override public void Update(GameTime gameTime)
        {
            HandleActions();
            HandleMovement();
        }

        private void HandleActions()
        {
            if (input.IsPressingRunButton())
            {
                owner.IsBoosting = true;
            }
            else
            {
                owner.IsBoosting = false;
            }
        }

        private void HandleMovement()
        {
            owner.InputMovementVector = input.GetMovement();
        }

    }
}
