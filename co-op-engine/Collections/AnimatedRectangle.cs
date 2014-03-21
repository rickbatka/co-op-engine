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

        public Rectangle CurrentDrawRectangle
        {
            get { return frames[currentFrameIndex].SourceRectangle; }
        }

        public Rectangle CurrentFrameRectangle
        {
            get { return frames[currentFrameIndex].SourceRectangle; }
        }

        public Frame CurrentFrame { get { return frames[currentFrameIndex]; } }

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

        public int AnimationDuration()
        { 
            if(frames.Count() == 0)
            {
                return 0;
            }

            int duration = 0;
            foreach(var frame in frames)
            {
                duration += frame.FrameTime;
            }
            return duration;
        }

        public static AnimatedRectangle BuildFromDataLines(string[] lineData)
        {
            List<Frame> frameList = new List<Frame>();

            for (int i = 0; i < lineData.Length; ++i)
            {
                // parse in frame rectangles
                Rectangle frameRectangle = readRectangles(lineData[i], '<', '>').First();

                // parse in damage dots (1x1 rectangles)
                List<Rectangle> damageDots = readRectangles(lineData[i], '(', ')');

                // parse in frame time
                int time = int.Parse(lineData[i].Substring(0, lineData[i].IndexOf('<')));

                frameList.Add(new Frame()
                {
                    FrameTime = time,
                    SourceRectangle = frameRectangle,
                    DamageDots = damageDots.ToArray()
                });
            }

            return new AnimatedRectangle(frameList.ToArray());
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

    public struct Frame
    {
        public Rectangle SourceRectangle;
        public Rectangle[] DamageDots;
        // drawrecctangle
        // physicsrectangle
        // origin?
        public int FrameTime;
    }
}
