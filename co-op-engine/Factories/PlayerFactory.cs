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
            player.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            player.Position = new Vector2(MechanicSingleton.Instance.rand.Next(1, 100));

            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new AnimatedRenderer(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            player.SetRenderer(renderer);
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));
            player.SetCombat(new CombatBase(player));

            // wire up the events between components
            player.EquipWeapon(GetSword(player));

            gameRef.container.AddObject(player);

            var parms = player.BuildCreateParams();
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

        public GameObject GetNetworkPlayer(int id = -1)
        {
            var player = new GameObject();
            player.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            player.Position = new Vector2(100, 100);

            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new AnimatedRenderer(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            player.SetRenderer(renderer);
            player.SetBrain(new NetworkClientBrain(player));
            player.SetCombat(new COMBATPLACEHOLDER(player));
            player.EquipWeapon(GetSword(player));

            gameRef.container.AddObject(player);

            return player;
        }

        public Sword GetSword(GameObject owner)
        {
            var sword = new Sword(owner);

            var swordRenderer = new AnimatedRenderer(sword, AssetRepository.Instance.SwordTexture, AssetRepository.Instance.SwordAnimations);
            sword.SetRenderer(swordRenderer);
            return sword;
        }

        public Sword GetAxe(GameObject owner)
        {
            var axe = new Sword(owner);
            var axeRenderer = new AnimatedRenderer(axe, AssetRepository.Instance.AxeTexture, AssetRepository.Instance.AxeAnimations);
            axe.SetRenderer(axeRenderer);
            return axe;
        }

        public Sword GetMace(GameObject owner)
        {
            var mace = new Sword(owner);
            var maceRenderer = new AnimatedRenderer(mace, AssetRepository.Instance.MaceTexture, AssetRepository.Instance.MaceAnimations);
            mace.SetRenderer(maceRenderer);
            return mace;
        }
    }
}
