using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Model
{
    //this whole monogame xna crossover problem is bullshit, they HAD to use the same namespaces....
    class LightAnimation
    {
        List<LightFrame> frames;
        int currentFrameIndex;
        TimeSpan currentFrameTimer;

        public LightAnimation(List<LightFrame> frameList)
        {
            frames = frameList;
            currentFrameIndex = 0;
            currentFrameTimer = TimeSpan.Zero;
        }

        public void InsertFrame(LightFrame frame)
        {
            frames.Add(frame);
        }

        public void DrawAndUpdate(SpriteBatch spriteBatch, TimeSpan elapsedTime, Texture2D spriteSheet)
        {
            currentFrameTimer -= elapsedTime;
            if (currentFrameTimer <= TimeSpan.Zero)
            {
                AdvanceFrameAndReset();
            }


            //threading issue
            if (currentFrameIndex >= frames.Count)
            {
                currentFrameIndex = 0;
            }

            if (frames.Count != 0)
            {
                spriteBatch.Draw(spriteSheet, frames[currentFrameIndex].DrawRectangle, frames[currentFrameIndex].SourceRectangle, Color.White);
            }
        }

        private void AdvanceFrameAndReset()
        {
            if (currentFrameIndex + 1 >= frames.Count)
            {
                currentFrameIndex = 0;
            }
            else
            {
                ++currentFrameIndex;
            }
            currentFrameTimer = TimeSpan.FromMilliseconds(frames[currentFrameIndex].FrameTime);
        }
    }

    class LightFrame
    {
        public Rectangle SourceRectangle;
        public Rectangle DrawRectangle;
        public Rectangle PhysicsRectangle;
        public Rectangle[] DamageDots;
        public int FrameTime;
    }

    static class LightAnimationsDataReader
    {
        


        public static LightFrame BuildFromDataLine(string lineData, float scale)
        {
            Rectangle sourceRectangle = readRectangles(lineData, '<', '>').First();

            Rectangle drawRectangle;

            drawRectangle = new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height);
            drawRectangle.Width = (int)(sourceRectangle.Width * scale);
            drawRectangle.Height = (int)(sourceRectangle.Height * scale);
            //drawRectangle.X = drawRectangle.Width / 2;

            Rectangle physicsRectangle = readRectangles(lineData, '{', '}').FirstOrDefault();
            physicsRectangle = new Rectangle(
                (int)(physicsRectangle.X * scale),
                (int)(physicsRectangle.Y * scale),
                (int)(physicsRectangle.Width * scale),
                (int)(physicsRectangle.Width * scale)
            );

            List<Rectangle> damageDots = readRectangles(lineData, '(', ')');
            for (int i = 0; i < damageDots.Count; i++)
            {
                damageDots[i] = new Rectangle(
                    (int)(damageDots[i].X * scale),
                    (int)(damageDots[i].Y * scale),
                    1,
                    1
                );
            }

            int time = int.Parse(lineData.Substring(0, lineData.IndexOf('<')));

            return new LightFrame()
            {
                FrameTime = time,
                SourceRectangle = sourceRectangle,
                DrawRectangle = drawRectangle,
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
