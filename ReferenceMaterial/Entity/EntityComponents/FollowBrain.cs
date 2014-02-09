//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using ReferenceMaterial.Entity;

//namespace ReferenceMaterial.Entity.EntityComponents
//{
//    /// <summary>
//    /// follows a game object up to a certain distance.
//    /// </summary>
//    class FollowBrain : BrainBase
//    {
//        private int timerMilli;
//        private TimeSpan timeLeft;
//        private float followSpeed;
//        private float wanderSpeed;
//        private float closeness;
//        private float noticeDistance;

//        //GameWorld worldRef;

//        public GameObject followTarget;

//        public FollowBrain(int AITimerGranularity, float speed, float idleSpeed, float sightRange, float followDistance, GameObject owner)
//            : base(owner)
//        {
//            timerMilli = AITimerGranularity;
//            closeness = followDistance;
//            noticeDistance = sightRange;
//            followSpeed = speed;
//            wanderSpeed = idleSpeed;
//            //worldRef = world;
//            timerMilli = AITimerGranularity;
//            timeLeft = TimeSpan.Zero;
//        }

//        public override void Update(GameTime gameTime)
//        {
//            timeLeft -= gameTime.ElapsedGameTime;

//            if (timeLeft <= TimeSpan.Zero)
//            {
//                UpdateMovement();
//                timeLeft = TimeSpan.FromMilliseconds(timerMilli);
//            }
//        }

//        private void UpdateMovement()
//        {
//            if (followTarget != null)
//            {
//                if (Vector2.Distance(owner.PhysicsComponent.Position, followTarget.PhysicsComponent.Position) > closeness)
//                {
//                    MoveDirection = followTarget.PhysicsComponent.Position - owner.PhysicsComponent.Position;
//                    MoveDirection.Normalize();
//                    MoveDirection *= followSpeed;
//                }
//                else
//                {
//                    //MoveDirection = new Vector2(Randomizer.Instance.Rand.Next(-30, 30), Randomizer.Instance.Rand.Next(-30, 30));
//                    if (MoveDirection != Vector2.Zero)
//                    {
//                        MoveDirection.Normalize();
//                        MoveDirection *= wanderSpeed;
//                    }
//                }
//            }
//            else
//            {
//                //target acquire logic here
//            }
//        }

//        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
//        {
//        }
//    }
//}
