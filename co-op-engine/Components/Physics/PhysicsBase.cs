using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace co_op_engine.Components.Physics
{
    public delegate void ActorDirectionChangedEventHandler(PhysicsBase sender, ActorDirectionChangedEventArgs directionData);
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
        private Vector2 _position;
        public virtual Vector2 Position
        {
            get { return _position; }
            set { _position = EnsureValidPosition(value); }
        }

        protected Rectangle LevelBounds;
        private int levelWallDistance = 30;

        protected Rectangle _physicsCollsisionBox;
        public virtual Rectangle PhysicsCollisionBox
        {
            get { return _physicsCollsisionBox; }
            set { _physicsCollsisionBox = value; }
        }




        public PhysicsBase(GameObject owner, Rectangle levelBounds)
        {
            this.owner = owner;
            LevelBounds = levelBounds;
        }

        virtual public void Update(GameTime gameTime)
        {
            VerifyBoundingBox();
        }

        public void VerifyBoundingBox()
        {
            PhysicsCollisionBox = new Rectangle(
                (int)(owner.CurrentFrame.PhysicsRectangle.X + (this.owner.Position.X - this.owner.CurrentFrame.DrawRectangle.Center.X)),
                (int)(owner.CurrentFrame.PhysicsRectangle.Y + (this.owner.Position.Y - this.owner.CurrentFrame.DrawRectangle.Center.Y)),
                this.owner.CurrentFrame.PhysicsRectangle.Width,
                this.owner.CurrentFrame.PhysicsRectangle.Height);
        }

        protected Vector2 EnsureValidPosition(Vector2 newPosition)
        {
            return new Vector2(
                  x: MathHelper.Clamp(
                        newPosition.X, 
                        LevelBounds.Left + (PhysicsCollisionBox.Width / 2) + levelWallDistance,
                        LevelBounds.Right - (PhysicsCollisionBox.Width / 2) - levelWallDistance),
                  y: MathHelper.Clamp(
                        newPosition.Y,
                        LevelBounds.Top + (PhysicsCollisionBox.Height / 2) + levelWallDistance, 
                        LevelBounds.Bottom - (PhysicsCollisionBox.Width / 2) - levelWallDistance));
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        internal void ReceiveCommand(Networking.Commands.GameObjectCommand command)
        {
            throw new NotImplementedException();
        }
    }
}