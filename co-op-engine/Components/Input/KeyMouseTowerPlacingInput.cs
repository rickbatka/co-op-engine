﻿using co_op_engine.GameStates;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    public delegate void CoordsUpdatedEventHandler(KeyMouseTowerPlacingInput sender, CoordEventArgs coordData);
    public struct CoordEventArgs
    {
        public Vector2 Coords;
    }

    public class KeyMouseTowerPlacingInput
    {
        GamePlay gameRef;
        Rectangle towerPlacingBox;
        public event EventHandler OnPlacementAttempted;
        public event CoordsUpdatedEventHandler OnCoordsUpdated;
        
        public KeyMouseTowerPlacingInput(GamePlay gameRef, Rectangle towerPlacingBox) 
        {
            this.gameRef = gameRef;
            this.towerPlacingBox = towerPlacingBox;
        }

        public void Update(GameTime gameTime)
        {
            GetCoords(); 
            ListenForClicks();
        }

        public void GetCoords()
        {
            var pos = InputHandler.MousePositionVectorCameraAdjusted();

            var lockedPos = LockToScreenGrid(pos);
            
            if (OnCoordsUpdated != null)
            {
                OnCoordsUpdated(this, new CoordEventArgs() { Coords = lockedPos });
            }
        }

        private Vector2 LockToScreenGrid(Vector2 pos)
        {
            float newX = pos.X;
            float newY = pos.Y;

            //lock to grid
            newX = LockToGrid(newX);
            newY = LockToGrid(newY);

            // lock to screen
            newX = MathHelper.Clamp(newX, 0, gameRef.ScreenRectangle.Right - towerPlacingBox.Width);
            newY = MathHelper.Clamp(newY, 0, gameRef.ScreenRectangle.Bottom - towerPlacingBox.Height);

            return new Vector2(newX, newY);
        }

        private float LockToGrid(float val)
        {
            float gridSize = gameRef.GridSize;
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
