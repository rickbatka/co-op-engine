using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    class PlayerControlInput
    {
        //@TODO the key mappings could live in this class
        public Vector2 GetMovement()
        {
            Vector2 inputVector = new Vector2();
            if (InputHandler.KeyDown(Keys.W))
            {
                inputVector.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S))
            {
                inputVector.Y = 1;
            }
            else
            {
                inputVector.Y = 0;
            }

            if (InputHandler.KeyDown(Keys.A))
            {
                inputVector.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D))
            {
                inputVector.X = 1;
            }
            else
            {
                inputVector.X = 0;
            }
            return inputVector;
        }

        public bool IsPressingRunButton()
        {
            return InputHandler.KeyDown(Keys.Space);
        }

    }
}
