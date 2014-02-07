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
        //not sure if you wanted to go the component route or not but I'm gonna start separating the object like that here
        ActionBase actionComponent;
        PhysicsBase physicsComponent;
        RenderBase renderComponent;

        public event EventHandler OnDeath;

        public GameObject(Texture2D tex)
        {
            //should be removed, just here for demo purposes
            this.plainWhiteTexture = tex;
            NewTimer();
        }

        public void Update(GameTime gameTime)
        {
            actionComponent.Update(gameTime);
            physicsComponent.Update(gameTime);
            renderComponent.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            renderComponent.Draw(spriteBatch);
            actionComponent.Draw(spriteBatch);
            physicsComponent.Draw(spriteBatch);

            //should be moved into render object during implementation
            spriteBatch.Draw(plainWhiteTexture, new Rectangle(10, 10, 10, 10), defaultDrawingColor);
        }

        //#######################################################################
        // below should be refactored and removed during actual implementation
        //#######################################################################

        Color defaultDrawingColor;
        Texture2D plainWhiteTexture;

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


        private void NewTimer()
        {
            GameTimerManager.Instance.SetTimer(1000, t => { ChangeColor(); NewTimer(); }, this);
        }
    }
}
