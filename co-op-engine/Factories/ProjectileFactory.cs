using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Brains.Projectiles;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.Weapons;
using co_op_engine.GameStates;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// builds a generic projectile obejct based on the animation and texture provided
        /// </summary>
        public GameObject GetGenericProjectileNoWeapon(GameObject source, Vector2 start, float scale, Texture2D texture, AnimationSet animationSet, int lifeMilli)
        {
            //new it up
            var projectileContainer = new GameObject();
            
            //stamp with network unambiguous construction name
            projectileContainer.ConstructionStamp = "Projectile";//TODO this is ambiguous to network construction, maybe enum and pass in
            
            //set team
            projectileContainer.Team = source.Team;
            
            //get id
            projectileContainer.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            
            //brain, combat, engine, mover, physics, renderer, skills
            //oneHit projectile brain
            projectileContainer.SetBrain(new OneHitStraightProjectileBrain(projectileContainer, lifeMilli));
            //no hit physics
            projectileContainer.SetPhysics(new NonCollidingPhysics(projectileContainer, gameRef.CurrentLevel.Bounds));
            //projectile weapon
            projectileContainer.SetSkills(new SkillsComponent(projectileContainer));
            //fixed velocity mover
            projectileContainer.SetMover(new SimpleNoStateMover(projectileContainer));
            //no renderer, engine, or combat
            
            //set initial state: location, actor state, direction, velocity
            projectileContainer.Position = start;
            projectileContainer.CurrentState = Constants.ACTOR_STATE_IDLE;

            //add to world
            gameRef.CurrentLevel.Container.AddObject(projectileContainer);

            //return is
            return projectileContainer;
        }

        //public GameObject GetArrow(GameObject shooter, GameObject target, int id = -1)
        //{
        //    var arrowContainerObject = new GameObject();
        //    arrowContainerObject.ConstructionStamp = "Arrow";
        //    arrowContainerObject.Team = shooter.Team;
        //    arrowContainerObject.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
        //    arrowContainerObject.CurrentFrame = AssetRepository.Instance.ArrowAnimations(arrowContainerObject.Scale).CurrentAnimatedRectangle.CurrentFrame;

        //    arrowContainerObject.UsedInPathing = false;
        //    arrowContainerObject.SetPhysics(new NonCollidingPhysics(arrowContainerObject, gameRef.CurrentLevel.Bounds));
        //    //arrow.SetRenderer(new RenderBase(arrow, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations));
        //    arrowContainerObject.SetBrain(new GenericProjectileBrain(arrowContainerObject));
        //    arrowContainerObject.SetMover(new ProjectileMover(arrowContainerObject, ));
        //    arrowContainerObject.SetSkills(new SkillsComponent(arrowContainerObject));

        //    var arrowWeapon = new AlwaysAttackingWeapon(arrowContainerObject.Skills, arrowContainerObject);
            
        //    //DOTHIS figure out arrow sitch
        //    //arrowWeapon.EquipEffect(new BasicDamageEffect(durationMS: 250, damageRating: 25));
        //    arrowWeapon.SetRenderer((new RenderBase(arrowWeapon, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations(arrowContainerObject.Scale))));
        //    arrowContainerObject.EquipWeapon(arrowWeapon);
    
        //    arrowContainerObject.CurrentState = Constants.ACTOR_STATE_IDLE;
        //    arrowContainerObject.Position = shooter.Position;
            
        //    gameRef.CurrentLevel.Container.AddObject(arrowContainerObject);
            
        //    return arrowContainerObject;
        //}

    }
}
