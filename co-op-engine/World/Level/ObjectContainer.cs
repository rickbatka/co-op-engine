using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using Microsoft.Xna.Framework;

namespace co_op_engine.World.Level
{
    class ObjectContainer
    {
        //needs spacial and iterative reference to objects, and probably even a unique indexable heap/binary tree
        ElasticQuadTree quadTree;

        public ObjectContainer(Rectangle levelBounds)
        {
            quadTree = new ElasticQuadTree(co_op_engine.Utility.RectangleFloat.FromRectangle(levelBounds), null);
        }
    }
}
