﻿using co_op_engine.Collections;
using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.Components.Input;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.ServiceProviders;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components
{
    public class TowerFactory
    {
        public static GameObject GetDoNothingTower(Game1 theGame, ElasticQuadTree tree, Texture2D texture, Texture2D placingZoneTexture)
        {
            var tower = new GameObject();
            tower.SetMover(new MoverBase(tower));
            tower.SetPhysics(new NonCollidingPhysics(tower, tree));
            tower.SetRenderer(new BasicRenderer(tower, texture));
            tower.SetBrain(new BasicTowerBrain(tower, placingZoneTexture, new KeyMouseTowerPlacingInput(tower.BoundingBox)));

            return tower;
        }
    }
}
