using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.Pathing;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.World.Level
{
    public class ObjectContainer
    {
        //needs spacial and iterative reference to objects, and probably even a unique indexable heap/binary tree
        SpacialBase SpacialReference;

        //iterative list for updating and linear referencing
        List<GameObject> LinearReference;

        Dictionary<int, GameObject> IndexedReference;

        public int ObjectCount { get { return LinearReference.Count(); } }

        public ObjectContainer(Rectangle levelBounds)
        {
            PATHING_TEST_REMOVE_IF_FORGOTTEN_AND_COMMITTED = TimeSpan.Zero;
            SpacialReference = new QuadTree(co_op_engine.Utility.RectangleFloat.FromRectangle(levelBounds), null);
            LinearReference = new List<GameObject>();
            IndexedReference = new Dictionary<int, GameObject>();
        }

        public void AddObject(GameObject newObject)
        {
            SpacialReference.MasterInsert(newObject);
            LinearReference.Add(newObject);
            IndexedReference.Add(newObject.ID, newObject);
        }

        public void RemoveObject(GameObject removeObject)
        {
#warning we cannot rely on local remove, sometimes this isn't valid mid update
            var removed = removeObject.CurrentQuad.Remove(removeObject); 
            removed = removed && LinearReference.Remove(removeObject);
            removed = removed && IndexedReference.Remove(removeObject.ID);
            if (!removed)
            {
                throw new Exception("Object containers out of sync");
            }
        }

#warning proto code, should be removed after commit
        private TimeSpan PATHING_TEST_REMOVE_IF_FORGOTTEN_AND_COMMITTED;
        private int PATHING_TEST_RESET_MILLI = 5000;

        public void RemoveDeletedObjects()
        {
            var objectsToRemove = LinearReference.Where(o => o.ShouldDelete).ToArray();
            int del = objectsToRemove.Count();
            for (int i = 0; i < del; i++)
            {
                RemoveObject(objectsToRemove[i]);
            }
        }

        public void UpdateAll(GameTime gameTime)
        {
            PATHING_TEST_REMOVE_IF_FORGOTTEN_AND_COMMITTED -= gameTime.ElapsedGameTime;
            if (PATHING_TEST_REMOVE_IF_FORGOTTEN_AND_COMMITTED <= TimeSpan.Zero)
            {
                PATHING_TEST_REMOVE_IF_FORGOTTEN_AND_COMMITTED = TimeSpan.FromMilliseconds(PATHING_TEST_RESET_MILLI);
                List<MetaObstacle> obstacles = new List<MetaObstacle>();

                for (int i = 0; i < LinearReference.Count; i++)
                {
                    var obj = LinearReference[i];
                    obj.Update(gameTime);
                    if (obj.UsedInPathing)
                    {
                        obstacles.Add(new MetaObstacle()
                        {
                            bounds = obj.PhysicsCollisionBox,
                            pathingWeight = 900000,
                        });
                    }
                }

                PathFinder.Instance.ReceiveSnapshot(obstacles, SpacialReference.Dimensions.ToRectangle());
            }
            else
            {
                for (int i = 0; i < LinearReference.Count; i++)
                {
                    var obj = LinearReference[i];
                    obj.Update(gameTime);
                }
            }
        }

        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var obj in LinearReference)
            {
                if (Camera.Instance.ViewBoundsRectangle.Contains(new Point((int)obj.Position.X, (int)obj.Position.Y)))
                {
                    obj.Draw(spriteBatch);
                }
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            SpacialReference.Draw(spriteBatch);
        }

        public GameObject GetObjectById(int Id)
        {
            return IndexedReference[Id];
        }

        public List<GameObjectCommand> GetWorldForNetwork()
        {
            List<GameObjectCommand> worldCommands = new List<GameObjectCommand>();

            foreach (var go in LinearReference)
            {
                worldCommands.Add(new GameObjectCommand()
                {
                    CommandType = GameObjectCommandType.Create,
                    Parameters = go.BuildCreateParams(),
                });
            }

            return worldCommands;
        }
    }
}
