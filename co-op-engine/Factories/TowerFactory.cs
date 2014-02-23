using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.Components.Input;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.ServiceProviders;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.World.Level;

namespace co_op_engine.Factories
{
    class TowerFactory
    {
        public static GameObject GetDoNothingTower(Game1 gameRef, Texture2D texture, Texture2D placingZoneTexture)
        {
            var tower = new GameObject();
            tower.UnShovable = true;
            tower.SetPhysics(new NonCollidingPhysics(tower));
            tower.SetRenderer(new RenderBase(tower, texture));
            tower.SetBrain(new BasicTowerBrain(tower, placingZoneTexture, new KeyMouseTowerPlacingInput(tower.BoundingBox)));

            gameRef.container.AddObject(tower);

            return tower;
        }
    }
}
