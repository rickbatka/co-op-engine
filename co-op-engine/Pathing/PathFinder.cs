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
using System.Threading;

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

        private ThreadSafeBuffer<PathRequest> pathRequests;
        private Thread pathingThread;

        private List<GridNode> openList; //woot sorted list is a log n retrieval with sorted values (beats heap of nlog n), can't sort on demand :(
        private List<GridNode> closedList;
        private List<LengthyRequestContainer> requeuedRequests;
        private PathingGrid grid;
        private ObjectContainer containerRef;
        private int GridSpacing = 20;
        private int lengthyPathThreshhold = 200;
        private bool ShuttingDown = false;

        private int TESTING_LAST_PATH_G = 0;
        private int TESTING_LAST_PATH_LENGTH = 0;

        private PathFinder(ObjectContainer container)
        {
            openList = new List<GridNode>();
            closedList = new List<GridNode>();
            requeuedRequests = new List<LengthyRequestContainer>();
            containerRef = container;
            grid = new PathingGrid();
            pathRequests = new ThreadSafeBuffer<PathRequest>();
            pathingThread = new Thread(new ThreadStart(PathingLoop));
            pathingThread.IsBackground = true;
            pathingThread.Start();
        }

        public void ShutDownPathing()
        {
            ShuttingDown = true; //will catch it if there are issues with the abort call;
            pathingThread.Abort();
        }

        /// <summary>
        /// Be sure your callback is quick and threadsafe, it won't be getting called on the same thread it enters
        /// </summary>
        /// <param name="callback">ENSURE THIS IS QUICK AND THREADSAFE!!!!</param>
        public static void RequestPath(Vector2 startPosition, Vector2 endPosition, Rectangle collisionBox, Action<Path> callback)
        {
            Instance.pathRequests.Add(new PathRequest(startPosition, endPosition, collisionBox, callback));
        }

        private void PathingLoop()
        {
            while (!ShuttingDown)
            {
                List<PathRequest> requests = pathRequests.Gather();

                if (requests.Count == 0 && requeuedRequests.Count == 0)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    List<LengthyRequestContainer> slowAgainRequests = new List<LengthyRequestContainer>();

                    foreach (LengthyRequestContainer requeuedRequest in requeuedRequests)
                    {
                        Path path = ContinuePath(requeuedRequest.Request.startPosition, requeuedRequest.Request.endPosition, requeuedRequest.Request.collisionBox, requeuedRequest.Nodes,requeuedRequest.OpenList, requeuedRequest.ClosedList);
                        if (path != null)
                        {
                            requeuedRequest.Request.callback(path);
                        }
                        else
                        {
                            slowAgainRequests.Add(new LengthyRequestContainer(
                                grid.GetNodesForStorage(),
                                requeuedRequest.Request,
                                openList,
                                closedList));
                        }
                    }

                    requeuedRequests = slowAgainRequests;

                    foreach (PathRequest request in requests)
                    {
                        Path path = BuildNewPath(request.startPosition, request.endPosition, request.collisionBox);

                        if (path != null)
                        {
                            request.callback(path);
                        }
                        else
                        {
                            requeuedRequests.Add(new LengthyRequestContainer(
                                grid.GetNodesForStorage(),
                                request,
                                openList,
                                closedList));
                        }
                    }
                }
            }
        }

        private Path ContinuePath(Vector2 startPosition, Vector2 endPosition, Rectangle collisionBox, GridNode[,] savedNodes, List<GridNode> open, List<GridNode> closed)
        {
            openList = open;
            closedList = closed;

            grid.LoadNodes(savedNodes);
            grid.PrepForPath(collisionBox, true);

            GridNode startNode = grid.RoundToNearestNode(startPosition);
            GridNode endNode = grid.RoundToNearestNode(endPosition);

            return ConstructionLoop(startNode, endNode, startPosition, endPosition);
        }

        private Path BuildNewPath(Vector2 startPosition, Vector2 endPosition, Rectangle collisionBox)
        {
            openList.Clear();
            closedList.Clear();

            //prepare/reset grid for new run
            grid.PrepForPath(collisionBox);

            //round destination and start to nearest node
            GridNode startNode = grid.RoundToNearestNode(startPosition);
            GridNode endNode = grid.RoundToNearestNode(endPosition);

            if (startNode == endNode)
            {
                return new Path(new List<Vector2>() { startPosition, endPosition });
            }

            //insert start node into open
            openList.Add(startNode);

            return ConstructionLoop(startNode, endNode, startPosition, endPosition);
        }

        private Path ConstructionLoop(GridNode startNode, GridNode endNode, Vector2 startPosition, Vector2 endPosition)
        {
            //looping until bumped into target node:
            bool finished = false;
            int countCheck = 0;
            TESTING_LAST_PATH_LENGTH = 0;
            while (!finished)
            {
                ++countCheck;
                ++TESTING_LAST_PATH_LENGTH;
                if (countCheck > lengthyPathThreshhold)
                {
                    return null;
                }
                //  find lowest cost in open list, move to closed list
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
                var pos = grid.GetPositionFromNode(currentNode);

                if (positions.Contains(pos))
                {
                    return new Path(positions); //TODO this if a hack, pathfinder is creating loops
                }

                positions.Insert(0, pos);

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
            try
            {
                grid.Draw(spriteBatch);
            }
            catch { } //this is needed for threadsafety and race conditions
            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial, 
                text: TESTING_LAST_PATH_G.ToString() + '\n' + TESTING_LAST_PATH_LENGTH.ToString(), 
                position: new Vector2(500, 500),
                color: Color.Red,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.None,
                depth: 1f
            );
        }

        private class LengthyRequestContainer
        {
            public GridNode[,] Nodes { get; set; }
            public PathRequest Request { get; set; }
            public List<GridNode> OpenList { get; set; }
            public List<GridNode> ClosedList { get; set; }

            public LengthyRequestContainer(GridNode[,] noded, PathRequest request, List<GridNode> openlist, List<GridNode> closedList)
            {
                Nodes = noded;
                Request = request;
                OpenList = openlist.ToList();
                ClosedList = closedList.ToList();
            }
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
