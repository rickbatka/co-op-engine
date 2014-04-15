using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility.Camera
{
    public class CameraEffectBase
    {
        public TimeSpan EffectTimeRemaining;
        protected int TotalEffectTime;

        public virtual void Apply()
        {
            EffectTimeRemaining = TimeSpan.FromMilliseconds(TotalEffectTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            EffectTimeRemaining -= gameTime.ElapsedGameTime;
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Finish() { }
    }
}
