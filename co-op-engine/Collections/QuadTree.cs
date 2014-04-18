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
    public class QuadTree
    {
        public QuadTree(RectangleFloat bounds, QuadTree parent)
        { }

        public void MasterInsert(GameObject queryBounds)
        {
            throw new NotImplementedException();
        }

        public List<GameObject> MasterQuery(RectangleFloat queryBounds)
        {
            throw new NotImplementedException();
        }

        public List<GameObject> MasterQuery(Rectangle queryBounds)
        {
            throw new NotImplementedException();
        }

        public bool Remove(GameObject newObject)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D drawTexture)
        {
            throw new NotImplementedException();
        }

        public bool Insert(GameObject newObject)
        {
            throw new NotImplementedException();
        }

        private int ChildCount()
        {
            throw new NotImplementedException();
        }

        private List<GameObject> GatherAll()
        {
            throw new NotImplementedException();
        }

        private void Verify()
        {
            throw new NotImplementedException();
        }

        private List<GameObject> Query(RectangleFloat query)
        {
            throw new NotImplementedException();
        }

        private void Split()
        {
            throw new NotImplementedException();
        }

        private void InflateBoundry(GameObject newObject)
        {
            throw new NotImplementedException();
        }

        public void NotfyOfMovement(GameObject ownedObject)
        {
            throw new NotImplementedException();
        }
    }
}
