using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Model
{
    class AnimationToolSystem
    {
        //current file
        
        //current animation data model
        //info to run specific animation (direction/action)

        //animation timers

        //timescale?
        public float Timescale { get; set; }


        public string FileName { get; set; }

        internal void SetTimeScale(int p)
        {
            throw new NotImplementedException();
        }

        internal void SetFile(string value)
        {
            throw new NotImplementedException();
        }
    }
}
