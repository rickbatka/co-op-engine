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
    public class QuadTree : SpacialBase
    {
        public override RectangleFloat Dimensions
        {
            get { return hardBounds; }
        }
        private readonly RectangleFloat hardBounds;
        private RectangleFloat queryBounds;

        private bool isParent { get { return NW != null; } }
        private QuadTree parent;
        private QuadTree NW;
        private QuadTree SW;
        private QuadTree NE;
        private QuadTree SE;

        private List<GameObject> heldObjects;
        private const int MAX_OBJECTS_PER_QUAD = 2;

        //done
        public QuadTree(RectangleFloat bounds, QuadTree parent)
        {
            heldObjects = new List<GameObject>();
            this.hardBounds = bounds;
            this.queryBounds = bounds;
            this.parent = parent;
        }

        //done
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

        //done
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

        //done
        public override bool Remove(GameObject newObject)
        {
            return heldObjects.Remove(newObject);
        }

        //done
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isParent)
            {
                NW.Draw(spriteBatch);
                SW.Draw(spriteBatch);
                NE.Draw(spriteBatch);
                SE.Draw(spriteBatch);
            }
            else
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
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.DebugGridTexture, 
                    destinationRectangle: hardBounds.ToRectangle(), 
                    sourceRectangle: null,
                    color: Color.Red,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effect: SpriteEffects.None,
                    depth: 1f
                );
            }
        }

        //done
        public override bool Insert(GameObject newObject)
        {
            if (!hardBounds.ContainsInclusive(newObject.BoundingBox.Center))
            {
                return false;
            }

            //placeholder for when we need to bring back stacked objects

            if (heldObjects.Count() != 0 && heldObjects.Any(o => o.Position == newObject.Position && o != newObject))
            {
                newObject.Position = new Vector2(newObject.Position.X + 1, newObject.Position.Y + 1);
                MasterInsert(newObject);
                return true;
            }

            if (!isParent)
            {
                heldObjects.Add(newObject);
                newObject.CurrentQuad = this;
                InflateBoundry(newObject);
                if (heldObjects.Count() > MAX_OBJECTS_PER_QUAD)
                {
                    Split();
                }
                return true;
            }
            else
            {
                InflateBoundry(newObject);
                if (NW.Insert(newObject)) return true;
                else if (NE.Insert(newObject)) return true;
                else if (SW.Insert(newObject)) return true;
                else if (SE.Insert(newObject)) return true;
                else
                {
                    throw new Exception("Quad insertion failure, this SHOULD never happen");
                }
            }
        }

        //done
        private int ChildCount()
        {
            if (isParent)
            {
                return NW.ChildCount()
                    + NE.ChildCount()
                    + SW.ChildCount()
                    + SE.ChildCount();
            }
            else
            {
                return heldObjects.Count();
            }
        }

        //done (almost fell into a ref trap)
        private List<GameObject> GatherAll()
        {
            if (isParent)
            {
                List<GameObject> gatheredList = new List<GameObject>();
                gatheredList.AddRange(NW.GatherAll());
                gatheredList.AddRange(NE.GatherAll());
                gatheredList.AddRange(SW.GatherAll());
                gatheredList.AddRange(SE.GatherAll());
                return gatheredList;
            }
            else
            {
                return new List<GameObject>(heldObjects);
            }
        }

        //NOT DONE FOIDJF LIDJ F#####################################################################
        private void Verify()
        {
#warning no verification of quadtree currently
        }

        //done
        private List<GameObject> Query(RectangleFloat query)
        {
            List<GameObject> objects = new List<GameObject>();

            if (!queryBounds.Intersects(query))
            {
                return objects;
            }

            if (isParent)
            {
                objects.AddRange(NW.Query(query));
                objects.AddRange(NE.Query(query));
                objects.AddRange(SW.Query(query));
                objects.AddRange(SE.Query(query));
                return objects;
            }
            else
            {
                foreach (GameObject obj in heldObjects)
                {
                    if (obj.BoundingBox != query && query.Intersects(obj.BoundingBox))
                    {
                        objects.Add(obj);
                    }
                }
                return objects;
            }
        }

        //done
        private void Split()
        {
            NW = new QuadTree(new RectangleFloat(hardBounds.Left, hardBounds.Top, hardBounds.Width / 2, hardBounds.Height / 2), this);
            NE = new QuadTree(new RectangleFloat(hardBounds.Left + hardBounds.Width / 2, hardBounds.Top, hardBounds.Width / 2, hardBounds.Height / 2), this);
            SW = new QuadTree(new RectangleFloat(hardBounds.Left, hardBounds.Top + hardBounds.Height / 2, hardBounds.Width / 2, hardBounds.Height / 2), this);
            SE = new QuadTree(new RectangleFloat(hardBounds.Left + hardBounds.Width / 2, hardBounds.Top + hardBounds.Height / 2, hardBounds.Width / 2, hardBounds.Height / 2), this);

            //heh got a little tricky with this one ^_^
            foreach (GameObject obj in heldObjects)
            {
                if (!Insert(obj))
                {
                    MasterInsert(obj);
                }
            }

            //memory grab issue
            heldObjects.Clear();
            heldObjects = null;
        }

        //direct copy, was already robust enough
        private void InflateBoundry(GameObject newObject)
        {
            float currentXInflation = (queryBounds.Width - hardBounds.Width) / 2;
            float currentYInflation = (queryBounds.Height - hardBounds.Height) / 2;

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

        //ehhhhh not quite done yet
        public override void NotifyOfMovement(GameObject ownedObject)
        {
            if (!hardBounds.ContainsInclusive(ownedObject.Position) || !heldObjects.Contains(ownedObject))
            {
                if (Remove(ownedObject))
                {
                    MasterInsert(ownedObject);
                }
                Verify();
            }
        }

        public override List<GameObject> DEBUGEXPOSURE_DONOTUSE()
        {
            return heldObjects;
        }
    }
}
