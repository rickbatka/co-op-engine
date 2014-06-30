using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.StatusEffects
{
    public abstract class StatusEffectBase
    {
        public GameObject ObjectReference { get; protected set; } //public for if there was a skill that wanted to increase or decrease based on applicants
        public TimeSpan Duration { get; protected set; }

        protected bool WasApplied;

        public bool Done { get; protected set; } //flag for being removed from active statuses

        public StatusEffectBase(GameObject applicant, TimeSpan duration)
        {
            Duration = duration;
            WasApplied = false;
            ObjectReference = applicant;
        }

        public void Start()
        {
            OnApplication();
        }

        /// <summary>
        /// if overridden ensure this method is called so the timer will count down
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            Duration -= gameTime.ElapsedGameTime;
            if (Duration <= TimeSpan.Zero)
            {
                OnRemoval();
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        protected abstract void OnApplication();
        protected virtual void OnRemoval()
        {
            Done = true;
        }
    }
}
