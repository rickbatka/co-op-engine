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
    class ElasticQuadTree
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

        public ElasticQuadTree(RectangleFloat bounds, ElasticQuadTree parent)
        {
            this.parent = parent;
            this.bounds = bounds;
            this.queryBounds = bounds;
        }

        public void MasterInsert(GameObject newObject) { }
        public List<GameObject> MasterQuery(RectangleFloat queryBounds) { return null; }
        public bool Insert(GameObject newObject) { return false; }
        public bool Contains(Vector2 location) { return false; }
        public bool Remove(GameObject newObject) { return false; }
        public void Draw(SpriteBatch spriteBatch, Texture2D drawTexture) { }

        private int ChildCount() { return 0; }
        private List<GameObject> GatherAll() { return null; }
        private void Verify() { }
        private List<GameObject> Query(RectangleFloat queryBounds) { return null; }
        private void Split() { }
        private void InflateBoundry(GameObject newObject) { }
    }
}
