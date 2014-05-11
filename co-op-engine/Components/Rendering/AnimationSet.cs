using co_op_engine.Rendering;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace co_op_engine.Collections
{
    public class AnimationSet
    {
        public static readonly int ANIM_STATE_DEFAULT_IDLE_SOUTH = 0;

        // indexed as such: animations[state, direction]
        Dictionary<int, Animation[]> animations = new Dictionary<int, Animation[]>();
        public int currentState = ANIM_STATE_DEFAULT_IDLE_SOUTH;
        public int currentFacingDirection = Constants.South;

        public Animation CurrentAnimatedRectangle { get { return GetAnimationFallbackToDefault(currentState, currentFacingDirection); } }

        public AnimationSet() { }

        public static AnimationSet BuildFromAsset(string[] lines, float scaleOverride = 1.0f)
        {
            var animationSet = new AnimationSet();
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
                            animationSet.animations.Add(animationIndex, new Animation[4]);
                        }
                        animationSet.animations[animationIndex][directionIndex] = Animation.BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>(), scaleOverride);
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
                    animationSet.animations.Add(animationIndex, new Animation[4]);
                }
                animationSet.animations[animationIndex][directionIndex] = Animation.BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>(), scaleOverride);
            }

            return animationSet;
        }

        public Animation GetAnimationFallbackToDefault(int state, int facingDirection)
        {
            return GetAnimation(state, facingDirection)
                ?? GetAnimation(state, Constants.South)
                ?? GetAnimation(ANIM_STATE_DEFAULT_IDLE_SOUTH, Constants.South);
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

        private Animation GetAnimation(int state, int facingDirection)
        {
            if (animations.ContainsKey(state))
            {
                return animations[state][facingDirection];
            }
            return null;
        }
    }
}
