using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace co_op_engine.Collections
{
    public class AnimationSet
    {
        public static readonly int ANIM_STATE_DEFAULT_IDLE_SOUTH = 0;

        // indexed as such: animations[state, direction]
        Dictionary<int, AnimatedRectangle[]> animations = new Dictionary<int, AnimatedRectangle[]>();
        public int currentState = ANIM_STATE_DEFAULT_IDLE_SOUTH;
        public int currentFacingDirection = Constants.South;

        public AnimationSet() { }

        public static AnimationSet BuildFromAsset(string file)
        {
            var animationSet = new AnimationSet();
            var lines = File.ReadAllLines(file);
            int animationIndex = 0;
            int directionIndex = 0;

            List<string> currentlyBuildingAnimationLines = new List<string>();
            foreach (var line in lines)
            { 
                if(line.StartsWith(";"))
                {
                    if (currentlyBuildingAnimationLines.Count > 0)
                    {
                        if (!animationSet.animations.ContainsKey(animationIndex))
                        {
                            animationSet.animations.Add(animationIndex, new AnimatedRectangle[4]);
                        }
                        animationSet.animations[animationIndex][directionIndex] = AnimatedRectangle.BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>());
                    }
                    var indexes = line.Split(';');
                    animationIndex = int.Parse(indexes[1]);
                    directionIndex = int.Parse(indexes[2]);
                    currentlyBuildingAnimationLines = new List<string>();
                    continue;
                }
                currentlyBuildingAnimationLines.Add(line);
            }

            if (currentlyBuildingAnimationLines.Count > 0)
            {
                //dont forget the last animation! repeated code...
                if (!animationSet.animations.ContainsKey(animationIndex))
                {
                    animationSet.animations.Add(animationIndex, new AnimatedRectangle[4]);
                }
                animationSet.animations[animationIndex][directionIndex] = AnimatedRectangle.BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>());
            }

            return animationSet;
        }

        public AnimatedRectangle GetAnimationFallbackToDefault(int state, int facingDirection)
        {
            return TryGetAnimation(state, facingDirection)
                ?? TryGetAnimation(state, Constants.South)
                ?? TryGetAnimation(ANIM_STATE_DEFAULT_IDLE_SOUTH, Constants.South);
        }

        public int GetAnimationDuration(int state, int facingDirection)
        {
            var anim = GetAnimationFallbackToDefault(state, facingDirection);

            if (anim == null)
            {
                return 0;
            }

            return anim.AnimationDuration();
        }

        public void Update(GameTime gameTime)
        {
            var curAnimation = GetAnimationFallbackToDefault(currentState, currentFacingDirection);
            if (curAnimation != null)
            {
                curAnimation.Update(gameTime);
            }
        }

        private AnimatedRectangle TryGetAnimation(int state, int facingDirection)
        {
            if (animations.ContainsKey(state))
            {
                return animations[state][facingDirection];
            }
            return null;
        }
    }
}
