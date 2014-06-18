using co_op_engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public static class Constants
    {
        public static readonly int South = 0;
        public static readonly int West = 1;
        public static readonly int North = 2;
        public static readonly int East = 3;

        public static readonly int ACTOR_STATE_IDLE = 0;
        public static readonly int ACTOR_STATE_WALKING = 1;
        public static readonly int ACTOR_STATE_DYING = 2;
        public static readonly int ACTOR_STATE_DEAD = 3;
        public static readonly int ACTOR_STATE_PLACING = 4;
        public static readonly int ACTOR_STATE_BEING_HURT = 5;
        public static readonly int ACTOR_STATE_ATTACKING = 6;
        public static readonly int ACTOR_STATE_BOOSTING = 7;
        public static readonly int ACTOR_STATE_RAGING = 8;
        public static readonly int ACTOR_STATE_CASTING = 9;

        public static readonly int PARTICLE_LIFETIME_MS = 100;

        public static readonly int BEING_HURT_EFFECT_TIME_MS = 200;
    }

    public class FireProjectileEventArgs : EventArgs
    {
        public GameObject TargetObject;
    }
}
