using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Factories;
using co_op_engine.Utility;
using co_op_engine.World.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using co_op_engine.Networking;

namespace co_op_engine.GameStates
{
    public class GamePlay : GameState
    {
        public ObjectContainer container;
        public NetworkServer server;

        public GamePlay(Game1 game)
            : base(game)
        {
            container = new ObjectContainer(GameRef.screenRectangle);
            Camera.Instantiate(GameRef.screenRectangle);
            PlayerFactory.Initialize(this);
            TowerFactory.Initialize(this);
            server = new NetworkServer();
        }

        public override void LoadContent()
        {


            server.StartHosting();

            ///////////////////////////////////////////////////////////

            //@TODO move to level setup
            PlayerFactory.Instance.GetPlayer();

            TowerFactory.Instance.GetDoNothingTower();
            //TowerFactory.GetDoNothingTower(container, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.PlainWhiteTexture);

            //var devTower = TowerFactory.GetDoNothingTower(this, tree, towerTexture, plainWhiteTexture);
            //Towers.Add(devTower);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameTimerManager.Instance.Update(gameTime);
            container.UpdateAll(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameRef.spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointWrap, null, null,null,Camera.Instance.Transformation);
            container.DrawAll(GameRef.spriteBatch);

            //@DEBUGDRAW DEBUG DRAW
            container.DebugDraw(GameRef.spriteBatch, AssetRepository.Instance.DebugGridTexture);
            DebugDrawStrings(gameTime);


            GameRef.spriteBatch.End();
        }

        private void DebugDrawStrings(GameTime gameTime)
        {
            string[] debugInfos = new string[] 
            { 
                "fps:" + 1000 / gameTime.ElapsedGameTime.Milliseconds,
                "obj count:" + container.ObjectCount
            };

            for(int i = 0; i < debugInfos.Length; i ++)
            {
                GameRef.spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: debugInfos[i],
                    position: new Vector2(25, (i+1)*25),
                    color: Color.White
                );
            }

            
        }

        public List<GameObject> GetPlayers()
        {
            return container.GetPlayers();
        }

        public Rectangle ScreenRectangle { get { return GameRef.screenRectangle; } }
        public int GridSize { get { return GameRef.gridSize; } }
    }
}
