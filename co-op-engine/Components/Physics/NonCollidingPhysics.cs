using co_op_engine.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Physics
{
    class NonCollidingPhysics : PhysicsBase, IPhysical
    {

        public NonCollidingPhysics(GameObject owner)
            : base(owner)
        { }

        override public void Update(GameTime gameTime)
        {
            CurrentQuad.NotfyOfMovement(owner);
            boundingBox.X = (int)(owner.Position.X - boundingBox.Width / 2);
            boundingBox.Y = (int)(owner.Position.Y - boundingBox.Height / 2);
        }
        override public void Draw(SpriteBatch spriteBatch)
        { }

    }
}
