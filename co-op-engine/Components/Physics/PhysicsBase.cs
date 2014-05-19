using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace co_op_engine.Components.Physics
{
    public delegate void ActorDirectionChangedEventHandler (PhysicsBase sender, ActorDirectionChangedEventArgs directionData);
    public struct ActorDirectionChangedEventArgs
    {
        public int OldDirection;
        public int NewDirection;
    }
    /// <summary>
    /// contains the physical representation of the object
    /// in the level.  implementations can be physical or not.
    /// </summary>
    public class PhysicsBase
    {
        protected GameObject owner;

        public PhysicsBase(GameObject owner)
        {
            this.owner = owner;
        }

        virtual public void Update(GameTime gameTime)
        {
            VerifyBoundingBox();
        }

        public void VerifyBoundingBox()
        {
            owner.BoundingBox = new Rectangle(
                (int)(owner.CurrentFrame.PhysicsRectangle.X + (this.owner.Position.X - this.owner.CurrentFrame.DrawRectangle.Center.X)),
                (int)(owner.CurrentFrame.PhysicsRectangle.Y + (this.owner.Position.Y - this.owner.CurrentFrame.DrawRectangle.Center.Y)), 
                this.owner.CurrentFrame.PhysicsRectangle.Width, 
                this.owner.CurrentFrame.PhysicsRectangle.Height);
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException();
        }
    }
}


/* * * * * *
 * notes on physics overhaul:
 *  - units units units
 *  - only use friction and force
 *  - introduce concept of impulse force along with it's own friction coefficient
 *  - 
 */