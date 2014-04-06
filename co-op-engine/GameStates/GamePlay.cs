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
using co_op_engine.Networking.Commands;
using co_op_engine.Components.Particles;
using Microsoft.Xna.Framework.Input;
using co_op_engine.World;

namespace co_op_engine.GameStates
{
    public class GamePlay : GameState
    {
        public ObjectContainer container;

        public NetworkBase Networking;

        public TiledBackground Background;

        private bool isHosting;

        public GamePlay(Game1 game, NetworkBase network)
            : base(game)
        {
            container = new ObjectContainer(GameRef.screenRectangle);
            network.RegisterWorldWithNetwork(container);
            Camera.Instantiate(GameRef.screenRectangle);
            PlayerFactory.Initialize(this, network);
            TowerFactory.Initialize(this, network);
            NetworkFactory.Initialize(this, network);
            Networking = network;
        }

        public override void LoadContent()
        {
            Background = new TiledBackground(AssetRepository.Instance.BushesTile);
            ///////////////////////////////////////////////////////////

            //@TODO move to level setup
            PlayerFactory.Instance.GetPlayer();
            //TowerFactory.GetDoNothingTower(container, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.PlainWhiteTexture);

            //var devTower = TowerFactory.GetDoNothingTower(this, tree, towerTexture, plainWhiteTexture);
            //Towers.Add(devTower);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameTimerManager.Instance.Update(gameTime);
            Background.Update(gameTime);
            container.UpdateAll(gameTime, Networking);

#warning temporary 
            if (InputHandler.KeyDown(Keys.P))
            {
                //int numParticlesToSpawn = MechanicSingleton.Instance.rand.Next(1, 10);
                //for (int i = 0; i < 10; i++)
                //{
                //ParticleEngine.Instance.AddEmitter(
                //  new FireEmitter(container.GetObjectById(0))
                //);
                //}
            }

            ParticleEngine.Instance.Update(gameTime);

            var netCommands = Networking.Output.Gather();

            if (netCommands.Count > 0)
            {
                foreach (var command in netCommands)
                {
                    ParseAndExecuteNetworkCommand(command);
                }
            }
        }

        private void ParseAndExecuteNetworkCommand(CommandObject command)
        {
            //parse command type
            var objectCommand = (GameObjectCommand)command.Command;

            switch (objectCommand.CommandType)
            {
                case GameObjectCommandType.Create:
                    {
                        //use factory to create object
                        NetworkFactory.BuildFromNetwork((CreateParameters)objectCommand.Parameters);
                    }
                    break;
                case GameObjectCommandType.Update:
                    {
                        //findby id and run update from network
                        var parameters = (UpdateParameters)objectCommand.Parameters;
                        var updatee = container.GetObjectById(parameters.ID);
                        updatee.UpdateFromNetworkParams(parameters);
                    }
                    break;
                default:
                    {
                        throw new NotImplementedException("this command type has not been implemented yet");
                    }
                    break;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameRef.spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointWrap, null, null, null, Camera.Instance.Transformation);
            Background.Draw(GameRef.spriteBatch);
            container.DrawAll(GameRef.spriteBatch);

            ParticleEngine.Instance.Draw(GameRef.spriteBatch);

            //@DEBUGDRAW DEBUG DRAW
            //container.DebugDraw(GameRef.spriteBatch, AssetRepository.Instance.DebugGridTexture);
            DebugDrawStrings(gameTime);


            GameRef.spriteBatch.End();
        }

        private void DebugDrawStrings(GameTime gameTime)
        {
            string[] debugInfos = new string[] 
            { 
                "fps:" + 1000f / (float)gameTime.ElapsedGameTime.Milliseconds,
                "obj count:" + container.ObjectCount
            };

            for (int i = 0; i < debugInfos.Length; i++)
            {
                GameRef.spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: debugInfos[i],
                    position: new Vector2(25, (i + 1) * 25),
                    color: Color.White
                );
            }
        }


        public Rectangle ScreenRectangle { get { return GameRef.screenRectangle; } }
        public int GridSize { get { return GameRef.gridSize; } }
    }
}
