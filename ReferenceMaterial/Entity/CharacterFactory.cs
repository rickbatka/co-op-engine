using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ReferenceMaterial.Entity
{
	class CharacterFactory
	{

        //static public GameObject GetStaticPlayerKeyboardControls(Texture2D texture, Vector2 startPosition, Level level)
        //{
        //    GameObject player = new GameObject();
        //    player.BrainComponent = new KeyboardBrain(player, 2);
        //    player.PhysicsComponent = new CollidablePhysics(player, startPosition, level);
        //    player.RenderComponent = new BasicRender(player, texture);

        //    return player;
        //}

        //static public GameObject GetPlayerKeyboardControls(Texture2D texture, Vector2 startPosition, Level level, AnimationContainer animationData)
        //{
        //    GameObject player = new GameObject();
        //    player.BrainComponent = new KeyboardBrain(player, 2);
        //    player.PhysicsComponent = new CollidablePhysics(player, startPosition, level);
        //    player.RenderComponent = new AnimatedCharacterRender(player, animationData,texture);

        //    return player;
        //}

        //static public GameObject GetFollowAI(Texture2D texture, Vector2 initialPosition, GameWorld worldRef, Level level)
        //{
        //    GameObject simpleFollower = new GameObject();
        //    simpleFollower.BrainComponent = new FollowBrain(worldRef, 1000, 2, 1, 50, 80, simpleFollower);
        //    simpleFollower.PhysicsComponent = new CollidablePhysics(simpleFollower, initialPosition, level);
        //    simpleFollower.RenderComponent = new BasicRender(simpleFollower, texture);

        //    return simpleFollower;
        //}
	}
}
