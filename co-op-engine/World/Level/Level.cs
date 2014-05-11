using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.World.Level
{
    /*
     * Starting: because we might have a time where play hasn't yet started, everyone is frozen and the networking is doing stuff. and maybe animation / text on screen / intro to level.
     * Playing: match is underway. constantly evaluate game conditions mainly for win state, maybe time warning or other stuff
     * Ended: because we'll want to display some stuff about the match that just happened before we jump to another screen
     */
    public enum MatchStates { Starting, Playing, Ended }

    public class Level
    {
        private TiledBackground Background;
        private GameDirectorBase GameDirector;
        //private AiDirectorBase AiDirector;

        public Rectangle Bounds;
        public MatchStates MatchState;
        public Vector2 StartingPositionPlayer0;

        public Level()
        {
        }

        public void SetGameDirector(GameDirectorBase gameDirector)
        {
            GameDirector = gameDirector;
        }

        public void Initialize()
        {
            Bounds = new Rectangle(0, 0, 2000, 2000);
            StartingPositionPlayer0 = new Vector2(Bounds.Width / 2, Bounds.Height / 2);
        }

        public void LoadContent()
        {
            Background = new TiledBackground(AssetRepository.Instance.BushesTile);
        }

        public void Update(GameTime gameTime)
        {
            Background.Update(gameTime);
            GameDirector.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            GameDirector.Draw(spriteBatch);

            //debugdraw
            spriteBatch.Draw(
                texture: AssetRepository.Instance.DebugGridTexture,
                destinationRectangle: Bounds,
                sourceRectangle: null,
                color: Color.Black,
                rotation: 0f,
                origin: Vector2.Zero,
                effect: SpriteEffects.None,
                depth: 1f
            );
        }
    }
}
