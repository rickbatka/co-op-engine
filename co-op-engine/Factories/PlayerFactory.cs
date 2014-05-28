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
using co_op_engine.Components.Weapons;
using Microsoft.Xna.Framework;
using co_op_engine.Utility;
using co_op_engine.Components.Combat;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.Components.Weapons.Effects;
using co_op_engine.Components.Brains.AI;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Engines;

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
            var player = new GameObject(gameRef.Level);
            player.ConstructionStamp = "Player";
            player.Friendly = true;

            player.Scale = 2f;

            player.ID = MechanicSingleton.Instance.GetNextObjectCountValue();

            if(position == null)
            {
                position = new Vector2(MechanicSingleton.Instance.rand.Next(1, 100));
            }

            player.Position = position.Value;

            var mover = new WalkingMover(player);
            mover.DefineNoF(200, .5f);

            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new RenderBase(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations(player.Scale));
            player.SetRenderer(renderer);
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));
            player.SetMover(mover);
            player.SetEngine(new WalkerEngine(player));
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

            NetCommander.CreatedObject(player);

            return player;
        }

        public GameObject GetEnemyFootSoldier(int id = -1)
        {
            var enemy = new GameObject(gameRef.Level);
            enemy.ConstructionStamp = "EnemyFootSoldier";

            enemy.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            enemy.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500));

            enemy.SetPhysics(new CollidingPhysics(enemy));
            //var renderer = new RenderBase(enemy, AssetRepository.Instance.Slime, AssetRepository.Instance.SlimeAnimations);

            enemy.Scale = 2f;
            var renderer = new RenderBase(enemy, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations(enemy.Scale));
            //var renderer = new RenderBase(enemy, AssetRepository.Instance.Slime, AssetRepository.Instance.SlimeAnimations(enemy.Scale));

            enemy.SetRenderer(renderer);

            var mover = new WalkingMover(enemy);
            mover.DefineNoMu(150, 100);
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

            // wire up the events between components
            enemy.EquipWeapon(GetSword(enemy));

            gameRef.container.AddObject(enemy);

            if (id == -1)
            {
                NetCommander.CreatedObject(enemy);
            }

            enemy.SpeedAccel = 25f;

            return enemy;
        }

        public GameObject GetEnemySlime(int id = -1)
        {
            var enemy = new GameObject(gameRef.Level);
            enemy.ConstructionStamp = "EnemySlime";

            enemy.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            enemy.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500));

            enemy.SetPhysics(new CollidingPhysics(enemy));

            enemy.Scale = 1.5f;
            var renderer = new RenderBase(enemy, AssetRepository.Instance.Slime, AssetRepository.Instance.SlimeAnimations(enemy.Scale));

            enemy.SetRenderer(renderer);

            enemy.SetMover(new WalkingMover(enemy));
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

            enemy.Children.Add(GetNewSword(enemy));

            gameRef.container.AddObject(enemy);

            if (id == -1)
            {
                NetCommander.CreatedObject(enemy);
            }

            enemy.SpeedAccel = 25f;

            return enemy;
        }
        public GameObject GetNetworkPlayer(int id = -1)
        {
            var player = new GameObject(gameRef.Level);
            player.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            player.Friendly = true;
            player.Position = new Vector2(100, 100);

            player.SetPhysics(new CollidingPhysics(player));
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
