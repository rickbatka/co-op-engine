using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons.Effects
{
    public abstract class WeaponEffectBase : ICloneable
    {
        public int WeaponEffectID;
        protected GameObject Receiver;
        int DurationMS;
        public TimeSpan Timer;
        public bool IsFinished = false;
        protected Vector2 RotationAtTimeOfHit;
        public bool AffectsNonFriendlies = true;
        public bool AffectsFriendlies = false;

        protected WeaponEffectBase(int weaponEffectId, int durationMs)
        {
            this.WeaponEffectID = weaponEffectId;
            this.DurationMS = durationMs;
        }

        public void SetReceiver(GameObject receiver)
        {
            this.Receiver = receiver;
        }

        public void SetRotationAtTimeOfHit(Vector2 rotation)
        {
            this.RotationAtTimeOfHit = rotation;
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
