using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    public delegate void PhysicsCollideEventHandler(GameObject sender, List<GameObject> collidors);
    class CollidingPhysics : PhysicsBase
    {
        public event PhysicsCollideEventHandler OnCollision;

        private Vector2 previousPosition;

        public CollidingPhysics(GameObject owner, Rectangle levelBounds)
            : base(owner, levelBounds)
        {
            previousPosition = owner.Position;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime); //moved

            var colliders = owner.CurrentQuad.MasterQuery(PhysicsCollisionBox);

            if (!owner.UsedInPathing && colliders.Any(u => u != this.owner)) //probably a more efficient check
            {
                //there is a collision
                colliders.Remove(owner);
                if (OnCollision != null)
                {
                    OnCollision(this.owner, colliders);
                }
                HandleCollision(colliders);
            }
            else if (previousPosition != owner.Position)
            {
                owner.CurrentQuad.NotifyOfMovement(owner);
            }
            previousPosition = owner.Position;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) 
        {
            base.Draw(spriteBatch);
        }

        public override void DebugDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.DebugDraw(spriteBatch);
            spriteBatch.Draw(
                texture: AssetRepository.Instance.DebugGridTexture,
                destinationRectangle: PhysicsCollisionBox, 
                sourceRectangle: null,
                color: Color.Red,
                rotation: 0f,
                origin: Vector2.Zero,
                effect: SpriteEffects.None,
                depth: 1f
            );

            owner.CurrentQuad.Draw(spriteBatch);

            //if (!owner.CurrentQuad.DEBUGEXPOSURE_DONOTUSE().Contains(owner))
            //{
            //    spriteBatch.Draw(AssetRepository.Instance.DebugFillTexture, owner.BoundingBox, Color.Yellow);
            //}
        }

        private void HandleCollision(List<GameObject> colliders)
        {
            GameObject biggest = colliders[0];
            Point biggestOverlap = Point.Zero;
            foreach (var obj in colliders)
            {
                var left = obj.PhysicsCollisionBox.Left > this.PhysicsCollisionBox.Left ? obj.PhysicsCollisionBox.Left : this.PhysicsCollisionBox.Left;
                var right = obj.PhysicsCollisionBox.Right > this.PhysicsCollisionBox.Right ? this.PhysicsCollisionBox.Right : obj.PhysicsCollisionBox.Right;
                var top = obj.PhysicsCollisionBox.Top > this.PhysicsCollisionBox.Top ? obj.PhysicsCollisionBox.Top : this.PhysicsCollisionBox.Top;
                var bottom = obj.PhysicsCollisionBox.Bottom > this.PhysicsCollisionBox.Bottom ? this.PhysicsCollisionBox.Bottom : obj.PhysicsCollisionBox.Bottom;

                var overlap = new Point(Math.Abs((int)(left - right)), Math.Abs((int)(top - bottom)));

                //var currentPoint = new Point(
                //    -(Math.Abs(obj.BoundingBox.X - owner.BoundingBox.X) - ((owner.BoundingBox.Width + obj.BoundingBox.Width) / 2)),
                //    -(Math.Abs(obj.BoundingBox.Y - owner.BoundingBox.Y) - ((owner.BoundingBox.Height + obj.BoundingBox.Height) / 2)));

                if ((biggestOverlap.X * biggestOverlap.Y) <= (overlap.X * overlap.Y))
                {
                    biggestOverlap = overlap;
                    biggest = obj;
                }
            }

            int xOverlap = biggestOverlap.X;
            int yOverlap = biggestOverlap.Y;

            Vector2 newOwnerBoundingBoxPos = new Vector2(this.PhysicsCollisionBox.Center.X, this.PhysicsCollisionBox.Center.Y);
            Vector2 newBiggestBoundingBoxPos = new Vector2(biggest.PhysicsCollisionBox.Center.X, biggest.PhysicsCollisionBox.Center.Y);

            int mod = biggest.UsedInPathing ? 1 : 2;
            if (xOverlap < yOverlap)
            {
                if (this.PhysicsCollisionBox.Center.X < biggest.PhysicsCollisionBox.Center.X)
                {
                    newOwnerBoundingBoxPos.X -= xOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestBoundingBoxPos.X += xOverlap / mod;
                    }
                }
                else
                {
                    newOwnerBoundingBoxPos.X += xOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestBoundingBoxPos.X -= xOverlap / mod;
                    }
                }
            }
            else
            {
                if (this.PhysicsCollisionBox.Center.Y < biggest.PhysicsCollisionBox.Center.Y)
                {
                    newOwnerBoundingBoxPos.Y -= yOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestBoundingBoxPos.Y += yOverlap / mod;
                    }
                }
                else
                {
                    newOwnerBoundingBoxPos.Y += yOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestBoundingBoxPos.Y -= yOverlap / mod;
                    }
                }
            }

            Vector2 ownerMovement = newOwnerBoundingBoxPos - new Vector2(this.PhysicsCollisionBox.Center.X, this.PhysicsCollisionBox.Center.Y);
            Vector2 biggestMovement = newBiggestBoundingBoxPos - new Vector2(biggest.PhysicsCollisionBox.Center.X, biggest.PhysicsCollisionBox.Center.Y);

            owner.Position += ownerMovement;
            biggest.Position += biggestMovement;

            owner.CurrentQuad.NotifyOfMovement(owner);
            this.VerifyBoundingBox();

            biggest.CurrentQuad.NotifyOfMovement(biggest);
            biggest.Physics.VerifyBoundingBox();
        }
    }
}
