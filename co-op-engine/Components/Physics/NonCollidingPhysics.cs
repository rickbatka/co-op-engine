using co_op_engine.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    class NonCollidingPhysics : PhysicsBase
    {

        public NonCollidingPhysics(GameObject owner)
            : base(owner)
        { }

        override public void Update(GameTime gameTime)
        {
            owner.CurrentQuad.NotfyOfMovement(owner);
            owner.BoundingBox.X = (int)(owner.Position.X - owner.Width / 2);
            owner.BoundingBox.Y = (int)(owner.Position.Y - owner.Height / 2);

            base.Update(gameTime);
        }
        override public void Draw(SpriteBatch spriteBatch)
        { }

    }
}
