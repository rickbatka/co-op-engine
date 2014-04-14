using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using Microsoft.Xna.Framework;

namespace co_op_engine.Pathing
{
    public class PathFinder
    {
        SortedSet<GridNode> openList; //woot sorted list is a log n retrieval with sorted values (beats heap of nlog n)
        SortedList<int,GridNode> closedList;

        public PathFinder()
        {
        }

        public Path GetPath(Vector2 start, Vector2 destination)
        {
            //round destination and start to nearest node

            //prepare/reset grid for new run

            //insert start node into open

            //looping until bumped into target node:
            //  find lowest cost in open list, move to closed list
            //  for each adjacent square
            //      if it's not on the open list, add it and point it to this one
            //      it it's on the open list and this G is better than it's G, point it at this one

            throw new NotImplementedException();
        }

        private class NodeComparer : IComparer<GridNode>
        {
            public int Compare(GridNode x, GridNode y)
            {
                return (int)(x.FCost() - y.FCost() * 10f);
            }
        }
    }
}
