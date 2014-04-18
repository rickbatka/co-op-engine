using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Pathing
{
    public class PathingGrid
    {
        //series of points weighted with things
        public void PrepForPath()
        {
            throw new NotImplementedException();
        }

        public GridNode GetNodeAt(int x, int y)
        {
            throw new NotImplementedException();
        }

        public GridNode RoundToNearest(Vector2 location)
        {
            throw new NotImplementedException();
        }

        public GridNode GetNodeAt(Point position)
        {
            return GetNodeAt(position.X, position.Y);
        }

        public Point GetLocationOfNode(GridNode node)
        {
            throw new NotImplementedException();
        }
    }
}
