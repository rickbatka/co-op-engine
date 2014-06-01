using co_op_engine.Utility;
using Microsoft.Xna.Framework;

namespace co_op_engine.Components.Brains.Weapons
{
    public class WeaponBrain : BrainBase
    {
        public WeaponBrain(GameObject owner)
            :base(owner)
        {
            owner.Parent.OnAttemptedToInitiatePrimaryAttack += TryInitiateAttack;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            QueryForHits();
        }

        public virtual void TryInitiateAttack(object sender, PrimaryAttackStartEventArgs e)
        {
            if (Owner.CurrentStateProperties.CanInitiatePrimaryAttackState)
            {
                PrimaryAttack(e);
            }
        }

        public virtual void PrimaryAttack(PrimaryAttackStartEventArgs e)
        {
            var attackTimer = 0;
            if (e == null || e.AttackDuration == 0)
            {
                attackTimer = Owner.Renderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, Owner.FacingDirection);
            }
            else
            {
                attackTimer = e.AttackDuration;
            }
            
            Owner.CurrentState = Constants.WEAPON_STATE_ATTACKING_PRIMARY;
            Owner.FireOnPrimaryAttackStarted(this, new PrimaryAttackStartEventArgs { AttackDuration = attackTimer });
        }

        private void QueryForHits()
        {
            if (Owner.CurrentStateProperties.IsAttacking && Owner.CurrentFrame.DamageDots != null)
            {
                var damageDots = Owner.CurrentFrame.DamageDots;
                foreach (var damageDot in damageDots)
                {
                    var damageDotPositionVector = DrawingUtility.GetAbsolutePosition(Owner, damageDot.Location);
                    var colliders = Owner.CurrentQuad.MasterQuery(DrawingUtility.VectorToPointRect(damageDotPositionVector));
                    foreach (var collider in colliders)
                    {
                        if (collider.ID != Owner.ID && collider.ID != Owner.Parent.ID && !Owner.Parent.HasObjectAsChild(collider))
                        {
                            collider.HandleHitByWeapon(Owner);
                            FireUsedWeaponEvent(collider);
                        }
                    }
                }
            }
        }

        protected void FireUsedWeaponEvent(GameObject receiver)
        {
            if (Owner.Parent.Friendly == receiver.Friendly)
            {
                Owner.Parent.FireOnUsedWeaponEffectOnFriendly(this, null);
            }
            else
            {
                Owner.Parent.FireOnDidAttackNonFriendly(this, null);
            }
        }

    }
}
