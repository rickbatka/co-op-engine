using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.World.Level
{
    public enum MatchStates { Starting, Playing, Ended }

    public class GameDirectorBase
    {
        //things the director needs to be aware of:
        //hits
        //kills
        //damage
        //spawns
        //errors
        //network

        //actions:
        //spawning, directly or setting up systems and letting them go
        //resource monitoring (towers)
        //pausing
        //connection management(networking)
        //saving/loading maybe
        //evaluating win/lose and triggering correct response

        //things that talk to this:
        //effects doing damage/killing
        //players constructing towers
        //spawners notify of spawn
        //error recovery from EVERYWHERE? maybe this should be in a logging class
        //network conenction, add / remove, handle adjustment

        private string SmallStatusText = "Match in progress.";
        private string BigCenterBannerText = "Game over?";

        private int PlayerOneDeaths = 0;
        private int PlayerOneKills = 0;

        public GameDirectorBase(Level level)
        {
        }

        public void Update(GameTime gameTime)
        {
            CheckWinConditions(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //should be for debug use only
        }

        private void CheckWinConditions(GameTime gameTime)
        {
            //lets say 50 kills
        }
    }
}
