using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Utility;
using co_op_engine.World.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Pathing
{
    public class PathFinder
    {
        #region Singleton boilerplate
        public static PathFinder Instance;
        public static void Initialize(ObjectContainer container)
        {
            Instance = new PathFinder(container);
        }
        #endregion

        private List<GridNode> openList; //woot sorted list is a log n retrieval with sorted values (beats heap of nlog n), can't sort on demand :(
        private List<GridNode> closedList;
        private PathingGrid grid;
        private ObjectContainer containerRef;
        private int GridSpacing = 20;

        private int TESTING_LAST_PATH_G = 0;

        private PathFinder(ObjectContainer container)
        {
            openList = new List<GridNode>();
            closedList = new List<GridNode>();
            containerRef = container;
            grid = new PathingGrid();
        }

        public Path GetPath(Vector2 startPosition, Vector2 endPosition, Rectangle collisionBox)
        {
            openList.Clear();
            closedList.Clear();

            //round destination and start to nearest node
            GridNode startNode = grid.RoundToNearestNode(startPosition);
            GridNode endNode = grid.RoundToNearestNode(endPosition);

            if (startNode == endNode)
            {
                return new Path(new List<Vector2>() { startPosition, endPosition });
            }

            //prepare/reset grid for new run
            grid.PrepForPath(collisionBox);

            //insert start node into open
            openList.Add(startNode);

            //looping until bumped into target node:
            bool finished = false;
            while (!finished)
            {
                //  find lowest cost in open list, move to closed list
#warning needs sort method here
                openList.Sort(comparer);
                GridNode currentNode = openList.First();
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                //  for each adjacent square
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        int checkX = currentNode.LocationInGrid.X + x;
                        int checkY = currentNode.LocationInGrid.Y + y;
                        if (!grid.WithinBounds(checkX, checkY))
                        {
                            continue;
                        }

                        GridNode checkNode = grid.GetNodeAt(checkX, checkY);
                        if (!closedList.Contains(checkNode))
                        {
                            if (checkNode == endNode)
                            {
                                checkNode.SetTrace(currentNode, 0, GetHeuristic(checkNode, endPosition));
                                TESTING_LAST_PATH_G = currentNode.G;
                                finished = true;
                            }
                            //if it's not on the open list, add it and point it to this one
                            else if (!openList.Contains(checkNode))
                            {
                                checkNode.SetTrace(currentNode, currentNode.G + GetMovementCost(x, y), GetHeuristic(checkNode, endPosition));
                                openList.Add(checkNode);
                            }
                            //if it's on the open list and this G is better than it's G, point it at this one
                            else if (currentNode.G + GetMovementCost(x, y) < checkNode.G)
                            {
                                checkNode.SetTrace(currentNode, currentNode.G + GetMovementCost(x, y), GetHeuristic(checkNode, endPosition));
                            }
                        }
                    }
                }
            }

            return ConstructPath(endNode);
        }

        private List<int> DebugHelperTraceThingy(List<GridNode> nodes)
        {
            return nodes.Select(n => n.F).ToList();
        }

        private int GetHeuristic(GridNode node, Vector2 endPoint)
        {
            return (int)Vector2.Distance(grid.GetPositionFromNode(node), endPoint)*10;
        }

        private Path ConstructPath(GridNode endPoint)
        {
            List<Vector2> positions = new List<Vector2>();

            GridNode currentNode = endPoint;

            bool done = false;
            while (!done)
            {
                positions.Insert(0, grid.GetPositionFromNode(currentNode));

                if (currentNode.Target != null)
                {
                    currentNode = currentNode.Target;
                }
                else
                {
                    done = true;
                }
            }

            return new Path(positions);
        }

        private int GetMovementCost(int x, int y)
        {
            return x * y == 0 ? 100 : 141;
        }

        public void ReceiveSnapshot(List<MetaObstacle> obstacles, Rectangle worldSize)
        {
            grid.UpdateGrid(GridSpacing, worldSize, obstacles);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            grid.Draw(spriteBatch);
            spriteBatch.DrawString(AssetRepository.Instance.Arial, TESTING_LAST_PATH_G.ToString(), new Vector2(500, 500), Color.Red);
        }

        private NodeComparer comparer = new NodeComparer();
        private class NodeComparer : IComparer<GridNode>
        {
            public int Compare(GridNode x, GridNode y)
            {
                return (x.F + -y.F);
            }
        }
    }
}
