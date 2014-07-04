using co_op_engine.Components.Input;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Components.Particles;
using co_op_engine.Components.Particles.Decorators;
using co_op_engine.Components.Particles.Emitters;

namespace co_op_engine.Components.Brains.TowerBrains
{
    public class HealingAOETowerBrain : BasicTowerBrain
    {
        Color TextureColor = new Color(Color.White, 0.001f);
        int healCooldown = 2000;

        public HealingAOETowerBrain(GameObject owner, TowerPlacingInput placingInput)
            : base(owner, placingInput)
        {
            
        }
        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void HandleFriendlyInRange(GameObject collider)
        {
            base.HandleFriendlyInRange(collider);


            //DOTHIS this was a hack, figure out a better way
            if (Owner.Skills.TryInititateWeaponAttack(healCooldown))
            {
                //collider.HandleHitBySkill(Owner.Skills.WeaponSkill);
                ParticleEngine.Instance.AddEmitter(new HealBeam(Owner, collider));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
