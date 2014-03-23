using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public abstract class WeaponEffectBase
    {
        int WeaponID;
        protected GameObject Receiver;
        int DurationMS;
        public TimeSpan Timer;
        public bool IsFinished = false;

        protected WeaponEffectBase(GameObject receiver, int weaponId, int durationMs)
        {
            this.Receiver = receiver;
            this.WeaponID = weaponId;
            this.DurationMS = durationMs;
        }

        public virtual void Apply()
        {
            this.Timer = new TimeSpan(0, 0, 0, 0, DurationMS);
        }
        public virtual void Clear()
        { }

        public virtual void Update(GameTime gameTime)
        {
            Timer -= gameTime.ElapsedGameTime;
            if (Timer <= TimeSpan.Zero)
            {
                IsFinished = true;
            }
        }

        public virtual void Draw() { }
    }
}
