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

namespace co_op_engine.Factories
{
    class PlayerFactory
    {
        public static PlayerFactory Instance;
        private GamePlay gameRef;
        private NetworkBase netRef;

        private PlayerFactory(GamePlay gameRef, NetworkBase netref)
        {
            this.gameRef = gameRef;
            this.netRef = netref;
        }

        public static void Initialize(GamePlay gameRef, NetworkBase netref)
        {
            Instance = new PlayerFactory(gameRef, netref);
        }

        public GameObject GetPlayer()
        {
            var player = new GameObject();
            player.ConstructionStamp = "Player";

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
                ConstructionId = player.ConstructionStamp,
                ID = player.ID,
                Position = player.Position
            };

            netRef.Input.Add(new CommandObject()
            {
                ClientId = netRef.ClientId,
                Command = new GameObjectCommand()
                {
                    CommandType = GameObjectCommandType.Create,
                    Parameters = parms,
                },
            });

            return player;
        }

        public GameObject GetEnemy()
        {
            var enemy = new GameObject();
            enemy.ConstructionStamp = "Enemy";

            enemy.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            enemy.Position = new Vector2(MechanicSingleton.Instance.rand.Next(100, 500));

            enemy.SetPhysics(new CollidingPhysics(enemy));
            var renderer = new RenderBase(enemy, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            enemy.SetRenderer(renderer);
            enemy.SetBrain(new DoNothingBrain(enemy));
            enemy.SetCombat(new CombatBase(enemy));

            // wire up the events between components
            enemy.EquipWeapon(GetAxe(enemy));

            gameRef.container.AddObject(enemy);

            var parms = new CreateParameters()
            {
                ConstructionId = enemy.ConstructionStamp,
                ID = enemy.ID,
                Position = enemy.Position
            };
            netRef.Input.Add(new CommandObject()
            {
                ClientId = netRef.ClientId,
                Command = new GameObjectCommand()
                {
                    CommandType = GameObjectCommandType.Create,
                    Parameters = parms,
                },
            });

            return enemy;
        }

        public GameObject GetNetworkPlayer(int id = -1)
        {
            var player = new GameObject();
            player.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
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

        public Sword GetSword(GameObject owner)
        {
            var sword = new Sword(owner);

            var swordRenderer = new RenderBase(sword, AssetRepository.Instance.SwordTexture, AssetRepository.Instance.SwordAnimations);
            sword.SetRenderer(swordRenderer);

            sword.EquipEffect(new BasicDamageEffectDefinition(
                durationMS: swordRenderer.animationSet.GetAnimationDuration(Constants.WEAPON_STATE_ATTACKING_PRIMARY, owner.FacingDirection),
                damageRating: 25
            ));
            return sword;
        }

        public Sword GetAxe(GameObject owner)
        {
            var axe = new Sword(owner);
            var axeRenderer = new RenderBase(axe, AssetRepository.Instance.AxeTexture, AssetRepository.Instance.AxeAnimations);
            axe.SetRenderer(axeRenderer);
            return axe;
        }

        public Sword GetMace(GameObject owner)
        {
            var mace = new Sword(owner);
            var maceRenderer = new RenderBase(mace, AssetRepository.Instance.MaceTexture, AssetRepository.Instance.MaceAnimations);
            mace.SetRenderer(maceRenderer);
            return mace;
        }
    }
}
