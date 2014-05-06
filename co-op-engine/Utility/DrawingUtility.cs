using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Utility
{
    public static class DrawingUtility
    {
        public static void DrawLine(Vector2 a, Vector2 b, int width, Texture2D texture, SpriteBatch spritebatch, Color color)
        {
            //Vector2 b = v.Length() > r.Length() ? v : r;
            //Vector2 a = v.Length() > r.Length() ? r : v;

            spritebatch.Draw(
                texture: texture,
                destinationRectangle: new Rectangle((int)(a.X), (int)(a.Y), (int)(width), (int)(Math.Abs((a - b).Length()))),
                sourceRectangle: null,
                color: color,
                rotation: Vector2ToRadian(a - b),
                origin: new Vector2(texture.Width / 2, 0),
                effect: SpriteEffects.None,
                depth: 1f
            );
        }

        public static void DrawLineScaledWithImageJittered(Vector2 a, Vector2 b, int width, Texture2D texture, SpriteBatch spriteBatch, Color color)
        {
            //build scale get length, if length > scale stretch, otherwise crops sourceRectangle



            float distance = Vector2.Distance(a, b);

            int scaledHeight = (int)(texture.Height * ((float)width / texture.Width));

            //scaled height
            if (distance > scaledHeight)
            {
                //stretch
                int offset = MechanicSingleton.Instance.rand.Next(0, texture.Height - 10);

                spriteBatch.Draw(
                    texture: texture,
                    destinationRectangle: new Rectangle((int)(a.X), (int)(a.Y), (int)(width), (int)(Math.Abs((a - b).Length()))),
                    sourceRectangle: new Rectangle(0, offset, texture.Width, texture.Height - 10),
                    color: color,
                    rotation: Vector2ToRadian(a - b),
                    origin: new Vector2(texture.Width / 2, 0),
                    effect: SpriteEffects.None,
                    depth: 1f
                );
            }
            else
            {
                //crop
                int offset = MechanicSingleton.Instance.rand.Next(0, texture.Height - (int)distance);

                spriteBatch.Draw(
                    texture: texture,
                    destinationRectangle: new Rectangle((int)(a.X), (int)(a.Y), (int)(width), (int)(Math.Abs((a - b).Length()))),
                    sourceRectangle: new Rectangle(0, offset, texture.Width, (int)distance),
                    color: color,
                    rotation: Vector2ToRadian(a - b),
                    origin: new Vector2(texture.Width / 2, 0),
                    effect: SpriteEffects.None,
                    depth: 1f
                );
            }


        }

        public static void DrawLineScaledWithImage(Vector2 a, Vector2 b, int width, Texture2D texture, SpriteBatch spriteBatch, Color color)
        {
            float distance = Vector2.Distance(a, b);

            int scaledHeight = (int)(texture.Height * ((float)width / texture.Width));

            if (distance > scaledHeight)
            {
                spriteBatch.Draw(
                    texture: texture,
                    destinationRectangle: new Rectangle((int)(a.X), (int)(a.Y), (int)(width), (int)(Math.Abs((a - b).Length()))),
                    sourceRectangle: new Rectangle(0, 0, texture.Width,texture.Height),
                    color: color,
                    rotation: Vector2ToRadian(a - b),
                    origin: new Vector2(texture.Width / 2, 0),
                    effect: SpriteEffects.None,
                    depth: 1f
                );
            }
            else
            {
                //crop
                spriteBatch.Draw(
                    texture: texture,
                    destinationRectangle: new Rectangle((int)(a.X), (int)(a.Y), (int)(width), (int)(Math.Abs((a - b).Length()))),
                    sourceRectangle: new Rectangle(0, 0, texture.Width, (int)distance),
                    color: color,
                    rotation: Vector2ToRadian(a - b),
                    origin: new Vector2(texture.Width / 2, 0),
                    effect: SpriteEffects.None,
                    depth: 1f
                );
            }


        }

        public static float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        public static Vector2 RadianToVector2(float radian)
        {
            radian = EuclideanRadianToXnaRadian(radian);
            return new Vector2((float)Math.Cos(radian), -(float)Math.Sin(radian));
        }

        public static float EuclideanRadianToXnaRadian(float direction)
        {
            return (float)Math.Atan2(Math.Cos(direction), (float)Math.Sin(direction));
        }

        public static RectangleFloat VectorToPointRect(Vector2 point)
        {
            return new RectangleFloat(point.X, point.Y, 1, 1);
        }

        public static Vector2 PointToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Vector2 GetAbsolutePosition(IRenderable owner, Point relativePosition)
        {
            return DrawingUtility.RotateVectorAboutOrigin(
                fThetaRadian: owner.RotationTowardFacingDirectionRadians,
                point: new Vector2(
                    relativePosition.X + owner.Position.X - (float)(owner.CurrentFrame.DrawRectangle.Width / 2f),
                    relativePosition.Y + owner.Position.Y - (float)(owner.CurrentFrame.DrawRectangle.Height / 2f)
                ),
                origin: owner.Position
            );
        }

        public static Vector2 RotateVectorAboutOrigin(float fThetaRadian, Vector2 point, Vector2 origin)
        {
            float newX = point.X - origin.X;
            float newY = point.Y - origin.Y;

            float cosT = (float)Math.Cos(fThetaRadian);
            float sinT = (float)Math.Sin(fThetaRadian);

            var vectorRes = new Vector2(
                cosT * newX - sinT * newY,
                sinT * newX + cosT * newY
            );
            vectorRes += origin;
            return vectorRes;
        }

        public static Vector2 EaseInOutLinear(Vector2 start, Vector2 change, float duration, float currentTime)
        {
            return new Vector2(
                EaseInOutLinear(start.X, change.X, duration, currentTime),
                EaseInOutLinear(start.Y, change.Y, duration, currentTime)
            );
        }

        public static float EaseInOutLinear(float startValue, float changeInValue, float duration, float currentTime)
        {
            return changeInValue * currentTime / duration + startValue;
        }

        public static Vector2 EaseInOutExpo(Vector2 start, Vector2 change, float duration, float currentTime)
        {
            return new Vector2(
                EaseInOutExpo(start.X, change.X, duration, currentTime),
                EaseInOutExpo(start.Y, change.Y, duration, currentTime)
            );
        }

        public static float EaseInOutExpo(float startValue, float changeInValue, float duration, float currentTime)
        {
            currentTime /= duration / 2;
            if (currentTime < 1)
            {
                return changeInValue / 2 * (float)Math.Pow(2, 10 * (currentTime - 1)) + startValue;
            }
            currentTime--;
            return changeInValue / 2 * (float)(-Math.Pow(2, -10 * currentTime) + 2) + startValue;
        }
    }
}
