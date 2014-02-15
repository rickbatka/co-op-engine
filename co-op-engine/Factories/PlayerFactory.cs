using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Components.Brains;
using co_op_engine.Components.Input;
using co_op_engine.Components.Movement;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Factories
{
    public class PlayerFactory
    {
        public static GameObject GetPlayer(Game1 theGame, ElasticQuadTree tree, Texture2D texture, TileSheet tileSheet)
        {
            var player = new GameObject();
            player.SetMover(new MoverBase(player));
            player.SetPhysics(new NonCollidingPhysics(player, tree));
            player.SetRenderer(new AnimatedRenderer(player, texture, tileSheet));
            player.SetBrain(new PlayerBrain(player, new PlayerControlInput()));

            return player;
        }
    }
}
