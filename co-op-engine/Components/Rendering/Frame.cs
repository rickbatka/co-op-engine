using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Rendering
{
    public struct Frame
    {
        public Rectangle SourceRectangle;
        public Rectangle PhysicsRectangle;
        public Rectangle[] DamageDots;
        public int FrameTime;
    }

    public static class FrameDataReader
    {
        public static Frame BuildFromDataLine(string lineData)
        {
            Rectangle sourceRectangle = readRectangles(lineData, '<', '>').First();

            Rectangle physicsRectangle = readRectangles(lineData, '{', '}').FirstOrDefault();

            List<Rectangle> damageDots = readRectangles(lineData, '(', ')');

            int time = int.Parse(lineData.Substring(0, lineData.IndexOf('<')));

            return new Frame()
            {
                FrameTime = time,
                SourceRectangle = sourceRectangle,
                PhysicsRectangle = physicsRectangle,
                DamageDots = damageDots.ToArray()
            };
        }

        private static List<Rectangle> readRectangles(string lineData, char leftSep, char rightSep)
        {
            int rectCount = lineData.Count(c => c == leftSep);
            List<Rectangle> rectList = new List<Rectangle>();
            int currentIndex = 0;
            for (int j = 0; j < rectCount; ++j)
            {
                lineData = lineData.Replace(" ", "").Replace("\t", "");
                var data = lineData.Substring(lineData.IndexOf(leftSep, currentIndex) + 1, lineData.IndexOf(rightSep, currentIndex) - lineData.IndexOf(leftSep, currentIndex) - 1).Split(',');
                rectList.Add(new Rectangle(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])));
                currentIndex = lineData.IndexOf(rightSep, currentIndex) + 1;
            }
            return rectList;
        }
    }
}
