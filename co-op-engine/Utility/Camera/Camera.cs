using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components;
using Microsoft.Xna.Framework;

namespace co_op_engine.Utility.Camera
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
        public bool IsTracking;
        private GameObject target;

        List<CameraEffectBase> CurrentEffects;

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
            :this(viewportRect, Vector2.Zero) { }

        private Camera(Rectangle viewportRect, Vector2 position)
        {
            ViewportRectangle = viewportRect;
            Position = position;
            CurrentEffects = new List<CameraEffectBase>();
        }

        public void SetCameraTackingObject(GameObject target)
        {
            this.target = target;
        }

        public void ApplyEffect(CameraEffectBase effect)
        {
            this.CurrentEffects.Add(effect);
            effect.Apply();
        }

        public void Update(GameTime gameTime)
        {
            //do nothing, dependant on others to change it's position
            if (IsTracking)
            {
                Position = new Vector2(target.Position.X - ViewportRectangle.Center.X, target.Position.Y - ViewportRectangle.Center.Y);
            }

            UpdateEffects(gameTime);
        }

        private void UpdateEffects(GameTime gameTime)
        {
            var effectsToRemove = new List<CameraEffectBase>();
            foreach (var effect in CurrentEffects)
            {
                effect.Update(gameTime);

                if (effect.EffectTimeRemaining <= TimeSpan.Zero)
                {
                    effectsToRemove.Add(effect);
                }
            }

            int cnt = effectsToRemove.Count;
            for (int i = 0; i < cnt; i++)
            {
                effectsToRemove[i].Finish();
                CurrentEffects.Remove(effectsToRemove[i]);
            }
        }

        public void Shake()
        {
            this.ApplyEffect(new CameraShakeEffect(this));
        }

        /// <summary>
        /// moves the camera to the specified position
        /// </summary>
        /// <param name="position">location to move the camera</param>
        /*public void CenterCameraOnPosition(Vector2 position)
        {
            Position = new Vector2(position.X - ViewportRectangle.Center.X, position.Y - ViewportRectangle.Center.Y);
        }*/
    }
}
