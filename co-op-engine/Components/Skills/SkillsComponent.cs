using co_op_engine.Components.Skills.Boosts;
using co_op_engine.Components.Skills.Rages;
using co_op_engine.Components.Skills.Weapons;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Skills
{
    /// <summary>
    /// responsibilities
    /// place to store all skills data
    /// holds methods to fire off skills
    /// 
    /// used only by players and possibly enemies, good candidate for moving off gameobject
    /// </summary>
    public class SkillsComponent
    {
        private GameObject Owner;

        public int RageMeter = 0;


        public WeaponBase WeaponSkill;
        public BoostBase BoostSkill;
        public RageBase RageSkill;
        //public Spell SpellSkill;
        private List<SkillBase> AllSkills;

        public SkillsComponent(GameObject owner)
        {
            Owner = owner;
            AllSkills = new List<SkillBase>();
        }

        public void SetWeapon(WeaponBase weaponSkill)
        {
            WeaponSkill = weaponSkill;
            AllSkills.Add(WeaponSkill);
        }

        public void SetRage(RageBase rageSkill)
        {
            RageSkill = rageSkill;
            AllSkills.Add(RageSkill);
        }

        public void SetBoost(BoostBase boostSkill)
        {
            BoostSkill = boostSkill;
            AllSkills.Add(BoostSkill);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var skill in AllSkills)
            {
                if (skill != null)
                {
                    skill.Draw(spriteBatch);
                }
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            foreach (var skill in AllSkills)
            {
                if (skill != null)
                {
                    skill.DebugDraw(spriteBatch);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var skill in AllSkills)
            {
                if (skill != null)
                {
                    skill.Update(gameTime);
                }
            }
        }

        public bool TryInititateWeaponAttack(int attackTimer = 0)
        {
            if (WeaponSkill != null && ActorStates.States[Owner.CurrentState].CanInitiateSkills)
            {
                WeaponSkill.Activate(attackTimer);
                return true;
            }

            return false;
        }

        public void TryInitiateBoost(int attackTimer = 0)
        {
            if (BoostSkill != null 
                && ActorStates.States[Owner.CurrentState].CanInitiateSkills
                && BoostSkill.IsReady)
            {
                BoostSkill.Activate();
            }
        }

        public void TryInitiateRage(int attackTimer = 0)
        {
            if (RageSkill != null
                && ActorStates.States[Owner.CurrentState].CanInitiateSkills
                && RageMeter >= RageSkill.RageCost)
            {
                RageSkill.Activate();
            }
        }
    }
}
