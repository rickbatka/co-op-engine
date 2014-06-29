using co_op_engine.Components.Movement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.StatusEffects
{
    public class BoostSimpleStatusEffect 
    {
        private GameObject ObjectReference;
        private bool WasApplied;
        private float BoostAmount;
        private TimeSpan Duration;

        public bool Done { get; private set; }

        //needs to be able to apply itself to an object
        //needs to be able to remove itself from an object

        public BoostSimpleStatusEffect(GameObject applicant, float amount, TimeSpan duration)
        {
            ObjectReference = applicant;
            BoostAmount = amount;
            WasApplied = false;
            Duration = duration;
            
            OnApplication();
        }

        public void Update(GameTime gameTime)
        {
            Duration -= gameTime.ElapsedGameTime;
            if (Duration <= TimeSpan.Zero)
            {
                OnRemoval();
            }
        }

        public void ModifyDuration(int milliModification)
        {
            Duration += TimeSpan.FromMilliseconds(milliModification);
        }

        protected void OnApplication()
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

        protected void OnRemoval()
        {
            if (WasApplied && ObjectReference.Mover is WalkingMover) //Hack: good reason to put movement equations in base class and let derived classes use them
            {
                (ObjectReference.Mover as WalkingMover).CurrentMoveModifier -= BoostAmount;
            }
            Done = true;
        }
    }
}
