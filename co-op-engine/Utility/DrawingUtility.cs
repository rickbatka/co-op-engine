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
        public static void DrawLine(Vector2 a, Vector2 b, SpriteBatch spritebatch)
        {
            //Vector2 b = v.Length() > r.Length() ? v : r;
            //Vector2 a = v.Length() > r.Length() ? r : v;

            spritebatch.Draw(AssetRepository.Instance.DebugFillTexture,
                new Rectangle((int)(a.X), (int)(a.Y), (int)(1), (int)( Math.Abs( (a-b).Length() ))),
                null,
                Color.White,
                Vector2ToRadian(b - a),
                new Vector2(1,1),
                SpriteEffects.None,
                0f);
        }

        public static float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        public static float EuclideanRadianToXnaRadian(float direction)
        {
            return (float)Math.Atan2(Math.Cos(direction), (float)Math.Sin(direction));
        }

        public static RectangleFloat VectorToPointRect(Vector2 point)
        {
            return new RectangleFloat(point.X, point.Y, 1, 1);
        }

        public static Vector2 GetAbsolutePosition(IRenderable owner, Point relativePosition)
        {
            return DrawingUtility.RotateVectorAboutOrigin(
                fThetaRadian: owner.RotationTowardFacingDirectionRadians,
                point: (new Vector2(relativePosition.X + owner.Position.X - (float)(owner.CurrentFrame.SourceRectangle.Width / 2f), relativePosition.Y + owner.Position.Y - (float)(owner.CurrentFrame.SourceRectangle.Height / 2f))),
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
    }
}
