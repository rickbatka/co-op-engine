using co_op_engine.Collections;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Weapons.Effects;
using co_op_engine.Components.Weapons.WeaponEngines;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Weapons
{
    public class Weapon : IRenderable
    {
        public GameObject owner;
        private RenderBase renderer;
        private WeaponEngine Engine;
        public SpacialBase CurrentQuad { get { return owner.CurrentQuad; } }

        public int ID;
        public int OwnerId;

        public int CurrentState { get; set; }
        public virtual WeaponState CurrentWeaponStateProperties { get { return WeaponStates.States[CurrentState]; } }
        public int CurrentOwnerState { get { return owner.CurrentState; } }

        public bool Visible { get; set; }
        public bool Friendly { get { return owner.Friendly; } }
        public Frame CurrentFrame { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get { return owner.Position; } }
        public int FacingDirection { get { return owner.FacingDirection; } set { owner.FacingDirection = value; } }
        public float RotationTowardFacingDirectionRadians { get { return owner.RotationTowardFacingDirectionRadians; } set { owner.RotationTowardFacingDirectionRadians = value; } }
        public float Scale { get { return owner.Scale; } }


        public List<WeaponEffectBase> Effects = new List<WeaponEffectBase>();

        public Weapon(GameObject owner)
        {
            this.owner = owner;
            this.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            this.OwnerId = owner.ID;
            this.Visible = true;
        }

        public void SetRenderer(RenderBase renderer)
        {
            this.renderer = renderer;
        }

        public void SetEngine(WeaponEngine engine)
        {
            this.Engine = engine;
        }

        public void Update(GameTime gameTime)
        {
            if (Engine != null)
            {
                Engine.Update(gameTime);
            }

            if(renderer != null)
            {
                renderer.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (renderer != null)
            {
                renderer.Draw(spriteBatch);
            }

            if(Engine != null)
            {
                Engine.Draw(spriteBatch);
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            if (renderer != null)
            {
                renderer.DebugDraw(spriteBatch);
            }

            if(Engine != null)
            {
                Engine.DebugDraw(spriteBatch);
            }
        }

        public bool TryInitiateAttack(int attackTimer = 0)
        {
            if (CurrentWeaponStateProperties.CanInitiatePrimaryAttack && Engine != null)
            {
                Engine.PrimaryAttack(attackTimer);
                return true;
            }
            return false;
        }

        public int GetAttackDuration()
        {
            if (renderer != null)
            {
                return renderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection);
            }

            return 0;
        }

        public void FireUsedWeaponEvent(GameObject receiver)
        {
            if(this.Friendly == receiver.Friendly)
            {
                owner.FireOnUsedWeaponEffectOnFriendly(this, null);
            }
            else
            {
                owner.FireOnDidAttackNonFriendly(this, null);
            }
        }

        public bool FullyRotatable { 
            get 
            {
                if (CurrentState == Constants.WEAPON_STATE_ATTACKING_PRIMARY)
                {
                    return true;
                }
                return false;
            } 
        }

        public void ResetAttackAnimation()
        {
            if (renderer != null)
            {
                renderer.animationSet.GetAnimationFallbackToDefault(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection).Reset();
            }
        }

        public void EquipEffect(WeaponEffectBase effect)
        {
            this.Effects.Add(effect);
        }
    }
}
