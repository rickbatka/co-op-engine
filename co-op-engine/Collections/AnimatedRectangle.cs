using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Collections
{
    /// <summary>
    /// all you need to do is update it and you are good to go
    /// </summary>
    public class AnimatedRectangle
    {
        //array of rectangles
        int currentFrameIndex;
        TimeSpan currentFrameTimer;
        Frame[] frames;

#warning THIS NEEDS TO BE RELATIVE DERPPPPPP, need to do transform off given facing direction (shouldn't be too hard once I think about it)
        public Rectangle CurrentDrawRectangle
        {
            get { return frames[currentFrameIndex].SourceRectangles[0]; }
        }

        public Rectangle[] CurrentFrameRectangles
        {
            get { return frames[currentFrameIndex].SourceRectangles; }
        }

        //needs data reader system
        private AnimatedRectangle(Frame[] frames)
        {
            currentFrameIndex = 0;
            currentFrameTimer = TimeSpan.FromMilliseconds(frames[0].FrameTime);
            this.frames = frames;
        }

        public void Reset()
        {
            currentFrameIndex = 0;
            currentFrameTimer = TimeSpan.FromMilliseconds(frames[0].FrameTime);
        }

        public void Update(GameTime gameTime)
        {
            currentFrameTimer -= gameTime.ElapsedGameTime;
            if (currentFrameTimer <= TimeSpan.Zero)
            {
                currentFrameIndex = (currentFrameIndex + 1) > (frames.Length - 1) ? 0 : currentFrameIndex + 1;
                currentFrameTimer = TimeSpan.FromMilliseconds(frames[currentFrameIndex].FrameTime);
            }
        }

        public static AnimatedRectangle BuildFromDataLines(string[] lineData)
        {
            List<Frame> frameList = new List<Frame>();

            for (int i = 0; i < lineData.Length; ++i)
            {
                int rectCount = lineData[i].Count(c => c == '<');
                List<Rectangle> rectList = new List<Rectangle>();
                int currentIndex = 0;
                for (int j = 0; j < rectCount; ++j)
                {
                    lineData[i] = lineData[i].Replace(" ","").Replace("\t","");
                    var data = lineData[i].Substring(lineData[i].IndexOf('<', currentIndex) + 1, lineData[i].IndexOf('>', currentIndex) - lineData[i].IndexOf('<', currentIndex) - 1).Split(',');
                    rectList.Add(new Rectangle(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])));
                    currentIndex = lineData[i].IndexOf('>', currentIndex) + 1;
                }

                int time = int.Parse(lineData[i].Substring(0, lineData[i].IndexOf('<')));

                frameList.Add(new Frame()
                {
                    FrameTime = time,
                    SourceRectangles = rectList.ToArray()
                });
            }

            return new AnimatedRectangle(frameList.ToArray());
        }
    }

    struct Frame
    {
        public Rectangle[] SourceRectangles;
        public int FrameTime;
    }
}
