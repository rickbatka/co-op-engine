﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Pathing
{
    class GridNode
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * ** 
         * Not to repeat the problems experienced in the    *       
         * first draft of physics, everything will have     *
         * units:                                           *
         *                                                  *
         * Severity: magnitude, units of (F)                *
         * Time: in milliseconds, used in decay(T)          *
         * * * * * * * * * * * * * * * * * * * * * * * * * **/

        /// <summary>
        /// this is used when there is a destroyable object in the path
        /// </summary>
        private float Adjustment_TimeToTraverse = 0f;   // F 
        private float Decay_TimeToTraverse = 1 / 1000;  // F/t 

        /// <summary>
        /// offset for recently pathed nodes, weighted for 
        /// their distance from the source object, used to
        /// avoid collisions in paths
        /// </summary>
        private float Adjustment_RecentPath = 0f;   //F  
        private float Decay_RecentPath = 1 / 1000;  //F/t

        /// <summary>
        /// recent deaths near this node tell ai to stay clear
        /// </summary>
        private float Adjustment_RecentDeath = 0f;  //F
        private float Decay_RecentDeath = 1 / 1000; // F/t

        public float CurrentDistance_F = 0f;

        public float FCost()
        {
            return Adjustment_TimeToTraverse + Adjustment_RecentPath + Adjustment_RecentDeath;
        }

        public void ApplyAdjustment(float objectInPath = 0f, float recentPath = 0f, float recentDeath = 0f)
        {
            Adjustment_TimeToTraverse += objectInPath;
            Adjustment_RecentPath += recentPath;
            Adjustment_RecentDeath += recentDeath;
        }
    }
}