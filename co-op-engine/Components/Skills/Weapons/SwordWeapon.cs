using co_op_engine.Sound;
using co_op_engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Weapons
{
    public class SwordWeapon : WeaponBase
    {
        public SwordWeapon(SkillsComponent skillsComponent, GameObject owner)
            :base(skillsComponent, owner)
        { }

        protected override void WeaponHitSomething(GameObject thingHit)
        {
            Knockback(thingHit, 10000);
            DamageHealth(Owner, thingHit, 10);
            AddDamageOverTime(thingHit, 8000, 500, 2f);
        }

        public override void Activate(int attackTimer = 0)
        {
            SoundManager.PlaySoundEffect(AssetRepository.Instance.SwordSwoosh1);
            UseSkill();
        }
    }
}
