using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace co_op_engine.Collections
{
    public class AnimationSet
    {
        //@TODO
        // change int to enumeration once we've figured out enumations
        Dictionary<int, AnimatedRectangle> animations = new Dictionary<int,AnimatedRectangle>();
        private int currentState = 0;

        public AnimationSet() { }

        public static AnimationSet BuildFromAsset(string file)
        {
            var animationSet = new AnimationSet();
            var lines = File.ReadAllLines(file);
            int animationIndex = 0;

            List<string> currentlyBuildingAnimationLines = new List<string>();
            foreach (var line in lines)
            { 
                if(line.StartsWith(";"))
                {
                    if (currentlyBuildingAnimationLines.Count > 0)
                    {
                        animationSet.animations.Add(animationIndex, AnimatedRectangle.BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>()));
                    }
                    animationIndex = int.Parse(line.Substring(1));
                    continue;
                }
                currentlyBuildingAnimationLines.Add(line);
            }

            //dont forget the last animation!
            animationSet.animations.Add(animationIndex, AnimatedRectangle.BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>()));

            return animationSet;
        }

        public AnimatedRectangle GetCurrentAnimationRectangle()
        {
            return TryGetAnimation(currentState) ?? TryGetAnimation(0);
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
