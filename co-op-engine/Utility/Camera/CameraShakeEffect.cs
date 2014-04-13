using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility.Camera
{
    class CameraShakeEffect : CameraEffectBase
    {
        private Camera owner;

        private const int shakeTotalEffectTime = 80;
        private const int shakePieceTimeMax = 20;
        private const int shakeForce = 2;
        private TimeSpan ShakeTimeCurrent;
        private Vector2 CurrentShakeStartPosition;
        private Vector2 ShakeVector;

        public CameraShakeEffect(Camera camera)
        {
            this.owner = camera;
            this.TotalEffectTime = shakeTotalEffectTime;
        }

        public override void Apply()
        {
            base.Apply();
            ShakeTimeCurrent = TimeSpan.Zero;
            CurrentShakeStartPosition = owner.Position;
            ShakeVector = GetNewShakeVector();
        }

        private Vector2 GetNewShakeVector()
        {
            return MechanicSingleton.Instance.RandomNormalizedVector() * shakeForce;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ShakeTimeCurrent += gameTime.ElapsedGameTime;

            if (ShakeTimeCurrent.TotalMilliseconds >= shakePieceTimeMax)
            {
                CurrentShakeStartPosition = owner.Position;
                ShakeVector = GetNewShakeVector();
                ShakeTimeCurrent = TimeSpan.Zero;
            }

            owner.Position = DrawingUtility.EaseInOutLinear(CurrentShakeStartPosition, ShakeVector, (float)shakePieceTimeMax, (float)ShakeTimeCurrent.TotalMilliseconds);
        }
    }
}
