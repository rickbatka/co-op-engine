using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine
{
    public static class VectorExtensions
    {
        public static Vector2 Rounded(this Vector2 vector)
        {
            Vector2 rounded = new Vector2(Round(vector.X), Round(vector.Y));
            return rounded;
        }

        public static float Round(float x)
        {
            int sign = (x < 0) ? -1 : 1;

            x = Math.Abs(x);

            float remainder = Math.Abs(x % 1);

            if (remainder == 0f) { return x * sign; } // no rounding, early return

            if (remainder < 0.5f) // round down
            {
                x -= remainder;
            }
            else //round up
            {
                x += (1 - remainder);
            }

            return x * sign;
        }
    }
}
