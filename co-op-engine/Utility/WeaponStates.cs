using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public static class WeaponStates
    {
        public static WeaponState[] States = BuildStateProperties();

        private static WeaponState[] BuildStateProperties()
        {
            var props = new WeaponState[4];

            props[Constants.WEAPON_STATE_IDLE] = new WeaponState(
                canInitiatePrimaryAttack: true,
                isAttacking: false
            );

            props[Constants.WEAPON_STATE_WALKING] = new WeaponState(
                canInitiatePrimaryAttack: true,
                isAttacking: false
            );

            props[Constants.WEAPON_STATE_ATTACKING_PRIMARY] = new WeaponState(
                canInitiatePrimaryAttack: false,
                isAttacking: true
            );

            props[Constants.WEAPON_STATE_DEAD] = new WeaponState(
                canInitiatePrimaryAttack: false,
                isAttacking: false
            );

            return props;
        }

    }

    public struct WeaponState
    {
        public bool CanInitiatePrimaryAttack;
        public bool IsAttacking;

        public WeaponState(bool canInitiatePrimaryAttack, bool isAttacking)
        {
            CanInitiatePrimaryAttack = canInitiatePrimaryAttack;
            IsAttacking = isAttacking;
        }
    }
}
