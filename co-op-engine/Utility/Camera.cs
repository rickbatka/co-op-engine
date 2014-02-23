using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Utility
{
    public class Camera
    {
        static public Camera Instance;
        static public void Instantiate(Rectangle ViewportRect)
        {
            if (Instance == null)
            {
                Instance = new Camera(ViewportRect);
            }
        }


        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public float Zoom { get; set; }
        public Rectangle ViewportRectangle { get; private set; }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        private Camera(Rectangle viewportRect)
        {
            Position = Vector2.Zero;
            Speed = 4f;
            Zoom = 1f;
            ViewportRectangle = viewportRect;
        }

        private Camera(Rectangle viewportRect, Vector2 position)
        {
            Speed = 4f;
            Zoom = 1f;
            ViewportRectangle = viewportRect;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            //do nothing, dependant on others to change it's position
        }

        public void CenterCameraOnPosition(Vector2 position)
        {
            Position = new Vector2(position.X - ViewportRectangle.Center.X, position.Y - ViewportRectangle.Center.Y);
        }
    }
}
