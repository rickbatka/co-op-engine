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

        public CollidingPhysics(GameObject owner)
            : base(owner)
        {
            previousPosition = owner.Position;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime); //moved

            var colliders = owner.CurrentQuad.MasterQuery(owner.BoundingBox);

            if (!owner.UsedInPathing && colliders.Any(u => u != this.owner)) //probably a more efficient check
            {
                //there is a collision
                colliders.Remove(owner);
                if (OnCollision != null)
                {
                    OnCollision(this.owner, colliders);
                }
                HandleCollisionPatternMethodTesting(colliders);
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
                destinationRectangle: owner.BoundingBox, 
                sourceRectangle: null,
                color: Color.Red,
                rotation: 0f,
                origin: Vector2.Zero,
                effect: SpriteEffects.None,
                depth: 1f
            );

            owner.CurrentQuad.Draw(spriteBatch);

            if (!owner.CurrentQuad.DEBUGEXPOSURE_DONOTUSE().Contains(owner))
            {
                spriteBatch.Draw(AssetRepository.Instance.DebugFillTexture, owner.BoundingBox, Color.Yellow);
            }
        }

        private void HandleCollisionPatternMethodTesting(List<GameObject> colliders)
        {
            GameObject biggest = colliders[0];
            Point biggestOverlap = Point.Zero;
            foreach (var obj in colliders)
            {
                var left = obj.BoundingBox.Left > owner.BoundingBox.Left ? obj.BoundingBox.Left : owner.BoundingBox.Left;
                var right = obj.BoundingBox.Right > owner.BoundingBox.Right ? owner.BoundingBox.Right : obj.BoundingBox.Right;
                var top = obj.BoundingBox.Top > owner.BoundingBox.Top ? obj.BoundingBox.Top : owner.BoundingBox.Top;
                var bottom = obj.BoundingBox.Bottom > owner.BoundingBox.Bottom ? owner.BoundingBox.Bottom : obj.BoundingBox.Bottom;

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

            Vector2 newOwnerBoundingBoxPos = new Vector2(owner.BoundingBox.Center.X, owner.BoundingBox.Center.Y);
            Vector2 newBiggestBoundingBoxPos = new Vector2(biggest.BoundingBox.Center.X, biggest.BoundingBox.Center.Y);

            int mod = biggest.UsedInPathing ? 1 : 2;
            if (xOverlap < yOverlap)
            {
                if (owner.BoundingBox.Center.X < biggest.BoundingBox.Center.X)
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
                if (owner.BoundingBox.Center.Y < biggest.BoundingBox.Center.Y)
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

            Vector2 ownerMovement = newOwnerBoundingBoxPos - new Vector2(owner.BoundingBox.Center.X, owner.BoundingBox.Center.Y);
            Vector2 biggestMovement = newBiggestBoundingBoxPos - new Vector2(biggest.BoundingBox.Center.X, biggest.BoundingBox.Center.Y);

            owner.Position += ownerMovement;
            biggest.Position += biggestMovement;

            owner.CurrentQuad.NotifyOfMovement(owner);
            this.VerifyBoundingBox();

            biggest.CurrentQuad.NotifyOfMovement(biggest);
            biggest.Physics.VerifyBoundingBox();
        }

        private void HandleCollision(List<GameObject> collidors)
        {
            //this is gonna be a depth resolution collision
            //only using first one for now, could use others later

            //take largest collider and resolve it only, this should remove problem of getting stuck 
            // on static walls because you will always be colliding more with the closest wall, 
            // correctly resolving the collision.

            GameObject biggest = collidors[0];
            Point biggestOverlap = Point.Zero;
            foreach (var obj in collidors)
            {
                var currentPoint = new Point(
                    -(Math.Abs(obj.BoundingBox.X - owner.BoundingBox.X) - ((owner.BoundingBox.Width + obj.BoundingBox.Width) / 2)),
                    -(Math.Abs(obj.BoundingBox.Y - owner.BoundingBox.Y) - ((owner.BoundingBox.Height + obj.BoundingBox.Height) / 2)));

                if ((biggestOverlap.X * biggestOverlap.Y) <= (currentPoint.X * currentPoint.Y))
                {
                    biggestOverlap = currentPoint;
                    biggest = obj;
                }
            }

            int xOverlap = biggestOverlap.X;
            int yOverlap = biggestOverlap.Y;

            Vector2 newOwnerPos = new Vector2(owner.Position.X, owner.Position.Y);
            Vector2 newBiggestPos = new Vector2(biggest.Position.X, biggest.Position.Y);

            int mod = biggest.UsedInPathing ? 1 : 2;
            if (xOverlap < yOverlap)
            {
                if (owner.Position.X < biggest.Position.X)
                {
                    newOwnerPos.X -= xOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestPos.X += xOverlap / mod;
                    }
                }
                else
                {
                    newOwnerPos.X += xOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestPos.X -= xOverlap / mod;
                    }
                }
            }
            else
            {
                if (owner.Position.Y < biggest.Position.Y)
                {
                    newOwnerPos.Y -= yOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestPos.Y += yOverlap / mod;
                    }
                }
                else
                {
                    newOwnerPos.Y += yOverlap / mod;
                    if (!biggest.UsedInPathing)
                    {
                        newBiggestPos.Y -= yOverlap / mod;
                    }
                }
            }
            owner.Position = newOwnerPos;
            biggest.Position = newBiggestPos;

            owner.CurrentQuad.NotifyOfMovement(owner);
            this.VerifyBoundingBox();

            biggest.CurrentQuad.NotifyOfMovement(biggest);
            biggest.Physics.VerifyBoundingBox();
        }
    }
}
