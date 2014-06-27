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
    /// </summary>
    public class SkillsComponent
    {
        private GameObject Owner;

        public Weapon WeaponSkill;
        public Boost BoostSkill;
        public Rage RageSkill;
        public Spell SpellSkill;
        private List<Skill> AllSkills;

        public SkillsComponent(GameObject owner)
        {
            Owner = owner;
            AllSkills = new List<Skill>();
        }

        public void SetWeapon(Weapon weaponSkill)
        {
            WeaponSkill = weaponSkill;
            AllSkills.Add(WeaponSkill);
        }

        public void SetRage(Rage rageSkill)
        {
            RageSkill = rageSkill;
            AllSkills.Add(RageSkill);
        }

        public void Draw(SpriteBatch spriteBatch) 
        { 
            foreach(var skill in AllSkills)
            {
                if(skill != null)
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
            if (WeaponSkill == null) { return false; }

            return WeaponSkill.TryInitiateSkill(attackTimer);
        }

        public bool TryInitiateRage(int attackTimer = 0)
        {
            if (RageSkill == null) { return false; }

            return RageSkill.TryInitiateSkill(attackTimer);
        }
    }
}
