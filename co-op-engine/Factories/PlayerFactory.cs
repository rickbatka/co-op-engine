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

namespace co_op_engine.Factories
{
    class PlayerFactory
    {
        public static GameObject GetPlayer(ObjectContainer container, Texture2D texture, AnimationSet animations)
        {
            var player = new GameObject();
            player.SetPhysics(new CollidingPhysics(player));
            var renderer = new AnimatedRenderer(player, texture, animations);
            player.SetRenderer(renderer);
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));

            // wire up the events between components
            player.brain.OnActorStateChanged += renderer.HandleStateChange;
            player.physics.OnActorDirectionChanged += renderer.HandleDirectionChange;

            container.AddObject(player);
            container.SetPlayer(player);

            return player;
        }
    }
}
