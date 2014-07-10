using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Weapons
{
    class ArrowWeapon : AlwaysAttackingWeaponBase
    {
        public ArrowWeapon(SkillsComponent skillsComponent, GameObject owner) 
            : base(skillsComponent, owner) { }

        protected override void WeaponHitSomething(GameObject thingHit)
        {
            DamageHealth(Owner, thingHit, 20);
        }
    }
}
