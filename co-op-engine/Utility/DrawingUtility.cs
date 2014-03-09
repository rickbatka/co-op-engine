using co_op_engine.Components.Rendering;
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

        public static Vector2 GetAbsolutePosition(IRenderable owner, Point relativePosition)
        {
            return DrawingUtility.RotateVectorAboutOrigin(
                fThetaRadian: owner.RotationTowardFacingDirectionRadians,
                point: (new Vector2(relativePosition.X + owner.Position.X - (float)(owner.Width / 2f), relativePosition.Y + owner.Position.Y - (float)(owner.Height / 2f))),
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
