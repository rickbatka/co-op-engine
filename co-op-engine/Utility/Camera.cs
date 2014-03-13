using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Utility
{
    /// <summary>
    /// Provides a scrolling viewpoint into the game world
    /// </summary>
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
        private float speed = 3f;
        public float Speed { get { return speed; } set { speed = value; } }
        private float zoom = 1f;
        public float Zoom { get { return zoom; } set { zoom = value; } }
        public Rectangle ViewportRectangle { get; private set; }

        /// <summary>
        /// the transformation matrix to be applied to the renderer
        /// </summary>
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
            ViewportRectangle = viewportRect;
        }

        private Camera(Rectangle viewportRect, Vector2 position)
        {
            ViewportRectangle = viewportRect;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            //do nothing, dependant on others to change it's position
        }

        /// <summary>
        /// moves the camera to the specified position
        /// </summary>
        /// <param name="position">location to move the camera</param>
        public void CenterCameraOnPosition(Vector2 position)
        {
            Position = new Vector2(position.X - ViewportRectangle.Center.X, position.Y - ViewportRectangle.Center.Y);
        }
    }
}
