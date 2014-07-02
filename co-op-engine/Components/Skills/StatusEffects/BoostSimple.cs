using co_op_engine.Components.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.StatusEffects
{
    public class BoostSimpleStatusEffect : StatusEffectBase
    {
        private float BoostAmount;

        public BoostSimpleStatusEffect(GameObject applicant, float amount, int durationMilli)
            : base(applicant, durationMilli)
        {
            BoostAmount = amount;
        }

        public override void Update(GameTime gameTime)
        {
            //stupid smoke trail yay
            Particles.ParticleEngine.Instance.AddEmitter(new co_op_engine.Components.Particles.DustFastEmitter(ObjectReference));
            base.Update(gameTime);
        }

        /// <summary>
        /// for sparkles and stuff
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //nothing for this guy
        }

        protected override void OnApplication()
        {
            if (ObjectReference.Mover is WalkingMover) //Hack: good reason to put movement equations in base class and let derived classes use them
            {
                (ObjectReference.Mover as WalkingMover).CurrentMoveModifier += BoostAmount;
                WasApplied = true;
            }
            else
            {
                WasApplied = false;
            }
        }

        protected override void OnRemoval()
        {
            if (WasApplied && ObjectReference.Mover is WalkingMover) //Hack: good reason to put movement equations in base class and let derived classes use them
            {
                (ObjectReference.Mover as WalkingMover).CurrentMoveModifier -= BoostAmount;
            }
            base.OnRemoval();
        }
    }
}
