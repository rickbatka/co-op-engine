using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Boosts
{
    public class SimpleBoostSkill : BoostBase
    {
        public SimpleBoostSkill(SkillsComponent skillsComponent, GameObject owner)
            : base(skillsComponent, owner, 5000)
        {
        }

        public override void Activate(int attackTimer = 0)
        {
            ApplySpeedModifier(Owner, 2000, 1000);
            UseSkill();
        }
    }
}
