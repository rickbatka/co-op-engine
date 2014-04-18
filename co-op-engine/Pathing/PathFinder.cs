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
        List<GridNode> closedList;
		PathingGrid grid;
		
        public PathFinder()
        {
			grid = new PathingGrid();
        }

        public Path GetPath(Vector2 startPosition, Vector2 endPosition)
        {
			openList.Clear();
			closedList.Clear();
		
            //round destination and start to nearest node
			GridNode startNode = grid.RoundToNearest(startPosition);
			GridNode endNode = grid.RoundToNearest(endPosition);

            //prepare/reset grid for new run
			grid.PrepForPath();

            //insert start node into open
			openList.Add(startNode);

            //looping until bumped into target node:
			bool finished = false;
			while(!finished)
			{
            //  find lowest cost in open list, move to closed list
#warning needs sort method here
				GridNode currentNode = openList.First();
				openList.Remove(currentNode);
				closedList.Add(currentNode);
				Point currentNodePosition = grid.GetLocationOfNode(currentNode);
				
            //  for each adjacent square
				for(int x=-1; x<=1; ++x)
				{
					for(int y=-1; y<=1; ++y)
					{
						GridNode checkNode = grid.GetNodeAt(currentNodePosition.X, currentNodePosition.Y);
            
						//if it's not on the open list, add it and point it to this one
						if(!openList.Contains(checkNode))
						{
							checkNode.SetTrace(currentNode, Vector2.Distance(Vector2.Zero,new Vector2(x,y)));
						}
						else  //if it's on the open list and this G is better than it's G, point it at this one
						{
							
						}
					}
				}
			}

            throw new NotImplementedException();
        }
		
		
		
		public void ReceiveSnapshot()
		{
			//receives quadtree and projects it onto the grid's metadata
		}
		
		//might come in handy later, but for not it's a waste
        /*private class NodeComparer : IComparer<GridNode>
        {
            public int Compare(GridNode x, GridNode y)
            {
                return (int)(x.FCost() - y.FCost() * 10f);
            }
        }*/
    }
}
