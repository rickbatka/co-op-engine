using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Movement
{
    public class MoverBase
    {
        protected GameObject Owner;
        protected float friction = 0.5f;

        public MoverBase(GameObject owner)
        {
            this.Owner = owner;
            this.Owner.FacingDirection = Constants.South;
        }
        

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        virtual public void Update(GameTime gameTime) 
        {
            SetFacingDirection();
        }

        private void SetFacingDirection()
        {
            int oldDirection = Owner.FacingDirection;
            int newDirection = oldDirection;

            var rotation = Owner.RotationTowardFacingDirectionRadians;

            if (Math.Abs(rotation) < (Math.PI) / 3f)
            {
                newDirection = Constants.North;
            }
            else if (Math.Abs(rotation) > ((Math.PI) / 3f) * 2)
            {
                newDirection = Constants.South;
            }
            else
            {
                if (rotation < 0)
                {
                    newDirection = Constants.West;
                }
                else
                {
                    newDirection = Constants.East;
                }
            }

            Owner.FacingDirection = newDirection;
        }
    }
}
