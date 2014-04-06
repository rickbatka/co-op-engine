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
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttack: true,
                isAttacking: false
            );

            props[Constants.WEAPON_STATE_WALKING] = new WeaponState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttack: true,
                isAttacking: false
            );

            props[Constants.WEAPON_STATE_ATTACKING_PRIMARY] = new WeaponState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttack: true,
                isAttacking: true
            );

            props[Constants.WEAPON_STATE_DEAD] = new WeaponState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttack: false,
                isAttacking: false
            );

            return props;
        }

    }

    public struct WeaponState
    {
        public bool CanInitiateIdleState;
        public bool CanInitiateWalkingState;
        public bool CanInitiatePrimaryAttack;
        public bool IsAttacking;

        public WeaponState(bool canInitiateIdleState, bool canInitiateWalkingState, bool canInitiatePrimaryAttack, bool isAttacking)
        {
            CanInitiateIdleState = canInitiateIdleState;
            CanInitiateWalkingState = canInitiateWalkingState;
            CanInitiatePrimaryAttack = canInitiatePrimaryAttack;
            IsAttacking = isAttacking;
        }
    }
}
