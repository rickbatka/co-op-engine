using co_op_engine.Collections;
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
using Microsoft.Xna.Framework;
using co_op_engine.Utility;
using co_op_engine.Components.Combat;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.Components.Brains.AI;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Engines;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.Boosts;
using co_op_engine.Components.Skills.Rages;
using co_op_engine.Components.Skills.Weapons;

namespace co_op_engine.Factories
{
    class PlayerFactory
    {
        public static PlayerFactory Instance;
        private GamePlay gameRef;

        private PlayerFactory(GamePlay gameRef)
        {
            this.gameRef = gameRef;
        }

        public static void Initialize(GamePlay gameRef)
        {
            Instance = new PlayerFactory(gameRef);
        }

        public GameObject GetPlayer(Vector2? position = null)
        {
            var player = new GameObject();
            player.ConstructionStamp = "Player";
            player.Team = 0;

            player.Scale = 2f;

            player.ID = MechanicSingleton.Instance.GetNextObjectCountValue();

            var mover = new WalkingMover(player);
            mover.SetMovementNoF(200, .5f);

            player.SetPhysics(new CollidingPhysics(player, gameRef.Level.Bounds));
            if (position == null)
            {
                position = new Vector2(MechanicSingleton.Instance.rand.Next(1, 100));
            }

            player.Position = position.Value;

            var renderer = new RenderBase(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations(player.Scale));
            player.SetRenderer(renderer);
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));
            player.SetMover(mover);
            player.SetEngine(new WalkerEngine(player));
            player.SetCombat(new CombatBase(player));
            player.SetSkills(new SkillsComponent(player));

            player.EquipWeapon(GetSword(player));
            player.EquipRage(GetRage(player));
            player.Skills.SetBoost(new SimpleBoostSkill(player.Skills, player));

            gameRef.container.AddObject(player);

            var parms = new CreateParameters()
            {
                ConstructorId = player.ConstructionStamp,
                ID = player.ID,
                Position = player.Position
            };

            NetCommander.CreatedObject(player);

            gameRef.controlManager.BuildHUDForPlayer(player);

            return player;
        }

        public GameObject GetEnemyFootSoldier(int id = -1)
        {
            var enemy = new GameObject();
            enemy.ConstructionStamp = "EnemyFootSoldier";

            enemy.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;

            enemy.SetPhysics(new CollidingPhysics(enemy, gameRef.Level.Bounds));
            enemy.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500));
            //var renderer = new RenderBase(enemy, AssetRepository.Instance.Slime, AssetRepository.Instance.SlimeAnimations);

            enemy.Scale = 2f;
            var renderer = new RenderBase(enemy, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations(enemy.Scale));
            //var renderer = new RenderBase(enemy, AssetRepository.Instance.Slime, AssetRepository.Instance.SlimeAnimations(enemy.Scale));

            enemy.SetRenderer(renderer);

            var mover = new WalkingMover(enemy);
            mover.SetMovementNoMu(100, 75);
            enemy.SetMover(mover);
            enemy.SetEngine(new WalkerEngine(enemy));

            if (id == -1)
            {
                enemy.SetBrain(new FootSoldierBrain(enemy, gameRef.container.GetObjectById(0)));
            }
            else
            {
                enemy.SetBrain(new NetworkPlayerBrain(enemy));
            }

            enemy.SetCombat(new CombatBase(enemy));
            enemy.SetSkills(new SkillsComponent(enemy));

            // wire up the events between components
            enemy.EquipWeapon(GetSword(enemy));

            gameRef.container.AddObject(enemy);

            if (id == -1)
            {
                NetCommander.CreatedObject(enemy);
            }

            enemy.SpeedAccel = 25f;
            enemy.Team = 1;

            return enemy;
        }

        public GameObject GetEnemySlime(int id = -1)
        {
            var enemy = new GameObject();
            enemy.ConstructionStamp = "EnemySlime";

            enemy.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;

            enemy.SetPhysics(new CollidingPhysics(enemy, gameRef.Level.Bounds));
            enemy.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500));

            enemy.Scale = 2.5f;
            var renderer = new RenderBase(enemy, AssetRepository.Instance.Slime, AssetRepository.Instance.SlimeAnimations(enemy.Scale));

            enemy.SetRenderer(renderer);

            var mover = new WalkingMover(enemy);
            mover.SetMovementNoMu(50, 75);
            enemy.SetMover(mover);
            enemy.SetEngine(new WalkerEngine(enemy));

            if (id == -1)
            {
                enemy.SetBrain(new FootSoldierBrain(enemy, gameRef.container.GetObjectById(0)));
            }
            else
            {
                enemy.SetBrain(new NetworkPlayerBrain(enemy));
            }
            enemy.SetSkills(new SkillsComponent(enemy));
            enemy.SetCombat(new CombatBase(enemy));

            //enemy.EquipWeapon(GetSword(enemy));

            gameRef.container.AddObject(enemy);

            if (id == -1)
            {
                NetCommander.CreatedObject(enemy);
            }

            enemy.SpeedAccel = 25f;
            enemy.Team = 1;

            return enemy;
        }
        public GameObject GetNetworkPlayer(int id = -1)
        {
            var player = new GameObject();
            player.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            player.Team = 0;
            player.Position = new Vector2(100, 100);

            player.SetPhysics(new CollidingPhysics(player, gameRef.Level.Bounds));
            var renderer = new RenderBase(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations(player.Scale));
            player.SetRenderer(renderer);
            player.SetBrain(new NetworkPlayerBrain(player));
            player.SetCombat(new COMBATPLACEHOLDER(player));
            player.SetMover(new WalkingMover(player));
            player.SetEngine(new WalkerEngine(player));
            player.EquipWeapon(GetSword(player));

            gameRef.container.AddObject(player);

            return player;
        }

        public RageBase GetRage(GameObject owner)
        {
            var rage = new RageExplosion(0, owner.Skills, owner);
            rage.Scale = 16f;
            var rageRenderer = new RenderBase(rage, AssetRepository.Instance.Rage2, AssetRepository.Instance.Rage2Animations(rage.Scale));
            rage.SetRenderer(rageRenderer);
            
            return rage;
        }

        public WeaponBase GetSword(GameObject owner)
        {
            var sword = new WeaponBase(owner.Skills, owner);

            var swordRenderer = new RenderBase(sword, AssetRepository.Instance.SwordTexture, AssetRepository.Instance.SwordAnimations(owner.Scale));
            sword.SetRenderer(swordRenderer);

            return sword;
        }

        public WeaponBase GetAxe(GameObject owner)
        {
            var axe = new WeaponBase(owner.Skills, owner);
            var axeRenderer = new RenderBase(axe, AssetRepository.Instance.AxeTexture, AssetRepository.Instance.AxeAnimations(owner.Scale));
            axe.SetRenderer(axeRenderer);
            return axe;
        }

        public WeaponBase GetMace(GameObject owner)
        {
            var mace = new WeaponBase(owner.Skills, owner);
            var maceRenderer = new RenderBase(mace, AssetRepository.Instance.MaceTexture, AssetRepository.Instance.MaceAnimations(owner.Scale));
            mace.SetRenderer(maceRenderer);
            return mace;
        }
    }
}
