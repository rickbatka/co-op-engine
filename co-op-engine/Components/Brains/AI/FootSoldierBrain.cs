using co_op_engine.Pathing;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Brains.AI
{
    public class FootSoldierBrain : BrainBase
    {
        private GameObject Target;
        private int HotPursuitDistance = 50;
        private int AttackDistance = 50;
        private enum PursuitTypes { Pathing, CrowFlies }
        private PursuitTypes PursuitType = PursuitTypes.Pathing;

        public FootSoldierBrain(GameObject owner, GameObject target)
            : base(owner)
        {
            SetTarget(target);
        }

        private void SetTarget(GameObject target)
        {
            Target = target;
            if (Pather != null)
            {
                Pather.SetTarget(target, true);
            }
        }

        public override void BeforeUpdate() { }

        public override void Update(GameTime gameTime)
        {
            DeterminePursuitType();
            Pursue();
            Attack();
            base.Update(gameTime);
        }

        private void DeterminePursuitType()
        {
            if (Target != null && Target.Position != null)
            {
                if ((Target.Position - Owner.Position).Length() < HotPursuitDistance) // in range for follow
                {
                    if (Pather != null)
                    {
                        Pather.ReleasePath();
                    }
                    PursuitType = PursuitTypes.CrowFlies;
                }
                else //out of range, need path again
                {
                    if (!Pather.HasPath())
                    {
                        Pather.RequestPath();
                    }
                }
            }
        }

        private void Pursue()
        {
            if (Target != null && Target.Position != null)
            {
                if(PursuitType == PursuitTypes.CrowFlies 
                    || !Pather.HasPath()) // we might have switched bak to path but not have a path yet, don't want to stutter
                {
                    Owner.RotationTowardFacingDirectionRadians = DrawingUtility.Vector2ToRadian(Target.Position - Owner.Position);
                    Owner.InputMovementVector = Target.Position - Owner.Position;

                    if ((Target.Position - Owner.Position).Length() >= HotPursuitDistance)
                    {
                        Pather.RequestPath();
                        PursuitType = PursuitTypes.Pathing;
                    }
                }
            }
        }

        private void Attack()
        {
            if((Target.Position - Owner.Position).Length() <= AttackDistance
                && Owner.CurrentStateProperties.CanInitiatePrimaryAttackState
                && Owner.Weapon != null)
            {
                Owner.Weapon.PrimaryAttack();
            }
        }

        public override void AfterUpdate() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
