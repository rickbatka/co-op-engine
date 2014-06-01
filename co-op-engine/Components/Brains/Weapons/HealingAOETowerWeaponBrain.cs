using co_op_engine.Components.Particles;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace co_op_engine.Components.Brains.Weapons
{
    public class HealingAOETowerWeaponBrain : WeaponBrain
    {
        public HealingAOETowerWeaponBrain(GameObject owner)
            :base(owner)
        { }

        public override void PrimaryAttack(PrimaryAttackStartEventArgs e)
        {
            base.PrimaryAttack(e);

            e.Target.HandleHitByWeapon(Owner);
            FireUsedWeaponEvent(e.Target);
            
            ParticleEngine.Instance.Add(
                new LineParticle()
                {
                    DrawColor = Color.White,
                    Lifetime = TimeSpan.FromMilliseconds(200),
                    Texture = AssetRepository.Instance.FuzzyLazer,
                    width = 40,
                    end = Owner.Position,
                    start = e.Target.Position
                }
            );

        }
    }
}
