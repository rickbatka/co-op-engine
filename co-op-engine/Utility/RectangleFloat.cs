<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
=======
﻿using Microsoft.Xna.Framework;
using System;
>>>>>>> vector rounding for camera / draw positioning, better than casting to ints

namespace co_op_engine.Utility
{
    /// <summary>
    /// A rectangle with floating point values
    /// </summary>
    public struct RectangleFloat
    {
        public float X;
        public float Y;
        public float Height;
        public float Width;

        public RectangleFloat(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static bool operator !=(RectangleFloat a, RectangleFloat b)
        {
            return !(a == b);
        }

        public static bool operator ==(RectangleFloat a, RectangleFloat b)
        {
            return a.Equals(b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(RectangleFloat))
            {
                var a = (RectangleFloat)obj;
                return (a.X == this.X && a.Y == this.Y && a.Width == this.Width && a.Height == this.Height);
            }
            return false;
        }

        public float Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(X + (Width / 2), Y + (Height / 2));
            }
        }

        public static RectangleFloat Empty
        {
            get
            {
                return new RectangleFloat(0, 0, 0, 0);
            }
        }

        public bool IsEmpty 
        {
            get
            {
                return this == Empty;
            }
        }

        public float Left 
        {
            get
            {
                return X;
            }
        }

        public float Right 
        {
            get
            {
                return X + Width;
            }
        }

        public float Top 
        {
            get
            {
                return Y;
            }
        }

        public bool ContainsInclusive(Point value)
        {
            return (value.X <= Right && value.X >= Left && value.Y >= Top && value.Y <= Bottom);
        }

        public bool ContainsInclusive(Vector2 value)
        {
            return (value.X <= Right && value.X >= Left && value.Y >= Top && value.Y <= Bottom);
        }

        public bool ContainsInclusive(float x, float y)
        {
            return (x <= Right && x >= Left && y >= Top && y <= Bottom);
        }

        public void Inflate(float horizontalValue, float verticalValue)
        {
            X -= horizontalValue;
            Width += horizontalValue * 2;
            Y -= verticalValue;
            Height += verticalValue * 2;
        }

        public bool Intersects(RectangleFloat value)
        {
            //this left < other right
            //this right > other left
            //this top < other bottom
            //this bottom > other top

            return (Left < value.Right && Right > value.Left && Top < value.Bottom && Bottom > value.Top);
        }

        public bool Intersects(Rectangle value)
        {
            return (Left < value.Right && Right > value.Left && Top < value.Bottom && Bottom > value.Top);
        }

        public static RectangleFloat FromRectangle(Rectangle input)
        {
            return new RectangleFloat(input.X, input.Y, input.Width, input.Height);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Width), (int)Math.Round(Height));
        }

        //public static implicit operator Rectangle(RectangleFloat rectf)
        //{
        //    return rectf.ToRectangle();
        //}

        public static implicit operator RectangleFloat(Rectangle rect)
        {
            return FromRectangle(rect);
        }
    }
}
