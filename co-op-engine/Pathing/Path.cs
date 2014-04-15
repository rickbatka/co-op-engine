using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Pathing
{
    public class Path
    {
        //a series of positions and possibly
        public Vector2 CurrentPoint;
        public List<Vector2> Points;

        public Path(List<Vector2> points)
        {
            Points = points;
            AdvancePoint();
        }

        public bool AdvancePoint()
        {
            CurrentPoint = Points.First();
            Points.Remove(CurrentPoint);
            return Points.Count() != 0;
        }
    }
}
