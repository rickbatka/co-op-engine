using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills.Boost
{
    public class SimpleBoostSkill : SkillSandbox
    {
        public override void Activate(GameObject owner)
        {
            base.AddSpeedBoost(owner, TimeSpan.FromMilliseconds(2000), 200f);
        }
    }
}
