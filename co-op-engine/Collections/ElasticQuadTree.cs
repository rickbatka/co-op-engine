using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using co_op_engine.Components.Physics;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Collections
{
    /// <summary>
    /// A Quadtree that expands it's query boundaries based on the largest 
    /// child object in it's child's collection
    /// </summary>
    public class ElasticQuadTree : SpacialBase
    {
        public override RectangleFloat Dimensions
        {
            get { return bounds; }
        }
        private RectangleFloat bounds;
        private RectangleFloat queryBounds;
        private GameObject containedObject;

        private bool IsParent { get { return NW != null; } }
        private ElasticQuadTree parent;

        //children
        private ElasticQuadTree NW;
        private ElasticQuadTree NE;
        private ElasticQuadTree SW;
        private ElasticQuadTree SE;

        /// <summary>
        /// if it's the very top one, set parent as null
        /// </summary>
        /// <param name="bounds">collidable area</param>
        /// <param name="parent">parent of this node</param>
        public ElasticQuadTree(RectangleFloat bounds, ElasticQuadTree parent)
        {
            this.parent = parent;
            this.bounds = bounds;
            this.queryBounds = bounds;
        }

        /// <summary>
        /// climbs to the top of the tree and begins an insert from there
        /// @TODO could just climb until insert works?
        /// </summary>
        /// <param name="newObject">object to be inserted</param>
        public override void MasterInsert(GameObject newObject)
        {
            if (parent != null)
            {
                parent.MasterInsert(newObject);
            }
            else
            {
                Insert(newObject);
            }
        }

        /// <summary>
        /// climbs the tree and queries the very top node
        /// point of confusion, this gets anything COLLIDING with this querybounds, not contained within
        /// </summary>
        /// <param name="queryBounds">the location of the query area</param>
        /// <returns>list of objects the INTERSECTS with the query</returns>
        public override List<GameObject> MasterQuery(RectangleFloat queryBounds)
        {
            if (parent != null)
            {
                return parent.MasterQuery(queryBounds);
            }
            else
            {
                return Query(queryBounds);
            }
        }

        /// <summary>
        /// climbs the tree and queries the very top node
        /// point of confusion, this gets anything COLLIDING with this querybounds, not contained within
        /// </summary>
        /// <param name="queryBounds">the location of the query area</param>
        /// <returns>list of objects the INTERSECTS with the query</returns>
        public List<GameObject> MasterQuery(Rectangle queryBounds)
        {
            return MasterQuery(RectangleFloat.FromRectangle(queryBounds));
        }

        /// <summary>
        /// removes a specified object from this node and 
        /// returns whether it was removed or not found
        /// </summary>
        /// <param name="newObject">Object to remove</param>
        /// <returns>if the object was removed/found</returns>
        public override bool Remove(GameObject newObject)
        {
            if (containedObject == newObject)
            {
                containedObject = null;
                queryBounds = bounds;
                return true;
            }
            return false;
        }

        /// <summary>
        /// used to visualize quads and their expanded queriable areas
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="drawTexture"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsParent)
            {
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.DebugGridTexture, 
                    destinationRectangle: queryBounds.ToRectangle(), 
                    sourceRectangle: null, 
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effect: SpriteEffects.None,
                    depth: 1f
                );
                //spriteBatch.Draw(drawTexture, bounds.ToRectangle(), Color.White);
            }
            else
            {
                NW.Draw(spriteBatch);
                SW.Draw(spriteBatch);
                NE.Draw(spriteBatch);
                SE.Draw(spriteBatch);
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.DebugGridTexture,
                    destinationRectangle: queryBounds.ToRectangle(), 
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effect: SpriteEffects.None,
                    depth: 1f
                );
            }
        }

        /// <summary>
        /// attempts to insert the object into this node
        /// if it's a parent, it delegates this task to each of it's children
        /// </summary>
        /// <param name="newObject">object to be inserted</param>
        /// <returns>returns true if it was inserted</returns>
        public override bool Insert(GameObject newObject)
        {
            //check if object is in the node
            if (!bounds.ContainsInclusive(newObject.Position))
            {
                return false;
            }

            if (containedObject != null && newObject.Position == containedObject.Position)
            {
                newObject.Position = new Vector2(newObject.Position.X + 1, newObject.Position.Y + 1);
                MasterInsert(newObject);
                return true;
            }

            //if it's null, set it to the object
            if (!IsParent)
            {
                if (containedObject == null)
                {
                    containedObject = newObject;
                    newObject.CurrentQuad = this;
                    if (parent != null)
                    {
                        InflateBoundry(newObject);
                    }
                    return true;
                }
            }

            //got here because it's not a parent and still has an object to insert
            if (!IsParent)
            {
                Split();
                if (parent != null)
                {
                    InflateBoundry(newObject);
                }
            }

            //insert it into children derp yes repeated code....
            if (NW.Insert(newObject))
            {
                InflateBoundry(newObject);
                return true;
            }
            else if (SW.Insert(newObject))
            {
                InflateBoundry(newObject);
                return true;
            }
            else if (NE.Insert(newObject))
            {
                InflateBoundry(newObject);
                return true;
            }
            else if (SE.Insert(newObject))
            {
                InflateBoundry(newObject);
                return true;
            }
            //very bad subdivision error, we should keep an eye out for this
            throw new Exception("couldn't insert into quadtree because it didn't fit into subquads");
        }

        /// <summary>
        /// If it's a parent, it counts it's children's held objects
        /// If not a parent, it checks if it contains an object
        /// </summary>
        /// <returns>count of all children</returns>
        private int ChildCount()
        {
            if (IsParent)
            {
                return NW.ChildCount()
                    + NE.ChildCount()
                    + SW.ChildCount()
                    + SE.ChildCount();
            }
            return containedObject == null ? 0 : 1;
        }

        /// <summary>
        /// gets all children objects or returns held object
        /// </summary>
        /// <returns>all contained objects from this node and/or below</returns>
        private List<GameObject> GatherAll()
        {
            if (IsParent)
            {
                var gathered = new List<GameObject>();
                gathered.AddRange(NW.GatherAll());
                gathered.AddRange(SW.GatherAll());
                gathered.AddRange(NE.GatherAll());
                gathered.AddRange(SE.GatherAll());
                return gathered;
            }
            else
            {
                List<GameObject> obj = new List<GameObject>();
                if (containedObject != null)
                {
                    obj.Add(containedObject);
                }
                return obj;
            }
        }

        /// <summary>
        /// folds up unused quads by checking this node and it's parent, and on up if it keeps finding empty nodes
        /// </summary>
        private void Verify()
        {
            if (IsParent)
            {
                //if it's 0, collapse check parent
                var children = ChildCount();
                if (children == 0)
                {
                    NW = SW = NE = SE = null;//ugly but it works better for this circumstance
                    parent.Verify();
                }
                else if (children == 1)
                {
                    var objects = GatherAll()[0];
                    NW = SW = NE = SE = null;
                    containedObject = objects;
                    objects.CurrentQuad = this;
                    
                    if (parent != null) //doh, forgot that the master quad doesn't have a parent...
                    {
                        parent.Verify();
                    }
                }
                //if it's more than 1, do nothing
            }
            else
            {
                if (parent != null) //temp bug fix not sure why this was null...
                {
                    parent.Verify();
                }
            }
        }

        /// <summary>
        /// gets all objects that collide with the queryrange
        /// </summary>
        /// <param name="queryBounds">the area to query</param>
        /// <returns>objects that collide with the query</returns>
        private List<GameObject> Query(RectangleFloat query)
        {
            var preparedObjects = new List<GameObject>();

            //if it's not here, return the empty one
            if (!queryBounds.Intersects(query))
            {
                return preparedObjects;
            }

            //if it's not a parent add and return
            if (!IsParent)
            {
                if (containedObject != null && containedObject.PhysicsCollisionBox.Intersects(query.ToRectangle()))
                {
                    preparedObjects.Add(containedObject);
                }
                return preparedObjects;
            }

            //get objects in children
            preparedObjects.AddRange(NW.Query(query));
            preparedObjects.AddRange(NE.Query(query));
            preparedObjects.AddRange(SW.Query(query));
            preparedObjects.AddRange(SE.Query(query));
            return preparedObjects;
        }

        /// <summary>
        /// splits this node into 4 quads and places it's held object into the appropriate child
        /// </summary>
        private void Split()
        {
            //build em up
            //nw left top
            NW = new ElasticQuadTree(new RectangleFloat(bounds.Left, bounds.Top, bounds.Width / 2, bounds.Height / 2), this);
            //ne center, top
            NE = new ElasticQuadTree(new RectangleFloat(bounds.Left + bounds.Width / 2, bounds.Top, bounds.Width / 2, bounds.Height / 2), this);
            //sw left center
            SW = new ElasticQuadTree(new RectangleFloat(bounds.Left, bounds.Top + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2), this);
            //se center center
            SE = new ElasticQuadTree(new RectangleFloat(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2), this);

            //semi dangerous assumption that it contains an object at this point
            var temp = containedObject;
            containedObject = null;

            if (NW.Insert(temp)) return;
            else if (SW.Insert(temp)) return;
            else if (NE.Insert(temp)) return;
            else if (SE.Insert(temp)) return;

            MasterInsert(temp);
        }

        /// <summary>
        /// inflates the queriable area of this node area to the largest object 
        /// so it can be detected across boundaries
        /// </summary>
        /// <param name="newObject">object inserted</param>
        private void InflateBoundry(GameObject newObject)
        {
            float currentXInflation = (queryBounds.Width - bounds.Width) / 2;
            float currentYInflation = (queryBounds.Height - bounds.Height) / 2;

            float inflateXBy = 0;
            float inflateYBy = 0;

            if (currentXInflation < newObject.PhysicsCollisionBox.Width / 2)
            {
                inflateXBy = (newObject.CurrentFrame.DrawRectangle.Width / 2) - currentXInflation;
            }

            if (currentYInflation < newObject.PhysicsCollisionBox.Height / 2)
            {
                inflateYBy = (newObject.CurrentFrame.DrawRectangle.Height / 2) - currentYInflation;
            }

            queryBounds.Inflate(inflateXBy, inflateYBy);
        }

        /// <summary>
        /// Allows objects to notify the quadtree that they moved
        /// </summary>
        /// <param name="ownedObject">the moved object</param>
        public override void NotifyOfMovement(GameObject ownedObject)
        {
            if (!bounds.ContainsInclusive(ownedObject.Position) || containedObject != ownedObject)
            {
                if (Remove(ownedObject))
                {
                    MasterInsert(ownedObject);
                }
                Verify();
            }
        }
    }
}
