using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.ServiceProviders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.World.Level
{
    public class ObjectContainer : IActorInformationProvider
    {
        //needs spacial and iterative reference to objects, and probably even a unique indexable heap/binary tree
        ElasticQuadTree quadTree;

        //iterative list for updating and linear referencing
        List<GameObject> gameObjects;

        public ObjectContainer(Rectangle levelBounds)
        {
            quadTree = new ElasticQuadTree(co_op_engine.Utility.RectangleFloat.FromRectangle(levelBounds), null);
            gameObjects = new List<GameObject>();
        }

        public void AddObject(GameObject newObject)
        {
            quadTree.MasterInsert(newObject);
            gameObjects.Add(newObject);
        }

        public void UpdateAll(GameTime gameTime)
        {
            foreach (var obj in gameObjects)
            {
                obj.Update(gameTime);
            }
        }

        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch, Texture2D debugTexture)
        {
            quadTree.Draw(spriteBatch, debugTexture);
        }

#region IActorInformationProvidor

#warning remove this, it's a hack to get things moving and should be replaced by a search method later
        public void SetPlayer(GameObject player)
        {
            players = new List<GameObject>();
            players.Add(player);
        }

        private List<GameObject> players;

        public List<GameObject> GetPlayers()
        {
            return players;
        }

#endregion IActorInformationProvidor
    }
}
