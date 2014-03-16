﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.World.Level
{
    public class ObjectContainer
    {
        //needs spacial and iterative reference to objects, and probably even a unique indexable heap/binary tree
        ElasticQuadTree SpacialReference;

        //iterative list for updating and linear referencing
        List<GameObject> LinearReference;

        Dictionary<int, GameObject> IndexedReference;

        public int ObjectCount { get { return LinearReference.Count(); } }

        public ObjectContainer(Rectangle levelBounds)
        {
            SpacialReference = new ElasticQuadTree(co_op_engine.Utility.RectangleFloat.FromRectangle(levelBounds), null);
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
            var removed = removeObject.CurrentQuad.Remove(removeObject);
            removed = removed && LinearReference.Remove(removeObject);
            removed = removed && IndexedReference.Remove(removeObject.ID);
            if (!removed)
            {
                throw new Exception("Object containers out of sync");
            }
        }

        public void UpdateAll(GameTime gameTime)
        {
            for (int i = 0; i < LinearReference.Count; i++ )
            {
                LinearReference[i].Update(gameTime);
            }
        }

        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var obj in LinearReference)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch, Texture2D debugTexture)
        {
            SpacialReference.Draw(spriteBatch, debugTexture);
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
