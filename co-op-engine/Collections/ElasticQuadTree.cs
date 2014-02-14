using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Collections
{
    /// <summary>
    /// A Quadtree that expands it's query boundaries based on the largest child object in it's collection
    /// </summary>
    public class ElasticQuadTree
    {
        RectangleFloat bounds;
        RectangleFloat queryBounds;
        GameObject containedObject;

        bool IsParent { get { return NW != null; } }
        ElasticQuadTree parent;
        ElasticQuadTree NW;
        ElasticQuadTree NE;
        ElasticQuadTree SW;
        ElasticQuadTree SE;

        /// <summary>
        /// if it's the very top one, set parent as null
        /// </summary>
        /// <param name="bounds">collidable area</param>
        /// <param name="parent">parent of this tree</param>
        public ElasticQuadTree(RectangleFloat bounds, ElasticQuadTree parent)
        {
            this.parent = parent;
            this.bounds = bounds;
            this.queryBounds = bounds;
        }

        /// <summary>
        /// inserts the object in the main quadtree
        /// </summary>
        /// <param name="newObject"></param>
        public void MasterInsert(GameObject newObject)
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
        /// goes to the top and does the query
        /// </summary>
        /// <param name="queryBounds">the location of the query area</param>
        /// <returns>list of objects the intersects with the query</returns>
        public List<GameObject> MasterQuery(RectangleFloat queryBounds)
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


        public bool Contains(Vector2 location) { return false; }
        public bool Remove(GameObject newObject) { return false; }
        public void Draw(SpriteBatch spriteBatch, Texture2D drawTexture)
        {
            if (!IsParent)
            {
                float r, g, b;
                r = (float)MechanicSingleton.Instance.rand.NextDouble();
                g = (float)MechanicSingleton.Instance.rand.NextDouble();
                b = (float)MechanicSingleton.Instance.rand.NextDouble();

                spriteBatch.Draw(drawTexture, queryBounds.ToRectangle(),
                    new Color((float)MechanicSingleton.Instance.rand.NextDouble(),
                        (float)MechanicSingleton.Instance.rand.NextDouble(),
                        (float)MechanicSingleton.Instance.rand.NextDouble(),0.3f));
                spriteBatch.Draw(drawTexture, bounds.ToRectangle(),
                    new Color((float)MechanicSingleton.Instance.rand.NextDouble(),
                        (float)MechanicSingleton.Instance.rand.NextDouble(),
                        (float)MechanicSingleton.Instance.rand.NextDouble(),0.3f));
            }
            else
            {
                NW.Draw(spriteBatch, drawTexture);
                SW.Draw(spriteBatch, drawTexture);
                NE.Draw(spriteBatch, drawTexture);
                SE.Draw(spriteBatch, drawTexture);
            }
        }

        /// <summary>
        /// attempts to insert the object into the query
        /// </summary>
        /// <param name="newObject">object to be inserted</param>
        /// <returns>returns true if it was inserted</returns>
        public bool Insert(GameObject newObject)
        {
            //check if object is in the node
            if (!bounds.ContainsInclusive(newObject.Position))
            {
                return false;
            }

            //if it's null, set it to the object
            if (!IsParent)
            {
                if (containedObject == null)
                {
                    containedObject = newObject;
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

            //insert it into children
            if (NW.Insert(newObject)) return true;
            else if (SW.Insert(newObject)) return true;
            else if (NE.Insert(newObject)) return true;
            else if (SE.Insert(newObject)) return true;

            throw new Exception("couldn't insert into quadtree because it didn't fit into subquads");
        }

        /// <summary>
        /// Get the count of all the objects held in the subchildren
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
        /// gets all children objects
        /// </summary>
        /// <returns>all children objects below this</returns>
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
            return new List<GameObject>() { containedObject };
        }

        /// <summary>
        /// if children are empty remove squares and fix up holes
        /// </summary>
        private void Verify()
        {
            //if it's 0, collapse check parent
            var children = ChildCount();
            if (children == 0)
            {
                NW = SW = NE = SE = null;//ugly but it works better for this circumstance
                parent.Verify();
            }
            else if (children == 1) //means we can collapse the quad's children and move it on up with the jeffersons
            {
                var objects = GatherAll();
                NW = SW = NE = SE = null;
                containedObject = objects[0]; // this should work... if it doesn't something else if wrong
            }
        }

        /// <summary>
        /// gets all objects that collide with the queryrange
        /// </summary>
        /// <param name="queryBounds">the area to query</param>
        /// <returns>objects that collide with the query</returns>
        private List<GameObject> Query(RectangleFloat queryBounds)
        {
            var preparedObjects = new List<GameObject>();

            //if it's not here, return the empty one
            if (!queryBounds.Intersects(queryBounds))
            {
                return preparedObjects;
            }

            //if it's not a parent add and return
            if (!IsParent)
            {
                preparedObjects.Add(containedObject);
                return preparedObjects;
            }

            //get objects in children
            preparedObjects.AddRange(NW.Query(queryBounds));
            preparedObjects.AddRange(NE.Query(queryBounds));
            preparedObjects.AddRange(SW.Query(queryBounds));
            preparedObjects.AddRange(SE.Query(queryBounds));
            return preparedObjects;
        }

        /// <summary>
        /// does split logic, placing contained object appropriately
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

            //insert children (this may not work... new strategy)
            Insert(containedObject);
            containedObject = null;
        }

        /// <summary>
        /// inflates the area to the largest object so it can be detected across boundaries
        /// </summary>
        /// <param name="newObject">object inserted</param>
        private void InflateBoundry(GameObject newObject)
        {
            float currentXInflation = (bounds.Width - queryBounds.Width) / 2;
            float currentYInflation = (bounds.Height - queryBounds.Height) / 2;

            float inflateXBy = 0;
            float inflateYBy = 0;

            if (currentXInflation < newObject.BoundingBox.Width / 2)
            {
                inflateXBy = (newObject.BoundingBox.Width / 2) - currentXInflation;
            }

            if (currentYInflation < newObject.BoundingBox.Height / 2)
            {
                inflateYBy = (newObject.BoundingBox.Height / 2) - currentYInflation;
            }

            queryBounds.Inflate(inflateXBy, inflateYBy);
        }
    }
}
