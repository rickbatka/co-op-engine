using co_op_engine.GameStates;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    public class TowerPlacingInput
    {
        GamePlay gameRef;
        Rectangle towerPlacingBox;
        
        public TowerPlacingInput(GamePlay gameRef, Rectangle towerPlacingBox) 
        {
            this.gameRef = gameRef;
            this.towerPlacingBox = towerPlacingBox;
        }

        public bool DidPressBuildButton()
        {
            return InputHandler.MouseLeftPressed();
        }

        public Vector2 GetCoords()
        {
            var pos = InputHandler.MousePositionVectorCameraAdjusted();

            var lockedPos = LockToScreenGrid(pos);

            return lockedPos;
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

    }
}
