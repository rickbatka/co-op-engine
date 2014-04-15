﻿using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public interface IParticle
    {
        Rectangle DrawRectangle { get; }
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
                    drawRectangle.X = (int)_position.X;
                    drawRectangle.Y = (int)_position.Y;
                }
            }
        }
        private Rectangle drawRectangle;
        public Rectangle DrawRectangle { get { return drawRectangle; } }
        public Texture2D Texture { get; set; }

        private int width;
        public int Width { get { return width; } set { width = value; drawRectangle.Width = value; } }
        private int height;
        public int Height { get { return height; } set { height= value; drawRectangle.Height = value; } }

        public Color DrawColor { get; set; }

        public Particle() 
        {
            IsAlive = true;
            Lifetime = TimeSpan.FromMilliseconds(500);
            width = 3;
            height = 3;
            drawRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, width, height);
            Texture = AssetRepository.Instance.PlainWhiteTexture;
            DrawColor = Color.White;
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
    }
}