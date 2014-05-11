using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using co_op_engine.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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

        public AnimationToolSystem()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        internal void SetFile(string filename, ContentManager content)
        {
            throw new NotImplementedException();
        }
    }
}
