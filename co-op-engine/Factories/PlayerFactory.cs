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
            //player.SetPhysics(new NonCollidingPhysics(player));
            player.SetRenderer(new AnimatedRenderer(player, texture, animations));
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));

            container.AddObject(player);
            container.SetPlayer(player);

            return player;
        }
    }
}
