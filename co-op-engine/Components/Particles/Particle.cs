using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility.Camera;

namespace co_op_engine.Components.Particles
{
    public interface IParticle
    {
        RectangleFloat DrawRectangle { get; set; }
        Texture2D Texture { get; }
        Color DrawColor { get; }
        bool IsAlive { get; }
        TimeSpan Lifetime { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Position { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        void Begin();
        void Update(GameTime gameTime);
        void End();
        void Draw(SpriteBatch spriteBatch);
    }

    public class Particle : IParticle
    {
        public bool IsAlive { get; set; }
        public TimeSpan Lifetime { get; set; }
        public Vector2 Velocity { get; set; }

        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if (DrawRectangle != null)
                {
                    drawRectangle.X = _position.X;
                    drawRectangle.Y = _position.Y;
                }
            }
        }
        private RectangleFloat drawRectangle;
        public RectangleFloat DrawRectangle { get { return drawRectangle; } set { drawRectangle = value; } }
        public Texture2D Texture { get; set; }

        private int width;
        public int Width { get { return width; } set { width = value; drawRectangle.Width = value; } }
        private int height;
        public int Height { get { return height; } set { height = value; drawRectangle.Height = value; } }

        public Color DrawColor { get; set; }

        public Particle(Texture2D texture = null)
        {
            IsAlive = true;
            Lifetime = TimeSpan.FromMilliseconds(500);
            width = 3;
            height = 3;
            drawRectangle = new RectangleFloat(this.Position.X, this.Position.Y, width, height);
            DrawColor = Color.White;
            Texture = texture != null ? texture : AssetRepository.Instance.PlainWhiteTexture;
        }

        public void Begin() { }

        public void Update(GameTime gameTime)
        {
            if (Lifetime > TimeSpan.Zero)
            {
                Lifetime -= gameTime.ElapsedGameTime;
            }

            if (Lifetime <= TimeSpan.Zero)
            {
                Lifetime = TimeSpan.Zero;
                IsAlive = false;
            }

            Position += Velocity;
        }

        public void End() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    texture: Texture,
                    destinationRectangle: DrawRectangle.ToRectangle(),
                    sourceRectangle: null,
                    color: DrawColor,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effect: SpriteEffects.None,
                    depth: Position.Y / Camera.Instance.ViewBoundsRectangle.Bottom
                );
        }
    }

    public class LineParticle : Particle
    {
        public Vector2 start;
        public Vector2 end;
        public int width;

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawingUtility.DrawLineScaledWithImageJittered(start, end, width, this.Texture, spriteBatch, this.DrawColor);
        }
    }
}
