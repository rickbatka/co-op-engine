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
        private void HandleHitEvent(object sender, EventArgs e) { }//hits
        private void HandleKillEvent(object sender, EventArgs e) { }//kills
        private void HandleDamageEvent(object sender, EventArgs e) { }//damage
        private void HandleSpawnEvent(object sender, EventArgs e) { }//spawns
        private void HandleErrorEvent(object sender, EventArgs e) { }//errors
        private void HandleNetworkEvent(object sender, EventArgs e) { }//network

        //actions:
        //spawning, directly or setting up systems and letting them go
        //  * if fire and forget, not really an action to notify of
        //resource monitoring (towers)
        //  * the inventory/buy management system needs to be aware of this value, but events seem silly for it
        public event EventHandler<EventArgs> OnMatchStateChanged;//pausing or game flow interruption(matchstate)
        public event EventHandler<EventArgs> OnMultiplayerConnectionChanged;//connection management(networking)
        //saving/loading maybe
        //  * not an event....
        //evaluating win/lose and triggering correct response
        public event EventHandler<EventArgs> OnMatchEnd;

        //things that talk to this(need info route, most likely through factories):
        //effects doing damage/killing (factory -> gameobject -> effect)
        //players constructing towers (factory -> gameobject -> brain or whatever controls it)
        //spawners notify of spawn (spawnerfactory?)
        //error recovery from EVERYWHERE? maybe this should be in a logging class
        //network conenction, add / remove, handle adjustment (has to go through gameplay)

        private string SmallStatusText = "Match in progress.";
        private string BigCenterBannerText = "Game over?";

        private int PlayerOneDeaths = 0;
        private int PlayerOneKills = 0;

        public GameDirectorBase()
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
