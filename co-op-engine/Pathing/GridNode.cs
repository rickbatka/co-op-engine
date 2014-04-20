using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Pathing
{
    public class GridNode
    {
        /* * * * * * * * * * * * * * * * * * * * * * * * * ** 
         * Not to repeat the problems experienced in the    *       
         * first draft of physics, everything will have     *
         * units:                                           *
         *                                                  *
         * Severity: magnitude, units of (F)                *
         * Time: in milliseconds, used in decay(T)          *
         * * * * * * * * * * * * * * * * * * * * * * * * * **/
        public Point LocationInGrid = Point.Zero;
        /// <summary>
        /// this is used when there is a destroyable object in the path
        /// </summary>
        private int Adjustment_TimeToTraverse = 0;   // F 
        private int Decay_TimeToTraverse = 1000;  // F/ms(t) 

        /// <summary>
        /// offset for recently pathed nodes, weighted for 
        /// their distance from the source object, used to
        /// avoid collisions in paths
        /// </summary>
        private int Adjustment_RecentPath = 0;   //F
        private int Decay_RecentPath = 1000;  //F/ms(t)

        /// <summary>
        /// recent deaths near this node tell ai to stay clear
        /// </summary>
        private int Adjustment_RecentDeath = 0;  //F
        private int Decay_RecentDeath = 1000; // F/ms(t)

        public int CurrentDistance_G = 0;
		public GridNode Target { get; private set; }

		/// <summary>
		/// the G adjustment for this node taking into account pathing, traversal time, and recent deaths in the area
		/// </summary>
        public int GAdjustment()
        {
            return Adjustment_TimeToTraverse + Adjustment_RecentPath + Adjustment_RecentDeath;
        }
		
		public int CurrentG()
		{
            return CurrentDistance_G + GAdjustment();
		}
		
        public void ApplyAdjustment(int objectInPath = 0, int recentPath = 0, int recentDeath = 0)
        {
            Adjustment_TimeToTraverse += objectInPath;
            Adjustment_RecentPath += recentPath;
            Adjustment_RecentDeath += recentDeath;
        }
		
		public void SetTrace(GridNode target, int G)
		{
			Target = target;
            CurrentDistance_G = G;
		}
    }
}
