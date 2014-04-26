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
using co_op_engine.Utility.Camera;
using co_op_engine.Pathing;

namespace co_op_engine.GameStates
{
    public class GamePlay : GameState
    {
        public ObjectContainer container;

        public TiledBackground Background;

        private bool isHosting;

        public GamePlay(Game1 game)
            : base(game)
        {
            container = new ObjectContainer(new Rectangle(-1000, -1000, 2000, 2000));
            PathFinder.Initialize(container);
            NetCommander.RegisterWorldWithNetwork(container);
            Camera.Instantiate(GameRef.screenRectangle);
            PlayerFactory.Initialize(this);
            TowerFactory.Initialize(this);
            NetworkFactory.Initialize(this);
        }

        public override void LoadContent()
        {
            Background = new TiledBackground(AssetRepository.Instance.BushesTile);
            ///////////////////////////////////////////////////////////

            //@TODO move to level setup
            Camera.Instance.SetCameraTackingObject(PlayerFactory.Instance.GetPlayer());
            Camera.Instance.IsTracking = true;
            //TowerFactory.GetDoNothingTower(container, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.PlainWhiteTexture);

            //var devTower = TowerFactory.GetDoNothingTower(this, tree, towerTexture, plainWhiteTexture);
            //Towers.Add(devTower);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameTimerManager.Instance.Update(gameTime);
            
            container.UpdateAll(gameTime);
            
            ParticleEngine.Instance.Update(gameTime);

            var netCommands = NetCommander.RendPendingCommands();

            if (netCommands.Count > 0)
            {
                foreach (var command in netCommands)
                {
                    RouteNetCommand(command);
                }
            }
            Background.Update(gameTime);
            Camera.Instance.Update(gameTime);
            
        }

        private void RouteNetCommand(NetworkCommandObject command)
        {
            //parse command type
            switch (command.CommandType)
            {
                case NetworkCommandType.GameObjectCommand:
                    {
                        RouteGameObjectCommand(command);
                        //NetworkFactory.BuildFromNetwork((CreateParameters)objectCommand.Parameters);
                    }
                    break;
                /*case GameObjectCommandType.Update:
                    {
                        //findby id and run update from network
                        var parameters = (UpdateParameters)objectCommand.Parameters;
                        var updatee = container.GetObjectById(parameters.ID);
                        updatee.UpdateFromNetworkParams(parameters);
                    }
                    break;*/
                default:
                    {
                        throw new NotImplementedException("this command type has not been implemented yet");
                    }
            }
        }

        private void RouteGameObjectCommand(NetworkCommandObject command)
        {
            GameObjectCommand objCommand = (GameObjectCommand)command.Command;
            switch (objCommand.CommandType)
            {
                case GameObjectCommandType.Create:
                    {
                        NetworkFactory.BuildFromNetwork(objCommand);
                    }
                    break;
                case GameObjectCommandType.Delete:
                    {
                        throw new NotImplementedException("placeholder, probably won't need");
                    }
                    break;
                case GameObjectCommandType.Update:
                    {
                        container.GetObjectById(objCommand.ID).UpdateFromNetworkParams(objCommand);
                    }
                    break;
                default:
                    {
                        throw new NotImplementedException("gameobject command type not implemented");
                    }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameRef.spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.LinearWrap, null, null, null, Camera.Instance.Transformation);
            Background.Draw(GameRef.spriteBatch);
            ParticleEngine.Instance.Draw(GameRef.spriteBatch);
            container.DrawAll(GameRef.spriteBatch);

            //@DEBUGDRAW DEBUG DRAW
            container.DebugDraw(GameRef.spriteBatch);
            DebugDrawStrings(gameTime);
            //PathFinder.Instance.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }

        private void DebugDrawStrings(GameTime gameTime)
        {
            string[] debugInfos = new string[] 
            { 
                "fps:" + 1000 / (int)gameTime.ElapsedGameTime.TotalMilliseconds,
                "obj count:" + container.ObjectCount,
                "net sent:" + NetCommander.SentCount,
                "net recv:" + NetCommander.RecvCount,
            };

            for (int i = 0; i < debugInfos.Length; i++)
            {
                GameRef.spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: debugInfos[i],
                    position: new Vector2(25, (i + 1) * 25),
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    effects: SpriteEffects.None,
                    depth: 1f
                );
            }
        }


        public Rectangle ScreenRectangle { get { return GameRef.screenRectangle; } }
        public int GridSize { get { return GameRef.gridSize; } }
    }
}
