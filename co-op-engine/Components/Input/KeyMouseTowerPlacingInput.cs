using co_op_engine.ServiceProviders;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    public class CoordEventArgs : EventArgs
    {
        public Vector2 Coords;
    }

    public class KeyMouseTowerPlacingInput : ITowerPlacingInput
    {
        IGraphicsInformationProvider graphicsInfo;
        Rectangle towerPlacingBox;
        public event EventHandler OnPlacementAttempted;
        public event EventHandler OnCoordsUpdated;
        
        public KeyMouseTowerPlacingInput(Rectangle towerPlacingBox) 
        {
            graphicsInfo = (IGraphicsInformationProvider)GameServicesProvider.GetService(typeof(IGraphicsInformationProvider));
            this.towerPlacingBox = towerPlacingBox;
        }

        public void Update(GameTime gameTime)
        {
            GetCoords(); 
            ListenForClicks();
        }

        public void GetCoords()
        {
            var pos = InputHandler.MousePositionVector();
            var lockedPos = LockToScreenGrid(pos);

            OnCoordsUpdated(this, new CoordEventArgs() {Coords = lockedPos });
        }

        private Vector2 LockToScreenGrid(Vector2 pos)
        {
            float newX = pos.X;
            float newY = pos.Y;

            //lock to grid
            newX = LockToGrid(newX);
            newY = LockToGrid(newY);

            // lock to screen
            if (pos.X < graphicsInfo.ScreenRectangle.Left) newX = 0;
            if (pos.X + towerPlacingBox.Width >= graphicsInfo.ScreenRectangle.Right) newX = graphicsInfo.ScreenRectangle.Right - towerPlacingBox.Width ;
            if (pos.Y < graphicsInfo.ScreenRectangle.Top) newY = 0;
            if (pos.Y + towerPlacingBox.Height >= graphicsInfo.ScreenRectangle.Bottom) newY = graphicsInfo.ScreenRectangle.Bottom - towerPlacingBox.Height;

            return new Vector2(newX, newY);
        }

        private float LockToGrid(float val)
        {
            float gridSize = graphicsInfo.GridSize;
            float mod = val % gridSize;

            if (mod < (gridSize / 2))
            {
                val -= mod;
            }
            else if (mod >= (gridSize / 2))
            {
                val += gridSize - mod;
            }

            return val;
        }

        private void ListenForClicks()
        {
            if (InputHandler.MouseLeftPressed())
            {
                OnPlacementAttempted(this, null);
            }
        }

    }
}
