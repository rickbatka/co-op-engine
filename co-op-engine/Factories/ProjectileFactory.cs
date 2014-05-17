﻿using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Brains.Projectiles;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Weapons;
using co_op_engine.Components.Weapons.Effects;
using co_op_engine.GameStates;
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
            arrowContainerObject.CurrentFrame = AssetRepository.Instance.ArrowAnimations.CurrentAnimatedRectangle.CurrentFrame;

            arrowContainerObject.UsedInPathing = false;
            arrowContainerObject.SetPhysics(new NonCollidingPhysics(arrowContainerObject));
            //arrow.SetRenderer(new RenderBase(arrow, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations));
            arrowContainerObject.SetBrain(new ArrowBrain(arrowContainerObject, target));

            var arrowWeapon = new AlwaysAttackingProjectileWeapon(arrowContainerObject);
            arrowWeapon.EquipEffect(new BasicDamageEffect(durationMS: 250, damageRating: 25));
            arrowWeapon.SetRenderer((new RenderBase(arrowWeapon, AssetRepository.Instance.ArrowTexture, AssetRepository.Instance.ArrowAnimations)));
            arrowWeapon.Friendly = true;
            arrowContainerObject.EquipWeapon(arrowWeapon);
    
            arrowContainerObject.CurrentState = Constants.ACTOR_STATE_IDLE;
            arrowContainerObject.Position = shooter.Position;
            
            gameRef.container.AddObject(arrowContainerObject);
            
            return arrowContainerObject;
        }
    }
}