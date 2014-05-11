using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using co_op_engine.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using co_op_engine.Components.Rendering;

namespace DevTools.Model
{
    class AnimationToolSystem
    {
        //current file
        string spritesheetFilename;
        string metadataFilename;
        AnimationSet animations;

        private TimeSpan frameTimer;
        public float Timescale { get; set; }
        public string FileName { get; set; }

        private bool isLoaded = false;
        private DateTime lastHit;
        private Texture2D currentTexture;

        public AnimationToolSystem()
        {
            lastHit = DateTime.Now;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //update logic
            TimeSpan elapsedTime = HitAndGetInterval();
            //animations.Update(elapsedTime);

            //draw logic
            //Frame currentFrame = animations.CurrentAnimatedRectangle.CurrentFrame;
            
            /*spriteBatch.Draw(
                currentTexture,
                Vector2.Zero,
                currentFrame.SourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                .1f
            );*/
        }

        internal void SetFile(string filename, ContentManager content)
        {
            //should be UI enforced
            FileInfo fileInfo = new FileInfo(filename);
            currentTexture = content.Load<Texture2D>(fileInfo.Name.Replace(fileInfo.Extension, ""));
        }

        private TimeSpan HitAndGetInterval()
        {
            DateTime swap = lastHit;
            lastHit = DateTime.Now;
            return lastHit - swap;
        }
    }
}
