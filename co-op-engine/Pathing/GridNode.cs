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

        private int _g;
        public int G
        {
            get
            {
                return _g + currentAdjustment;
            }
            set
            {
                _g = value;
            }
        }


        public int H = 0;
        public int F { get { return G +H; } }

        private int currentAdjustment = 0;

        public GridNode Target { get; private set; }

        public void ApplyAdjustment(int adjustment)
        {
            currentAdjustment += adjustment;
        }

        public void SetTrace(GridNode target, int g, int h)
        {
            Target = target;
            this.G = g;
            H = h;
        }

        public void ClearAdjustments()
        {
            currentAdjustment = 0;
        }
    }
}
