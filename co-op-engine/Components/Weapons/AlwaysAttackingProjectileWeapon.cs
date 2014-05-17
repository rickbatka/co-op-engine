using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons
{
    public class AlwaysAttackingProjectileWeapon : Weapon
    {
        //this is a flying arrow, never stops attacking
        public override WeaponState CurrentWeaponStateProperties { get { return WeaponStates.States[Constants.WEAPON_STATE_ATTACKING_PRIMARY]; } }

        public AlwaysAttackingProjectileWeapon(GameObject owner)
            : base(owner) { }

        protected override void UpdateState(GameTime gameTime)
        {
            if(CurrentState != Constants.WEAPON_STATE_ATTACKING_PRIMARY)
            {
                //shouldnt happen
                CurrentState = Constants.WEAPON_STATE_ATTACKING_PRIMARY;
            }
        }
    }
}
