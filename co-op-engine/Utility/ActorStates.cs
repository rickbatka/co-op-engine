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
            var props = new ActorState[7];

            props[Constants.ACTOR_STATE_IDLE] = new ActorState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttackState: true,
                isAttacking: false,
                canStartDying: true,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                canExceedSpeedLimit: false,
                canChangeRotation: true
            );

            props[Constants.ACTOR_STATE_WALKING] = new ActorState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiatePrimaryAttackState: true,
                isAttacking: false,
                canStartDying: true,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                canExceedSpeedLimit: false,
                canChangeRotation: true
            );

            props[Constants.ACTOR_STATE_DYING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: true,
                canBuild: false,
                isVulnerable: false,
                canExceedSpeedLimit: false,
                canChangeRotation: true
            );

            props[Constants.ACTOR_STATE_DEAD] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: false,
                canExceedSpeedLimit: false,
                canChangeRotation: true
            );

            props[Constants.ACTOR_STATE_PLACING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: true,
                isVulnerable: true,
                canExceedSpeedLimit: false,
                canChangeRotation: true
            );

            props[Constants.ACTOR_STATE_BEING_HURT] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: false,
                canExceedSpeedLimit: true,
                canChangeRotation: false
            );

            props[Constants.ACTOR_STATE_BOOSTING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiatePrimaryAttackState: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                canExceedSpeedLimit: true,
                canChangeRotation: false
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
        public bool CanStartDying;
        public bool CanFinishDying;
        public bool CanBuild;
        public bool IsVulnerable;
        public bool CanExceedSpeedLimit;
        public bool CanChangeRotation;


        public ActorState(bool canInitiateIdleState, bool canInitiateWalkingState, 
            bool canInitiatePrimaryAttackState, bool isAttacking, bool canStartDying, 
            bool canFinishDying, bool canBuild, bool isVulnerable, bool canExceedSpeedLimit,
            bool canChangeRotation)
        {
            CanInitiateIdleState = canInitiateIdleState;
            CanInitiateWalkingState = canInitiateWalkingState;
            CanInitiatePrimaryAttackState = canInitiatePrimaryAttackState;
            IsAttacking = isAttacking;
            CanStartDying = canStartDying;
            CanFinishDying = canFinishDying;
            CanBuild = canBuild;
            IsVulnerable = isVulnerable;
            CanExceedSpeedLimit = canExceedSpeedLimit;
            CanChangeRotation = canChangeRotation;
        }
    }
}
