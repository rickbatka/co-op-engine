using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.WeaponEngines
{
    public class WeaponEngine
    {
        protected Weapon Weapon;
        protected GameObject Owner;

        public WeaponEngine(Weapon weapon)
        {
            this.Weapon = weapon;
        }

        virtual public void Draw(SpriteBatch spriteBatch) { }

        virtual public void DebugDraw(SpriteBatch spriteBatch) { }

        virtual public void Update(GameTime gameTime) { }
    }
}
