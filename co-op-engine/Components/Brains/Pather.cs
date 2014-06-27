using co_op_engine.Pathing;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains
{
    public class Pather
    {
        Path Path;
        BrainBase OwningBrain;
        GameObject Owner;
        GameObject Target;
        float distanceToProgress = 32f;

        int MoveThreshhold = 20;
        TimeSpan MoveCheckTimer = TimeSpan.Zero;
        int MoveCheckTimerReset = 2000;
        Vector2 moveCheckLastPosition = Vector2.Zero;

        public Pather(BrainBase owningBrain, GameObject owner)
        {
            this.OwningBrain = owningBrain;
            this.Owner = owner;
        }

        public void SetTarget(GameObject target, bool startPathingImmediately = true)
        {
            this.Target = target;
            if (startPathingImmediately)
            {
                RequestPath();
            }
        }

        public void RequestPath()
        {
            if (Target != null && Target.Position != null)
            {
                PathFinder.RequestPath(Owner.Position, Target.Position, Owner.PhysicsCollisionBox, SetPath);
            }
        }

        public void SetPath(Path path)
        {
            Path = path;
        }

        public void ReleasePath()
        {
            Path = null;
        }

        public bool HasPath()
        {
            return Path != null;
        }

        public void Update(GameTime gameTime)
        {
            if (Path != null)
            {
                if ((-Owner.Position + Path.CurrentPoint).Length() <= distanceToProgress)
                {
                    if (Path.Points.Count() == 0)
                    {
                        RequestPath();
                    }
                    else
                    {
                        Path.AdvancePoint();
                        Owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(Path.LastOrCurrent() - Owner.Position);

                        OwningBrain.SendUpdate(new co_op_engine.Components.Brains.PlayerBrain.PlayerBrainUpdateParams()
                        {
                            InputMovementVector = Owner.InputMovementVector,
                            Position = Owner.Position,
                            RotationTowardFacingDirectionRadians = Owner.RotationTowardFacingDirectionRadians,
                            CurrentState = Owner.CurrentState
                        });
                    }
                }

                MoveCheckTimer -= gameTime.ElapsedGameTime;
                if (MoveCheckTimer <= TimeSpan.Zero)
                {
                    MoveCheckTimer = TimeSpan.FromMilliseconds(MoveCheckTimerReset);

                    if (Vector2.Distance(moveCheckLastPosition, Owner.Position) < MoveThreshhold)
                    {
                        RequestPath();
                    }

                    moveCheckLastPosition = Owner.Position;
                }

                Owner.InputMovementVector = Path.CurrentPoint - Owner.Position;
            }
            else
            {
                //Owner.InputMovementVector = Vector2.Zero;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Path != null)
            {
                //DEBUGDRAW PATH
                Path.DEBUG_DRAW(spriteBatch);
                DrawingUtility.DrawLine(Owner.Position, Path.CurrentPoint, 1, AssetRepository.Instance.DebugFillTexture, spriteBatch, Color.White);
            }
        }
    }
}
