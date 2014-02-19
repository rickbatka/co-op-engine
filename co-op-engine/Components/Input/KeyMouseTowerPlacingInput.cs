using co_op_engine.Events;
using co_op_engine.ServiceProviders;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    public class KeyMouseTowerPlacingInput
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
            newX = MathHelper.Clamp(newX, 0, graphicsInfo.ScreenRectangle.Right - towerPlacingBox.Width);
            newY = MathHelper.Clamp(newY, 0, graphicsInfo.ScreenRectangle.Bottom - towerPlacingBox.Height);

            return new Vector2(newX, newY);
        }

        private float LockToGrid(float val)
        {
            float gridSize = graphicsInfo.GridSize;
            return ((int)(val / gridSize)) * gridSize;
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
