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
using co_op_engine.Content;

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
            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new AnimatedRenderer(player, AssetRepository.Instance.KnightTexture, AssetRepository.Instance.KnightAnimations);
            player.SetRenderer(renderer);
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));

            // wire up the events between components
            player.brain.OnActorStateChanged += renderer.HandleStateChange;
            player.physics.OnActorDirectionChanged += renderer.HandleDirectionChange;

            gameRef.container.AddObject(player);
            gameRef.container.SetPlayer(player);

            return player;
        }
    }
}
