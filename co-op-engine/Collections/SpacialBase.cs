using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using co_op_engine.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Collections
{
    public abstract class SpacialBase
    {
        abstract public RectangleFloat Dimensions { get; }
        abstract public void MasterInsert(GameObject newObject);
        abstract public List<GameObject> MasterQuery(RectangleFloat query);
        abstract public bool Remove(GameObject removeObject);
        abstract public void Draw(SpriteBatch spriteBatch);
        abstract public bool Insert(GameObject newObject);
        abstract public void NotifyOfMovement(GameObject ownedObject);
    }
}
