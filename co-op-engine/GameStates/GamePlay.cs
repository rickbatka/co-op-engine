using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Factories;
using co_op_engine.Sound;
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
using co_op_engine.GameRules;

namespace co_op_engine.GameStates
{
    public class GamePlay : GameState
    {
        public Level CurrentLevel;

        private bool isHosting;

        public GamePlay(Game1 game, Level level)
            : base(game)
        {
            CurrentLevel = level;
            CurrentLevel.Initialize();
            PathFinder.Initialize(CurrentLevel.Container);
            NetCommander.RegisterWorldWithNetwork(CurrentLevel.Container);
            Camera.Instantiate(GameRef.screenRectangleActual);
            PlayerFactory.Initialize(this);
            TowerFactory.Initialize(this);
            NetworkFactory.Initialize(this);
            ProjectileFactory.Initialize(this);
            
            //SoundManager.CrossfadeMusic(5000, AssetRepository.Instance.Music1);
        }

        public override void LoadContent()
        {
            CurrentLevel.LoadContent();
        }

        public void ClosingGameplay()
        {
            PathFinder.Instance.ShutDownPathing();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameTimerManager.Instance.Update(gameTime);
            SoundManager.Update(gameTime);
            CurrentLevel.Update(gameTime);
            ParticleEngine.Instance.Update(gameTime);
            
            var netCommands = NetCommander.RendPendingCommands();
            if (netCommands.Count > 0)
            {
                foreach (var command in netCommands)
                {
                    RouteNetCommand(command);
                }
            }

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
                        CurrentLevel.Container.GetObjectById(objCommand.ID).UpdateFromNetworkParams(objCommand);
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
            GameRef.spriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack, 
                blendState: BlendState.NonPremultiplied, 
                samplerState: SamplerState.PointWrap, 
                depthStencilState: null,
                rasterizerState: null, 
                effect: null,
                transformMatrix: Camera.Instance.Transformation
            );
            CurrentLevel.Draw(GameRef.spriteBatch);
            
            CurrentLevel.Container.DrawAll(GameRef.spriteBatch);
            ParticleEngine.Instance.Draw(GameRef.spriteBatch);

#if DEBUGSTRINGS
            container.DebugDraw(GameRef.spriteBatch);
            DebugDrawStrings(gameTime);
            PathFinder.Instance.Draw(GameRef.spriteBatch);
#endif
            controlManager.DrawUI(GameRef.spriteBatch);

            GameRef.spriteBatch.End();


            //UI
            GameRef.spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                SamplerState.PointWrap,
                null,
                null,
                null);


            controlManager.DrawHUD(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }

        private void DebugDrawStrings(GameTime gameTime)
        {
            string[] debugInfos = new string[] 
            { 
                "fps:" + 1000 / (int)gameTime.ElapsedGameTime.TotalMilliseconds,
                "obj count:" + CurrentLevel.Container.ObjectCount,
                "net sent:" + NetCommander.SentCount,
                "net recv:" + NetCommander.RecvCount,
            };

            for (int i = 0; i < debugInfos.Length; i++)
            {
                GameRef.spriteBatch.DrawString(
                    spriteFont: AssetRepository.Instance.Arial,
                    text: debugInfos[i],
                    position: new Vector2(Camera.Instance.ViewBoundsRectangle.Left + 25, Camera.Instance.ViewBoundsRectangle.Top + ((i+1)*25) ),
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    effects: SpriteEffects.None,
                    depth: 1f
                );
            }
        }


        public Rectangle ScreenRectangle { get { return GameRef.screenRectangleActual; } }
        public int GridSize { get { return GameRef.gridSize; } }
    }
}
