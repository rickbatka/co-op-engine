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
            owner.CurrentQuad.NotifyOfMovement(owner);
            
            base.Update(gameTime);
        }
        override public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
