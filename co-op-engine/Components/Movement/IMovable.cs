using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Movement
{
    public interface IMovable
    {
        Vector2 Velocity { get; }
        Vector2 Acceleration { get; }
        Vector2 InputMovementVector { set; }
        Vector2 Position { get; set; }
        bool IsBoosting { set; }
        int Width { get; }
        int Height { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
