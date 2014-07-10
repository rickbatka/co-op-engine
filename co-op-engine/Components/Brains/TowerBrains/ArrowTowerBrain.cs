﻿using co_op_engine.Components.Input;
using co_op_engine.Factories;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.TowerBrains
{
    public class ArrowTowerBrain : BasicTowerBrain
    {
        public GameObject Target { get; private set; }
        Color TextureColor = new Color(Color.White, 0.001f);
        
        public ArrowTowerBrain(GameObject owner, TowerPlacingInput placingInput)
            : base(owner, placingInput)
        {
            
        }

        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void HandleNonFriendlyInRange(GameObject collider)
        {
            base.HandleNonFriendlyInRange(collider);
            Target = collider;
            Owner.Skills.TryInitiateTowerSkill();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
