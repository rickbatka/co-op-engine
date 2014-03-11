using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public static class StateProperties
    {
        public static StatePropertySet[] Properties = BuildStateProperties();

        private static StatePropertySet[] BuildStateProperties()
        {
            var props = new StatePropertySet[3];

            props[Constants.STATE_IDLE] = new StatePropertySet(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttackState: true,
                isAttacking: false
            );

            props[Constants.STATE_WALKING] = new StatePropertySet(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttackState: true,
                isAttacking: false
            );

            props[Constants.STATE_ATTACKING_MELEE] = new StatePropertySet(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: true
            );

            return props;
        }

    }

    public struct StatePropertySet
    {
        public bool CanInitiateIdleState;
        public bool CanInitiateWalkingState;
        public bool CanInitiatePrimaryAttackState;
        public bool IsAttacking;

        public StatePropertySet(bool canInitiateIdleState, bool canInitiateWalkingState, bool canInitiatePrimaryAttackState, bool isAttacking)
        {
            CanInitiateIdleState = canInitiateIdleState;
            CanInitiateWalkingState = canInitiateWalkingState;
            CanInitiatePrimaryAttackState = canInitiatePrimaryAttackState;
            IsAttacking = isAttacking;
        }
    }
}
