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
    public class GameDirectorBase
    {
        private Level Level;

        private string SmallStatusText = "Match in progress.";
        private string BigCenterBannerText = "Game over?";

        private int PlayerOneDeaths = 0;
        private int PlayerOneKills = 0;

        public GameDirectorBase(Level level)
        {
            Level = level;
            Level.MatchState = MatchStates.Starting;
        }

        public void Update(GameTime gameTime)
        {
            CheckMatchState();
            CheckWinConditions(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Level.MatchState != MatchStates.Playing)
            {
                // draw center banner
                spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: BigCenterBannerText,
                    position: DrawingUtility.PointToVector(Camera.Instance.ViewBoundsRectangle.Center),
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 5f,
                    effects: SpriteEffects.None,
                    depth: 1f
                );
            }
            else
            {
                // draw status text in right
                spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: SmallStatusText,
                    position: new Vector2(x: Camera.Instance.ViewBoundsRectangle.Right - 250, y: Camera.Instance.ViewBoundsRectangle.Top + 100),
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    effects: SpriteEffects.None,
                    depth: 1f
                );
            }
        }

        private void CheckMatchState()
        {
            if(Level.MatchState == MatchStates.Starting)
            {
                // for now, we don't have anything to do 
                ChangeToState(MatchStates.Playing);
            }
        }

        private void ChangeToState(MatchStates matchState)
        {
            Level.MatchState = matchState;
        }

        private void CheckWinConditions(GameTime gameTime)
        {
            //if(PlayerOneKills >= 1)
            if(gameTime.TotalGameTime.TotalSeconds >= 150)
            {
                BigCenterBannerText = "You won.";
                ChangeToState(MatchStates.Ended);
            }
        }
    }
}
