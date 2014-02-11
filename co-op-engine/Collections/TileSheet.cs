using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Collections
{
    class TileSheet
    {
        //@TODO
        // change int to enumeration once we've figured out enumations
        Dictionary<int, Animation> animations;

        //@TODO another enumeration deferral of decision, takes an enum 
        //and tries to get the animation for it(will do nothing if we
        //don't set an animation) did this because it's generic enough 
        //for things that may not have walking animations but walk....
        public void TryGetAnimation(int state, ref Animation currentAnimation)
        {
            if (animations.ContainsKey(state))
            {
                currentAnimation = animations[state];
            }
        }

        public static void ReadFromFile(string filename)
        {
            //@TODO
            //read file
            //parse file
            //separate out into grid
            //resolve grid to state enumeration
            //build up dictionary of animations
            throw new NotImplementedException();
        }
    }
}
