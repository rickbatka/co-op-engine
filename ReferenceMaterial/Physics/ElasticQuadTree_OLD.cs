using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReferenceMaterial.Entity;
using ReferenceMaterial.Entity.EntityComponents;

namespace ReferenceMaterial.Physics
{
	class ElasticQuadTree_OLD
	{
		const int NODE_CAPACITY = 1;

		FloatRect boundary;
		FloatRect queryableBoundry;
		List<GameObject> objects;
		bool isParent
		{
			get { return NW != null; }
		}

		ElasticQuadTree_OLD parent;

		ElasticQuadTree_OLD NW;
		ElasticQuadTree_OLD SW;
		ElasticQuadTree_OLD NE;
		ElasticQuadTree_OLD SE;

		//main methods

		public ElasticQuadTree_OLD(FloatRect boundingBox)
		{
			objects = new List<GameObject>();
			boundary = boundingBox;
			queryableBoundry = boundingBox;
		}

		public ElasticQuadTree_OLD(FloatRect boundingBox, ElasticQuadTree_OLD parent)
		{
			objects = new List<GameObject>();
			this.boundary = boundingBox;
			this.parent = parent;
			queryableBoundry = boundingBox;
		}

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

		public List<GameObject> MasterQuery(FloatRect gatherArea)
		{
			if (parent != null)
			{
				return parent.MasterQuery(gatherArea);
			}
			else
			{
				return QueryArea(gatherArea);
			}
		}

		public bool Insert(GameObject newObject)
		{
			//check if object is even in this node
			if (!boundary.Contains(newObject.PhysicsComponent.Position))
			{
				return false;
			}

			//if there's space, add it
			if (!isParent)
			{
				if (objects.Count < NODE_CAPACITY)
				{
					objects.Add(newObject);
					((CollidablePhysics)newObject.PhysicsComponent).currentQuad = this;

					if (parent != null)
					{
						InflateBoundary(newObject);
					}
					return true;
				}
			}

			//if it's null split
			if (NW == null)
			{
				Split();
			}

			//insert it into the children
			if (NW.Insert(newObject)) return true;
			else if (SW.Insert(newObject)) return true;
			else if (NE.Insert(newObject)) return true;
			else if (SE.Insert(newObject)) return true;

			throw new Exception("shouldn't get here");

			//return false;
		}

		private void InflateBoundary(GameObject newObject)
		{
			float currentXInflation = (boundary.Width - queryableBoundry.Width) / 2;
			float currentYInflation = (boundary.Height - queryableBoundry.Height) / 2;

			float inflateXBy = 0;
			float inflateYBy = 0;

			if (currentXInflation < newObject.PhysicsComponent.BoundryBox.Width / 2)
			{
				inflateXBy = (newObject.PhysicsComponent.BoundryBox.Width / 2) - currentXInflation;
			}

			if (currentYInflation < newObject.PhysicsComponent.BoundryBox.Height / 2)
			{
				inflateYBy = (newObject.PhysicsComponent.BoundryBox.Height / 2) - currentYInflation;
			}

			queryableBoundry.X -= inflateXBy;
			queryableBoundry.Y -= inflateYBy;

			queryableBoundry.Width += inflateXBy * 2;
			queryableBoundry.Height += inflateYBy * 2;
		}

		public void Split()
		{
			//instantiate each quadrant
			NW = new ElasticQuadTree_OLD(new FloatRect(boundary.X, boundary.Y, boundary.Width / 2, boundary.Height / 2), this);
			SW = new ElasticQuadTree_OLD(new FloatRect(boundary.X, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), this);
			NE = new ElasticQuadTree_OLD(new FloatRect(boundary.X + boundary.Width / 2, boundary.Y, boundary.Width / 2, boundary.Height / 2), this);
			SE = new ElasticQuadTree_OLD(new FloatRect(boundary.X + boundary.Width / 2, boundary.Y + boundary.Height / 2, boundary.Width / 2, boundary.Height / 2), this);

			//divy out children
			foreach (GameObject go in objects)
			{
				if (NW.Insert(go)) continue;
				else if (SW.Insert(go)) continue;
				else if (NE.Insert(go)) continue;
				else if (SE.Insert(go)) continue;
			}

			objects.Clear();
		}

		public List<GameObject> QueryArea(FloatRect gatherArea)
		{
			//prepare list
			List<GameObject> gatheredObjects = new List<GameObject>();

			//return empty list if range not in this area
			if (!gatherArea.Intersects(queryableBoundry))
			{
				return gatheredObjects;
			}

			//loop over objects, adding those in range (should ignore if parent)
			if (!isParent)
			{
				foreach (GameObject go in objects)
				{
					if (gatherArea.Intersects(go.PhysicsComponent.BoundryBox))
					{
						gatheredObjects.Add(go);
					}
				}
				return gatheredObjects;
			}

			//else get objects in children
			gatheredObjects.AddRange(NW.QueryArea(gatherArea));
			gatheredObjects.AddRange(SW.QueryArea(gatherArea));
			gatheredObjects.AddRange(NE.QueryArea(gatherArea));
			gatheredObjects.AddRange(SE.QueryArea(gatherArea));
			return gatheredObjects;
		}

		public bool IsWithinThisQuad(Vector2 position)
		{
			return boundary.Contains(position);
		}

		public bool RemoveFromThisQuad(GameObject gameObject)
		{
			//remove it
			bool removed = objects.Remove(gameObject);

			if (removed && parent != null)
			{
				//test integrity parent ref?
				parent.VerifyChildren();
			}

			return removed;
		}

		private void VerifyChildren()
		{
			if (GetChildCount() <= NODE_CAPACITY)
			{
				var objects = GatherAll();
				NW = null;
				SW = null;
				NE = null;
				SE = null;
				objects.ForEach(u => this.Insert(u));

				if (objects.Count <= NODE_CAPACITY && parent != null)
				{
					parent.VerifyChildren();
				}
			}
		}

		private List<GameObject> GatherAll()
		{
			if (isParent)
			{
				List<GameObject> gathered = new List<GameObject>();
				gathered.AddRange(NW.GatherAll());
				gathered.AddRange(SW.GatherAll());
				gathered.AddRange(NE.GatherAll());
				gathered.AddRange(SE.GatherAll());
				return gathered;
			}
			else return objects;
		}

		public int GetChildCount()
		{
			if (isParent)
			{
				return NW.GetChildCount()
					+ NE.GetChildCount()
					+ SW.GetChildCount()
					+ SE.GetChildCount();
			}
			return 0;
		}

		public void Draw(SpriteBatch spriteBatch, Texture2D squaretexture)
		{
			if (!isParent)
			{
				spriteBatch.Draw(squaretexture, queryableBoundry.ToRectangle(), Color.White);
				spriteBatch.Draw(squaretexture, boundary.ToRectangle(), Color.White);
			}
			else
			{
				NW.Draw(spriteBatch, squaretexture);
				SW.Draw(spriteBatch, squaretexture);
				NE.Draw(spriteBatch, squaretexture);
				SE.Draw(spriteBatch, squaretexture);
			}
		}
	}

	struct FloatRect
	{
		public float X;
		public float Y;
		public float Width;
		public float Height;

		public FloatRect(float x, float y, float width, float height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public bool Contains(Vector2 position)
		{
			return position.X >= X && position.X <= X + Width && position.Y >= Y && position.Y <= Y + Height;
		}

		public bool Intersects(FloatRect rect)
		{
			return X <= rect.X + rect.Width
				&& rect.X <= X + Width
				&& Y <= rect.Y + rect.Height
				&& rect.Y <= Y + Height;
		}

		public bool Intersects(Rectangle rect)
		{
			return X <= rect.X + rect.Width
				&& rect.X <= X + Width
				&& Y <= rect.Y + rect.Height
				&& rect.Y <= Y + Height;
		}

		public Rectangle ToRectangle()
		{
			return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
		}

		static public FloatRect FromRectangle(Rectangle rectangle)
		{
			return new FloatRect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
	}
}
