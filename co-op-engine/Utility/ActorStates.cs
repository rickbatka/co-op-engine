using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public static class ActorStates
    {
        public static ActorState[] States = BuildStateProperties();

        private static ActorState[] BuildStateProperties()
        {
            var props = new ActorState[3];

            props[Constants.ACTOR_STATE_IDLE] = new ActorState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttackState: true,
                isAttacking: false
            );

            props[Constants.ACTOR_STATE_WALKING] = new ActorState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttackState: true,
                isAttacking: false
            );

            props[Constants.ACTOR_STATE_ATTACKING_MELEE] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: true
            );

            return props;
        }

    }

    public struct ActorState
    {
        public bool CanInitiateIdleState;
        public bool CanInitiateWalkingState;
        public bool CanInitiatePrimaryAttackState;
        public bool IsAttacking;

        public ActorState(bool canInitiateIdleState, bool canInitiateWalkingState, bool canInitiatePrimaryAttackState, bool isAttacking)
        {
            CanInitiateIdleState = canInitiateIdleState;
            CanInitiateWalkingState = canInitiateWalkingState;
            CanInitiatePrimaryAttackState = canInitiatePrimaryAttackState;
            IsAttacking = isAttacking;
        }
    }
}
