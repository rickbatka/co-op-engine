using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.World.Level;
using Microsoft.Xna.Framework;

namespace co_op_engine.Pathing
{
    //series of points weighted with things
    public class PathingGrid
    {
        private GridNode[][] nodes;

        public void UpdateGrid(float nodeSpacing, Rectangle worldSpace, List<MetaObstacle> obstacles)
        {
        }

        /// <summary>
        /// clears nodes of any previous path specific information
        /// </summary>
        public void PrepForPath()
        {
            throw new NotImplementedException();
        }

        public GridNode GetNodeAt(int x, int y)
        {
            throw new NotImplementedException();
        }

        public GridNode RoundToNearestNode(Vector2 location)
        {
            throw new NotImplementedException();
        }

        public GridNode GetNodeAt(Point position)
        {
            return GetNodeAt(position.X, position.Y);
        }

        public Point GetCoordForNode(GridNode node)
        {
            throw new NotImplementedException();
        }
    }
}
