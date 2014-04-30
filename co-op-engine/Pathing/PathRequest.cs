using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Pathing
{
    class PathRequest
    {
        public Vector2 startPosition { get; private set; }
        public Vector2 endPosition { get; private set; }
        public Rectangle collisionBox { get; private set; }
        public Action<Path> callback { get; private set; }

        public PathRequest(Vector2 startPosition, Vector2 endPosition, Rectangle collisionBox, Action<Path> callback)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.collisionBox = collisionBox;
            this.callback = callback;
        }
    }
}
