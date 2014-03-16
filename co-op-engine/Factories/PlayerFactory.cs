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
            gameRef.container.SetPlayer(player);

            return player;
        }

        public GameObject GetNetworkPlayer(InitialNetworkData networkData)
        {
            var player = new GameObject();
            player.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            player.Position = new Vector2(100, 100);

            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new AnimatedRenderer(player, AssetRepository.Instance.HeroTexture, AssetRepository.Instance.HeroAnimations);
            player.SetRenderer(renderer);
            player.SetBrain(new NetworkClientBrain(player));
            player.SetCombat(new COMBATPLACEHOLDER(player));

            return player;
        }

        public MeleeWeapon GetSword(GameObject owner)
        {
            var sword = new MeleeWeapon(owner);

            var swordRenderer = new AnimatedRenderer(sword, AssetRepository.Instance.SwordTexture, AssetRepository.Instance.SwordAnimations);
            sword.SetRenderer(swordRenderer);
            return sword;
        }

        public MeleeWeapon GetAxe(GameObject owner)
        {
            var axe = new MeleeWeapon(owner);
            var axeRenderer = new AnimatedRenderer(axe, AssetRepository.Instance.AxeTexture, AssetRepository.Instance.AxeAnimations);
            axe.SetRenderer(axeRenderer);
            return axe;
        }

        public MeleeWeapon GetMace(GameObject owner)
        {
            var mace = new MeleeWeapon(owner);
            var maceRenderer = new AnimatedRenderer(mace, AssetRepository.Instance.MaceTexture, AssetRepository.Instance.MaceAnimations);
            mace.SetRenderer(maceRenderer);
            return mace;
        }
    }
}
