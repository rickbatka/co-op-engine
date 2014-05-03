using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;

namespace co_op_engine.Pathing
{
    public class Path
    {
        //a series of positions and possibly
        public Vector2 CurrentPoint;
        public List<Vector2> Points;

        public Path(List<Vector2> points)
        {
            Points = points;
            AdvancePoint();
        }

        public void AdvancePoint()
        {
            CurrentPoint = Points.First();
            Points.Remove(CurrentPoint);
        }

        public int PointsLeft()
        {
            return Points.Count();
        }

        public void DEBUG_DRAW(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (Points.Count() > 0 && CurrentPoint != null)
            {
                DrawingUtility.DrawLine(CurrentPoint, Points[0], 1,AssetRepository.Instance.DebugFillTexture, spriteBatch, Color.Red);
            }

            for (int i = 1; i < Points.Count(); ++i)
            {
                DrawingUtility.DrawLine(Points[i - 1], Points[i], 1, AssetRepository.Instance.DebugFillTexture, spriteBatch, Color.Red);
            }
        }
    }
}
