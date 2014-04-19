using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Pathing;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Brains
{
    public class PathingTestBrain : BrainBase
    {
        Path Path;
        float distanceToProgress = 32f;


        public PathingTestBrain(GameObject owner)
            : base(owner)
        {
            this.Path = new Path(new List<Vector2>()
            {
                new Vector2(10,10),
                new Vector2(500,10),
                new Vector2(10,500),
                new Vector2(500,500),
            });
        }

        private Path GenRandPath()
        {
            List<Vector2> path = new List<Vector2>();
            for (int i = 0; i < 20; ++i)
            {
                path.Add(new Vector2((int)MechanicSingleton.Instance.rand.Next(10, 500), (int)MechanicSingleton.Instance.rand.Next(10, 500)));
            }
            return new Path(path);
        }

        public override void Update(GameTime gameTime)
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
            owner.InputMovementVector = Path.CurrentPoint - owner.Position;
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
