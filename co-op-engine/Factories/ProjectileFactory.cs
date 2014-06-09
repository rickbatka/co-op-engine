using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Brains.Projectiles;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.Weapons;
using co_op_engine.Effects;
using co_op_engine.GameStates;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Factories
{
    public class ProjectileFactory
    {
        public static ProjectileFactory Instance;
        private GamePlay gameRef;

        private ProjectileFactory(GamePlay gameRef)
        {
            this.gameRef = gameRef;
        }

        public static void Initialize(GamePlay gameRef)
        {
            Instance = new ProjectileFactory(gameRef);
        }

        public GameObject GetArrow(GameObject shooter, GameObject target, int id = -1)
        {
            var arrowContainerObject = new GameObject(gameRef.Level);
            arrowContainerObject.ConstructionStamp = "Arrow";
            arrowContainerObject.Friendly = true;
            arrowContainerObject.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            arrowContainerObject.CurrentFrame = AssetRepository.Instance.ArrowAnimations(arrowContainerObject.Scale).CurrentAnimatedRectangle.CurrentFrame;

            arrowContainerObject.UsedInPathing = false;
            arrowContainerObject.SetPhysics(new NonCollidingPhysics(arrowContainerObject));
            //arrow.SetRenderer(new RenderBase(arrow, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations));
            arrowContainerObject.SetBrain(new ArrowBrain(arrowContainerObject, target));
            arrowContainerObject.SetMover(new ProjectileMover(arrowContainerObject));
            arrowContainerObject.SetSkills(new SkillsComponent(arrowContainerObject));

            var arrowWeapon = new AlwaysAttackingWeapon(arrowContainerObject.Skills, arrowContainerObject);
            arrowWeapon.EquipEffect(new BasicDamageEffect(durationMS: 250, damageRating: 25));
            arrowWeapon.SetRenderer((new RenderBase(arrowWeapon, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations(arrowContainerObject.Scale))));
            arrowContainerObject.EquipWeapon(arrowWeapon);
    
            arrowContainerObject.CurrentState = Constants.ACTOR_STATE_IDLE;
            arrowContainerObject.Position = shooter.Position;
            
            gameRef.container.AddObject(arrowContainerObject);
            
            return arrowContainerObject;
        }

    }
}
