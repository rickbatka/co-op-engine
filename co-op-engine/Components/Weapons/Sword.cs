using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons
{
    public class Sword : WeaponBase
    {
        public Sword(GameObject owner)
            : base(owner)
        { }

        override public void Draw(SpriteBatch spriteBatch) 
        {
            base.Draw(spriteBatch);
        }

        override public void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
        }
    }
}
