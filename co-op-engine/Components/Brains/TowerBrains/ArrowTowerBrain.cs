using co_op_engine.Components.Input;
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
        Color TextureColor = new Color(Color.White, 0.001f);
        int shotCooldown = 2000;

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

            if (Owner.Weapon.TryInitiateAttack(shotCooldown))
            {
                var arrow = ProjectileFactory.Instance.GetArrow(Owner, collider);
                arrow.FireOnWasFiredAtFixedPoint(this, new FireProjectileEventArgs { TargetObject = collider });
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
