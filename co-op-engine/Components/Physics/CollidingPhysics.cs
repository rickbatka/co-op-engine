using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using co_op_engine.Content;

namespace co_op_engine.Components.Physics
{
    public delegate void PhysicsCollideEventHandler(GameObject sender, List<GameObject> collidors);
    class CollidingPhysics : PhysicsBase
    {
        public event PhysicsCollideEventHandler OnCollision;

        public CollidingPhysics(GameObject owner)
            : base(owner)
        { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime); //moved

            var colliders = owner.CurrentQuad.MasterQuery(owner.BoundingBox);

            if (!owner.UnShovable && colliders.Any(u => u != this.owner)) //probably a more efficient check
            {
                //there is a collision
                colliders.Remove(owner);
                if (OnCollision != null)
                {
                    OnCollision(this.owner, colliders);
                }
                HandleCollision(colliders);
            }
            else
            {
                owner.CurrentQuad.NotfyOfMovement(owner);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) { }

        public override void DebugDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetRepository.Instance.DebugGridTexture, this.owner.BoundingBox, Color.White);
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

                if ((biggestOverlap.X * biggestOverlap.Y) < (currentPoint.X * currentPoint.Y))
                {
                    biggestOverlap = currentPoint;
                    biggest = obj;
                }
            }

            int xOverlap = biggestOverlap.X;
            int yOverlap = biggestOverlap.Y;

            int mod = biggest.UnShovable ? 1 : 2;
            if (xOverlap < yOverlap)
            {
                if (owner.Position.X < biggest.Position.X)
                {
                    owner.Position.X -= xOverlap / mod;
                    if (!biggest.UnShovable)
                    {
                        biggest.Position.X += xOverlap / mod;
                    }
                }
                else
                {
                    owner.Position.X += xOverlap / mod;
                    if (!biggest.UnShovable)
                    {
                        biggest.Position.X -= xOverlap / mod;
                    }
                }
            }
            else
            {
                if (owner.Position.Y < biggest.Position.Y)
                {
                    owner.Position.Y -= yOverlap / mod;
                    if (!biggest.UnShovable)
                    {
                        biggest.Position.Y += yOverlap / mod;
                    }
                }
                else
                {
                    owner.Position.Y += yOverlap / mod;
                    if (!biggest.UnShovable)
                    {
                        biggest.Position.Y -= yOverlap / mod;
                    }
                }
            }
            owner.CurrentQuad.NotfyOfMovement(owner);
            this.VerifyBoundingBox();

            biggest.CurrentQuad.NotfyOfMovement(biggest);
            biggest.physics.VerifyBoundingBox();
        }
    }
}
