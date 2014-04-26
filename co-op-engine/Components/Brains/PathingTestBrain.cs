using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Factories;
using co_op_engine.Pathing;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Brains
{
    public class PathingTestBrain : BrainBase
    {
        Path Path;
        float distanceToProgress = 32f;
        bool pursue = false;

        int MoveThreshhold = 100;
        TimeSpan MoveCheckTimer = TimeSpan.Zero;
        int MoveCheckTimerReset = 1000;
        Vector2 moveCheckLastPosition = Vector2.Zero;

        public PathingTestBrain(GameObject owner)
            : base(owner)
        {
            this.Path = new Path(new List<Vector2>()
            {
                new Vector2(10,10),
            });
        }

        private Path GenRandPath()
        {
            if (Vector2.Distance(PlayerFactory.Instance.playerRef_testing_pathing.Position, owner.Position) < 100)
            {
                List<Vector2> path = new List<Vector2>();
                for (int i = 0; i < 1; ++i)
                {
                    path.Add(new Vector2((int)MechanicSingleton.Instance.rand.Next(10, 500), (int)MechanicSingleton.Instance.rand.Next(10, 500)));
                }
                return new Path(path);
            }
            else
            {
                return PathFinder.Instance.GetPath(owner.Position, PlayerFactory.Instance.playerRef_testing_pathing.Position, owner.BoundingBox);
            }
        }

        

        public override void Update(GameTime gameTime)
        {
            if (pursue)
            {
                if ((-owner.Position + Path.CurrentPoint).Length() <= distanceToProgress)
                {
                    if (Path.Points.Count() == 0)
                    {
                        Path = GenRandPath();
                    }
                    else
                    {
                        Path.AdvancePoint();
                    }
                }

                MoveCheckTimer -= gameTime.ElapsedGameTime;
                if (MoveCheckTimer <= TimeSpan.Zero)
                {
                    MoveCheckTimer = TimeSpan.FromMilliseconds(MoveCheckTimerReset);

                    if (Vector2.Distance(moveCheckLastPosition, owner.Position) < MoveThreshhold)
                    {
                        Path = GenRandPath();
                    }

                    moveCheckLastPosition = owner.Position;
                }

                owner.InputMovementVector = Path.CurrentPoint - owner.Position;
            }
            else
            {
                owner.InputMovementVector = Vector2.Zero;
            }

            if (InputHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.X))
            {
                pursue = pursue ? false : true;
                Path = GenRandPath();
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Path.DEBUG_DRAW(spriteBatch);
            DrawingUtility.DrawLine(owner.Position, Path.CurrentPoint, spriteBatch, Color.White);
        }

        public void SetPath(Path path)
        {
            Path = path;
        }
    }
}
