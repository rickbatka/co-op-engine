using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.WeaponEngines
{
    class BasicAttackWithCooldownWeaponEngine : WeaponEngine
    {
        public BasicAttackWithCooldownWeaponEngine(Weapon weapon)
            :base(weapon)
        { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}
