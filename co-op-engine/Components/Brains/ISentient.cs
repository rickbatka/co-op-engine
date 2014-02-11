﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace co_op_engine.Components.Brains
{
    public interface ISentient
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}