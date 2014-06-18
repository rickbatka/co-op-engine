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
            var props = new ActorState[12];

            props[Constants.ACTOR_STATE_IDLE] = new ActorState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiateSkills: true,
                isAttacking: false,
                canStartDying: true,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                isBoosting: false,
                canMove: true
            );

            props[Constants.ACTOR_STATE_WALKING] = new ActorState(
                canInitiateIdleState: true,
                canInitiateWalkingState: true,
                canInitiateSkills: true,
                isAttacking: false,
                canStartDying: true,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                isBoosting: false,
                canMove: true
            );

            props[Constants.ACTOR_STATE_DYING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: true,
                canBuild: false,
                isVulnerable: false,
                isBoosting: false,
                canMove: false
            );

            props[Constants.ACTOR_STATE_DEAD] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: false,
                isBoosting: false,
                canMove: true
            );

            props[Constants.ACTOR_STATE_PLACING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: true,
                isVulnerable: true,
                isBoosting: false,
                canMove: true
            );

            props[Constants.ACTOR_STATE_BEING_HURT] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: false,
                isBoosting: false,
                canMove: false
            );

            props[Constants.ACTOR_STATE_ATTACKING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: true,
                canStartDying: true,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                isBoosting: false,
                canMove: false
            );

            props[Constants.ACTOR_STATE_BOOSTING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: true,
                isBoosting: true,
                canMove: true
            );

            props[Constants.ACTOR_STATE_RAGING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: false,
                isBoosting: false,
                canMove: false
            );

            props[Constants.ACTOR_STATE_CASTING] = new ActorState(
                canInitiateIdleState: false,
                canInitiateWalkingState: false,
                canInitiateSkills: false,
                isAttacking: false,
                canStartDying: false,
                canFinishDying: false,
                canBuild: false,
                isVulnerable: false,
                isBoosting: false,
                canMove: false
            );

            return props;
        }

    }

    public struct ActorState
    {
        public bool CanInitiateIdleState;
        public bool CanInitiateWalkingState;
        public bool CanInitiateSkills;
        public bool IsAttacking;
        public bool CanStartDying;
        public bool CanFinishDying;
        public bool CanBuild;
        public bool IsVulnerable;
        public bool IsBoosting;
        public bool CanMove;


        public ActorState(bool canInitiateIdleState, bool canInitiateWalkingState, 
            bool canInitiateSkills, bool isAttacking, bool canStartDying, 
            bool canFinishDying, bool canBuild, bool isVulnerable, bool isBoosting,
            bool canMove)
        {
            CanInitiateIdleState = canInitiateIdleState;
            CanInitiateWalkingState = canInitiateWalkingState;
            CanInitiateSkills = canInitiateSkills;
            IsAttacking = isAttacking;
            CanStartDying = canStartDying;
            CanFinishDying = canFinishDying;
            CanBuild = canBuild;
            IsVulnerable = isVulnerable;
            IsBoosting = isBoosting;
            CanMove = canMove;
        }
    }
}
