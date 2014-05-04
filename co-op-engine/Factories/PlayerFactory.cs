﻿using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Input;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.World.Level;
using co_op_engine.GameStates;
using co_op_engine.Components.Weapons;
using Microsoft.Xna.Framework;
using co_op_engine.Utility;
using co_op_engine.Components.Combat;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.Components.Weapons.Effects;
using co_op_engine.Components.Brains.AI;

namespace co_op_engine.Factories
{
    class PlayerFactory
    {
        public static PlayerFactory Instance;
        private GamePlay gameRef;

#warning TEST VARS FOR PATHING REMOVE WHEN POLISHED
        public GameObject playerRef_testing_pathing { get; private set; }

        private PlayerFactory(GamePlay gameRef)
        {
            this.gameRef = gameRef;
        }

        public static void Initialize(GamePlay gameRef)
        {
            Instance = new PlayerFactory(gameRef);
        }

        public GameObject GetPlayer()
        {
            var player = new GameObject();
            player.ConstructionStamp = "Player";
            player.Friendly = true;

            player.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            player.Position = new Vector2(MechanicSingleton.Instance.rand.Next(1, 100));

            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new RenderBase(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            player.SetRenderer(renderer);
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));
            player.SetCombat(new CombatBase(player));

            // wire up the events between components
            player.EquipWeapon(GetSword(player));

            gameRef.container.AddObject(player);

            var parms = new CreateParameters()
            {
                ConstructorId = player.ConstructionStamp,
                ID = player.ID,
                Position = player.Position
            };

            playerRef_testing_pathing = player;
            NetCommander.CreatedObject(player);

            return player;
        }

        public GameObject GetEnemy(int id = -1)
        {
            var enemy = new GameObject();
            enemy.ConstructionStamp = "Enemy";

            enemy.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            enemy.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500));

            enemy.SetPhysics(new CollidingPhysics(enemy));
            var renderer = new RenderBase(enemy, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            enemy.SetRenderer(renderer);

            if (id == -1)
            {
                enemy.SetBrain(new PathingTestBrain(enemy));
            }
            else
            {
                enemy.SetBrain(new NetworkPlayerBrain(enemy));
            }

            enemy.SetCombat(new CombatBase(enemy));

            // wire up the events between components
            enemy.EquipWeapon(GetAxe(enemy));

            gameRef.container.AddObject(enemy);

            if (id == -1)
            {
                NetCommander.CreatedObject(enemy);
            }

            return enemy;
        }

        public GameObject GetNetworkPlayer(int id = -1)
        {
            var player = new GameObject();
            player.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            player.Friendly = true;
            player.Position = new Vector2(100, 100);

            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new RenderBase(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            player.SetRenderer(renderer);
            player.SetBrain(new NetworkPlayerBrain(player));
            player.SetCombat(new COMBATPLACEHOLDER(player));
            player.EquipWeapon(GetSword(player));

            gameRef.container.AddObject(player);

            return player;
        }

        public Weapon GetSword(GameObject owner)
        {
            var sword = new Weapon(owner);

            var swordRenderer = new RenderBase(sword, AssetRepository.Instance.SwordTexture, AssetRepository.Instance.SwordAnimations);
            sword.SetRenderer(swordRenderer);

            sword.EquipEffect(new BasicDamageEffect(
                durationMS: swordRenderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection),
                damageRating: 25
            ));

            return sword;
        }

        public Weapon GetAxe(GameObject owner)
        {
            var axe = new Weapon(owner);
            var axeRenderer = new RenderBase(axe, AssetRepository.Instance.AxeTexture, AssetRepository.Instance.AxeAnimations);
            axe.SetRenderer(axeRenderer);
            return axe;
        }

        public Weapon GetMace(GameObject owner)
        {
            var mace = new Weapon(owner);
            var maceRenderer = new RenderBase(mace, AssetRepository.Instance.MaceTexture, AssetRepository.Instance.MaceAnimations);
            mace.SetRenderer(maceRenderer);
            return mace;
        }
    }
}
