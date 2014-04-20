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
        private GridNode[,] nodes;
        private int nodeSpacing = 200;

        public void UpdateGrid(int nodeSpacing, Rectangle worldSpace, List<MetaObstacle> obstacles)
        {
            this.nodeSpacing = nodeSpacing;

            //initialize array size
            nodes = new GridNode[worldSpace.Width / nodeSpacing, worldSpace.Height / nodeSpacing];

            //loop and weigh
            for (int x = 0; x < nodes.GetLength(0); ++x)
            {
                for (int y = 0; y < nodes.GetLength(1); ++y)
                {
                    GridNode currentNode = nodes[x,y] = new GridNode();
                    Point currentNodeLocation = new Point(x*nodeSpacing, y*nodeSpacing);

                    //have x,y ref now
                    foreach (MetaObstacle obstacle in obstacles)
                    {
                        if(obstacle.bounds.Contains(currentNodeLocation))
                        {
                            currentNode.ApplyAdjustment(objectInPath: obstacle.pathingWeight);
                        }
                    }

                    currentNode.LocationInGrid = new Point(x, y);
                }
            }
        }

        /// <summary>
        /// clears nodes of any previous path specific information
        /// </summary>
        public void PrepForPath()
        {
            foreach (GridNode node in nodes)
            {
                node.SetTrace(null, 0);
            }
        }

        public GridNode GetNodeAt(int x, int y)
        {
            return nodes[x, y];
        }

        public GridNode RoundToNearestNode(Vector2 location)
        {
            return GetNodeAt(new Point((int)location.X / nodeSpacing, (int)location.Y / nodeSpacing));
        }

        public GridNode GetNodeAt(Point position)
        {
            return GetNodeAt(position.X, position.Y);
        }

        public bool WithinBounds(int x, int y)
        {
            return x >= 0 && x < nodes.GetLength(0) && y >= 0 && y < nodes.GetLength(1);
        }

        public Vector2 GetPositionFromNode(GridNode currentNode)
        {
            return new Vector2(currentNode.LocationInGrid.X * nodeSpacing, currentNode.LocationInGrid.Y * nodeSpacing);
        }
    }
}
