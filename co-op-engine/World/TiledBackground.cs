using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.World
{
    public class TiledBackground
    {
        BackgroundTile tile;
        Vector2 origin;
        int numColumns;
        int numRows;
        int fudgeFactor = 20;

        public TiledBackground(BackgroundTile tile)
        {
            this.tile = tile;
            this.origin = new Vector2(0, 0);
            this.numColumns = Camera.Instance.ViewportRectangle.Width / tile.width + 2;
            this.numRows = Camera.Instance.ViewportRectangle.Height / tile.height + 2;
        }

        public void Update(GameTime gameTime)
        {
            if(Camera.Instance.Position.X <= origin.X + fudgeFactor)
            {
                origin.X -= tile.width;
            }
            else if (Camera.Instance.Position.X + Camera.Instance.ViewportRectangle.Width > origin.X - fudgeFactor + (tile.width * numColumns))
            {
                origin.X += tile.width;
            }

            if (Camera.Instance.Position.Y <= origin.Y + fudgeFactor)
            {
                origin.Y -= tile.height;
            }
            else if (Camera.Instance.Position.Y + Camera.Instance.ViewportRectangle.Height > origin.Y - fudgeFactor + (tile.height * numRows))
            {
                origin.Y += tile.height;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < numColumns + 1; i++)
            {
                for (int j = 0; j < numRows + 1; j++)
                {
                    spriteBatch.Draw(
                        tile.texture, 
                        new Vector2(
                            origin.X + tile.width * i, 
                            origin.Y + tile.height * j
                        ), 
                        Color.White
                    );
                }
            }
        }
    }

    public class BackgroundTile
    {
        public Texture2D texture;
        public int width;
        public int height;

        public BackgroundTile(Texture2D texture, int width, int height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
        }
    }
}
