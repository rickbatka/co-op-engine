using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Collections
{
    /// <summary>
    /// all you need to do is update it and you are good to go
    /// </summary>
    class Animation
    {
        //array of rectangles
        int currentFrameIndex;
        TimeSpan currentFrameTimer;
        Frame[] frames;

        public Rectangle CurrentFrame
        {
            get { return frames[currentFrameIndex].SourceRectangle; }
        }

        //needs data reader system
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
                currentFrameIndex = (currentFrameIndex + 1) % (frames.Length - 1);
                currentFrameTimer = TimeSpan.FromMilliseconds(frames[currentFrameIndex].FrameTime);
            }
        }

        //should be split into data class to pass in
        public static Animation BuildFromAsset(string file)
        {
            //@TODO
            //load file
            //parse info
            //return animation
            throw new NotImplementedException();
        }
    }

    struct Frame
    {
        public Rectangle SourceRectangle;
        public int FrameTime;
    }
}
