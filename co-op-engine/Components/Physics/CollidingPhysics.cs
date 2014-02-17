using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
            if (colliders.Any(u => u != this.owner)) //probably a more efficient check
            {
                //there is a collision
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

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.DEBUG_GRID_TEXTURE, this.owner.BoundingBox, Color.White);
        }

        private void HandleCollision(List<GameObject> collidors)
        {
            //this is gonna be a depth resolution collision
            //only using first one for now, could use others later

            var collide = collidors.Where(u => u != this.owner).First();


            int xOverlap = -(Math.Abs(collide.BoundingBox.X - owner.BoundingBox.X) - ((owner.BoundingBox.Width + collide.BoundingBox.Width) / 2));
            int yOverlap = -(Math.Abs(collide.BoundingBox.Y - owner.BoundingBox.Y) - ((owner.BoundingBox.Height + collide.BoundingBox.Height) / 2));

            if (xOverlap < yOverlap)
            {
                if (owner.Position.X < collide.Position.X)
                {
                    owner.Position.X -= xOverlap / 2;
                    collide.Position.X += xOverlap / 2;
                }
                else
                {
                    owner.Position.X += xOverlap / 2;
                    collide.Position.X -= xOverlap / 2;
                }
            }
            else
            {
                if (owner.Position.Y < collide.Position.Y)
                {
                    owner.Position.Y -= yOverlap / 2;
                    collide.Position.Y += yOverlap / 2;
                }
                else
                {
                    owner.Position.Y += yOverlap / 2;
                    collide.Position.X -= yOverlap / 2;
                }
            }
            owner.CurrentQuad.NotfyOfMovement(owner);
            collide.CurrentQuad.NotfyOfMovement(collide);
        }
    }
}
