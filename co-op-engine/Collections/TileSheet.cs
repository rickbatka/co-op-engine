using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Collections
{
    public class TileSheet
    {
        //@TODO
        // change int to enumeration once we've figured out enumations
        Dictionary<int, AnimatedRectangle> animations;
        private int currentState = 0;

        //@TODO stub constructor that hardcodes one single animation, need it to actually read list of animations from file
        public TileSheet(AnimatedRectangle animatedRectangle)
        {
            animations = new Dictionary<int, AnimatedRectangle>();
            animations.Add(0, animatedRectangle);
        }

        public AnimatedRectangle GetCurrentAnimationRectangle()
        {
            var anim = TryGetAnimation(currentState);
            if (anim == null)
            {
                anim = TryGetAnimation(0);
            }

            return anim;
        }

        public void Update(GameTime gameTime)
        {
            var curAnimation = TryGetAnimation(currentState);
            if (curAnimation != null)
            {
                curAnimation.Update(gameTime);
            }
        }

        //@TODO another enumeration deferral of decision, takes an enum 
        //and tries to get the animation for it(will do nothing if we
        //don't set an animation) did this because it's generic enough 
        //for things that may not have walking animations but walk....
        public AnimatedRectangle TryGetAnimation(int state)
        {
            if (animations.ContainsKey(state))
            {
                return animations[state];
            }
            return null;
        }

        public static void ReadFromFile(string filename)
        {
            //@TODO
            //read file
            //parse file
            //separate out into grid
            //resolve grid to state enumeration
            //build up dictionary of animations

            //lets assume syntax
            //<enumeration>
            //{
            //  data
            //  data
            //  data
            //}
            //<enumeration>
            //{
            //  data
            //  data
            //  data
            //}

            throw new NotImplementedException();
        }
    }
}
