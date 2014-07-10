using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Weapons
{
    public abstract class AlwaysAttackingWeaponBase : WeaponBase
    {
        public AlwaysAttackingWeaponBase(SkillsComponent skillsComponent, GameObject owner)
        :base(skillsComponent, owner)
        { }

        protected override void UpdateState(GameTime gameTime)
        {
        }

        public override void Activate(int attackTimer = 0)
        {
            this.UseSkill(1);
        }
    }
}
