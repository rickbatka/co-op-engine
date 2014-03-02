using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public static class DrawingUtility
    {
        public static float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        public static float EuclideanRadianToXnaRadian(float direction)
        {
            return (float)Math.Atan2(Math.Cos(direction), (float)Math.Sin(direction));
        }
    }
}
