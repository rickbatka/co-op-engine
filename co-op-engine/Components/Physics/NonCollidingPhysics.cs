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
        { }
        override public void Draw(SpriteBatch spriteBatch)
        { }

    }
}
