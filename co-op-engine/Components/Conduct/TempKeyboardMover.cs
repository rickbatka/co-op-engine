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
            UpdatePosition();
        }

        private void HandleInput()
        {
            if (InputHandler.KeyDown(Keys.Down))
            {
                this.movementDirection.Y = 1;
            }
            else
            {
                this.movementDirection.Y = 0;
            }
        }

        private void UpdatePosition()
        {
            this.position += this.movementDirection;
        }

        override public void Draw(SpriteBatch spriteBatch) { }

        
    }
}
