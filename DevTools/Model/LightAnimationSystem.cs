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
        public List<LightFrame> frames;
        public int currentFrameIndex { get; private set; }
        TimeSpan currentFrameTimer;
        public int FrameCount { get { return frames.Count; } }

        public LightFrame CurrentFrame
        {
            get { return frames[currentFrameIndex]; }
        }

        public LightAnimation(List<LightFrame> frameList)
        {
            frames = frameList;
            currentFrameIndex = 0;
            currentFrameTimer = TimeSpan.Zero;
        }

        public void DrawAndUpdate(SpriteBatch spriteBatch, TimeSpan elapsedTime, Texture2D spriteSheet, float timescale, Texture2D grid)
        {
            if (timescale != 0)
            {
                currentFrameTimer -= TimeSpan.FromMilliseconds(elapsedTime.TotalMilliseconds * timescale);
                if (currentFrameTimer <= TimeSpan.Zero)
                {
                    AdvanceFrameAndReset();
                }
            }

            //threading issue
            if (currentFrameIndex >= frames.Count)
            {
                currentFrameIndex = 0;
            }

            if (frames.Count != 0)
            {
                spriteBatch.Draw(spriteSheet, frames[currentFrameIndex].DrawRectangle, frames[currentFrameIndex].SourceRectangle, Color.White);

                spriteBatch.Draw(grid, frames[currentFrameIndex].DrawRectangle, Color.Red);
                spriteBatch.Draw(grid, frames[currentFrameIndex].SourceRectangle, Color.Blue);
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

        internal void DrawPhysics(SpriteBatch spriteBatch, Texture2D debugTex, Color color)
        {
            spriteBatch.Draw(debugTex, frames[currentFrameIndex].PhysicsRectangle, color);
        }

        internal void DrawDamageDots(SpriteBatch spriteBatch, Texture2D debugTex, Color color)
        {
            foreach (var dot in frames[currentFrameIndex].DamageDots)
            {
                spriteBatch.Draw(debugTex, dot, color);
            }
        }

        internal void SetIndex(int value)
        {
            currentFrameIndex = value;
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


}
