using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ReferenceMaterial.Physics;

namespace ReferenceMaterial.Entity.EntityComponents
{
	class CollidablePhysics : PhysicsBase
	{
		//needed to be public for the quadtree dual dependancy
		public ElasticQuadTree_OLD currentQuad;

		Vector2 velocity;
		float friction = 0.8f;
		float maxSpeed = 4;
		float moveForce = 10;

		public CollidablePhysics(GameObject owner, Vector2 startPosition)
			: base(owner)
		{
			Position = startPosition;
			//levelRef = level;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			//don't bother doing anything unless it is moving
            //if (velocity != Vector2.Zero || owner.BrainComponent.MoveDirection != Vector2.Zero)
            //{
            //    Move(owner.BrainComponent.MoveDirection);
            //}
			SyncBoundry();
		}

		

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			//debug
		}

		public void Move(Vector2 moveVector)
		{
			ApplyFrictionAndMove(moveVector);

			CheckForCollisionsWithWorld();

			CheckForCollisionsWithObjects();

			//check if position is inside current quad
			if (!currentQuad.IsWithinThisQuad(Position))
			{
				//if not, remove from current and reinsert back into world
				currentQuad.RemoveFromThisQuad(owner);
				currentQuad.MasterInsert(owner);
			}
		}

		private void CheckForCollisionsWithObjects()
		{
			List<GameObject> colliders = currentQuad.MasterQuery(FloatRect.FromRectangle(boundryBox));
			colliders.Remove(owner);
			if (colliders.Count > 0)
			{
				GameObject collider = colliders[0];

				Rectangle otherBoundry = collider.PhysicsComponent.BoundryBox;
				Vector2 positionsDiff = Position - collider.PhysicsComponent.Position;

				//dirty copypasta, probably inefficient, but I'll address that when framerate starts to drop
				int xOverlap = -(Math.Abs(otherBoundry.X - boundryBox.X) - ((boundryBox.Width + otherBoundry.Width) / 2));
				int yOverlap = -(Math.Abs(otherBoundry.Y - boundryBox.Y) - ((boundryBox.Height + otherBoundry.Height) / 2));

				if (collider.PhysicsComponent.Position.Y > boundryBox.Y)
				{
					Position.Y -= yOverlap / 2;
				}
				else
				{
					Position.Y += yOverlap / 2;
				}

				if (collider.PhysicsComponent.Position.X > boundryBox.X)
				{
					Position.X -= xOverlap / 2;
				}
				else
				{
					Position.X += xOverlap / 2;
				}
			}
		}

		private void ApplyFrictionAndMove(Vector2 directionOfForce)
		{
			//apply force if present
			if (directionOfForce.Length() != 0)
			{
				directionOfForce.Normalize();
				directionOfForce *= moveForce;
				velocity += directionOfForce;
			}

			//account for friction
			velocity *= friction;

			//limit to max speed
			if (velocity.Length() > maxSpeed)
			{
				velocity.Normalize();
				velocity *= maxSpeed;
			}

			//move object
			Position += velocity;
		}

		private void CheckForCollisionsWithWorld()
		{
			//Point tilePosition = TileEngineHelper.PositionToTile(Position);

			//gonna keep it simple, resolve them back out where they came from
			//we can do wall sliding another day, this is a prototype

            //int xMin = (int)MathHelper.Clamp(tilePosition.X - 1, 0, levelRef.tilemap.XLength());
            //int xMax = (int)MathHelper.Clamp(tilePosition.X + 1, 0, levelRef.tilemap.XLength());

            //int yMin = (int)MathHelper.Clamp(tilePosition.Y - 1, 0, levelRef.tilemap.YLength());
            //int yMax = (int)MathHelper.Clamp(tilePosition.Y + 1, 0, levelRef.tilemap.YLength());
            /*
			for (int x = xMin; x <= xMax; ++x)
			{
				for (int y = yMin; y <= yMax; ++y)
				{
					if (levelRef.tilemap.IsPhysical(x, y))
					{
						Rectangle tileRect = TileEngineHelper.GetTileRectangle(x, y);

						if (tileRect.Intersects(boundryBox))
						{
							//determine if x or y needs to be resolved

							int xOverlap = -(Math.Abs(tileRect.X - boundryBox.X) - ((boundryBox.Width + tileRect.Width) / 2));
							int yOverlap = -(Math.Abs(tileRect.Y - boundryBox.Y) - ((boundryBox.Height + tileRect.Height) / 2));

							if (xOverlap > yOverlap)
							{
								if (tileRect.Y > boundryBox.Y)
								{
									Position.Y -= yOverlap;
								}
								else
								{
									Position.Y += yOverlap;
								}
							}
							else
							{
								if (tileRect.X > boundryBox.X)
								{
									Position.X -= xOverlap;
								}
								else
								{
									Position.X += xOverlap;
								}
							}
						}
					}
				}
			}*/
		}
	}
}
