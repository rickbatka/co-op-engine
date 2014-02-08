using co_op_engine.Components.Conduct;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Conduct
{
    /// <summary>
    /// houses the logic of the object, how it behaves thinks
    /// and makes decisions.  has all events relating to decision
    /// making and input.
    /// </summary>
    abstract class BrainBase : IIntelligent
    {
        protected GameObject owner;
        //movement direction vect
        //object state manager? (maybe make it generic enough for all game objects

        //events

        //constructor takes dataobject to populate values, could be aggregate like a 
        // higher level dataobject with a base dataobject component passed through

        public BrainBase(GameObject owner)
        {
            this.owner = owner;
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
    }
}
