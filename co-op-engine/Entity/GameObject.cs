using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Entity
{
    class GameObject
    {
        Color defaultDrawingColor;
        Texture2D plainWhiteTexture;


        public event EventHandler OnDeath;

        public GameObject(Texture2D tex)
        {
            this.plainWhiteTexture = tex;
            NewTimer();
        }

        private void NewTimer()
        {
            GameTimerManager.Instance.SetTimer(1000, t => { ChangeColor(); NewTimer(); }, this);
        }

        public void Update(GameTime gameTime)
        {
        }

        private void ChangeColor()
        {
            if (defaultDrawingColor == Color.White)
            {
                defaultDrawingColor = Color.Black;
            }
            else
            {
                defaultDrawingColor = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(plainWhiteTexture, new Rectangle(10, 10, 10, 10), defaultDrawingColor);
        }
    }
}
