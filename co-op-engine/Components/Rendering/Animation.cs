using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using co_op_engine.Components.Rendering;

namespace co_op_engine.Rendering
{
    /// <summary>
    /// all you need to do is update it and you are good to go
    /// </summary>
    public class Animation
    {
        //array of rectangles
        int currentFrameIndex;
        TimeSpan currentFrameTimer;
        Frame[] frames;

        public Frame CurrentFrame { get { return frames[currentFrameIndex]; } }

        private Animation(Frame[] frames)
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

        public static Animation BuildFromDataLines(string[] lineData)
        {
            List<Frame> frameList = new List<Frame>();

            for (int i = 0; i < lineData.Length; ++i)
            {
                frameList.Add(FrameDataReader.BuildFromDataLine(lineData[i]));
            }

            return new Animation(frameList.ToArray());
        }
    }
}
