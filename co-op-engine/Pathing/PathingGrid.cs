using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using co_op_engine.World.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Pathing
{
    //series of points weighted with things
    public class PathingGrid
    {
        private GridNode[,] nodes;
        private int nodeSpacing = 200;
        private List<MetaObstacle> currentObstacles;

        private int pendingNodeSpacing;
        private Rectangle pendingWorldSpace;
        private List<MetaObstacle> pendingObstacles;

        public PathingGrid()
        {
            nodes = new GridNode[1, 1];
        }

        public void UpdateGrid(int nodeSpacing, Rectangle worldSpace, List<MetaObstacle> obstacles)
        {
            pendingNodeSpacing = nodeSpacing;
            pendingWorldSpace = worldSpace;
            pendingObstacles = obstacles;
        }

        private void BuildGridFromSnapshot()
        {
            if (pendingNodeSpacing != 0 && pendingWorldSpace != Rectangle.Empty && pendingObstacles != null)
            {
                nodeSpacing = pendingNodeSpacing;

                //initialize array size
                nodes = new GridNode[pendingWorldSpace.Width / nodeSpacing, pendingWorldSpace.Height / nodeSpacing];

                currentObstacles = pendingObstacles;

                ////loop and weigh
                for (int x = 0; x < nodes.GetLength(0); ++x)
                {
                    for (int y = 0; y < nodes.GetLength(1); ++y)
                    {
                        GridNode currentNode = nodes[x, y] = new GridNode();
                        currentNode.LocationInGrid = new Point(x, y);
                    }
                }

                pendingObstacles = null;
                pendingWorldSpace = Rectangle.Empty;
                pendingNodeSpacing = 0;
            }
        }

        /// <summary>
        /// clears nodes of any previous path specific information
        /// </summary>
        public void PrepForPath(Rectangle physBox)
        {
            BuildGridFromSnapshot();

            physBox.Inflate(physBox.Width / 2, physBox.Height / 2);

            foreach (GridNode node in nodes)
            {
                node.SetTrace(null, 0, 0);
                node.ClearAdjustments();

                foreach (MetaObstacle obstacle in currentObstacles)
                {
                    if (obstacle.bounds.Intersects(CenterOnNode(node, physBox)))
                    {
                        node.ApplyAdjustment(obstacle.pathingWeight);
                    }
                }
            }
        }

        private Rectangle CenterOnNode(GridNode node, Rectangle centerer)
        {
            Vector2 position = GetPositionFromNode(node);
            return new Rectangle((int)(position.X - centerer.Width / 2), (int)(position.Y - centerer.Height / 2), centerer.Width, centerer.Height);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GridNode node in nodes)
            {
                if (node != null && node.Target != null)
                {
                    DrawingUtility.DrawLine(GetPositionFromNode(node), GetPositionFromNode(node.Target), spriteBatch, Color.White);
                }
            }
        }

        public Vector2 GetPositionFromNode(GridNode currentNode)
        {
            return new Vector2(currentNode.LocationInGrid.X * nodeSpacing, currentNode.LocationInGrid.Y * nodeSpacing);
        }
    }
}
