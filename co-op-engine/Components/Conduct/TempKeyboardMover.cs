using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace co_op_engine.Components.Conduct
{
    class TempKeyboardMover : MoverBase, IMovable
    {
        public TempKeyboardMover(GameObject owner)
            : base(owner)
        { 
            //@TODO get into factoy
            this.position = new Vector2(50, 50);
            this.width = 50;
            this.height = 50;
        }

        override public void Update(GameTime gameTime)
        {
            HandleInput();
            UpdatePosition(gameTime);
        }

        private void HandleInput()
        {
            if (InputHandler.KeyDown(Keys.S))
            {
                this.inputMovementVector.Y = 1;
            }
            else if (InputHandler.KeyDown(Keys.W))
            {
                this.inputMovementVector.Y = -1;
            }
            else
            {
                this.inputMovementVector.Y = 0;
            }

            if (InputHandler.KeyDown(Keys.A))
            {
                this.inputMovementVector.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D))
            {
                this.inputMovementVector.X = 1;
            }
            else
            {
                this.inputMovementVector.X = 0;
            }

            acceleration = (inputMovementVector * accelerationModifier);

        }

        private void UpdatePosition(GameTime gameTime)
        {
            //this.position += this.movementDirection;
            

            velocity *= friction;

            if ((velocity + acceleration).Length() < speedLimit)
            {
                velocity += acceleration;
            }

            acceleration = Vector2.Zero;

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        
    }
}
