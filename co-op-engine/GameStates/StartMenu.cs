using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.UIElements;
using co_op_engine.Utility;
using co_op_engine.World;
using Microsoft.Xna.Framework;
using co_op_engine.World.Level;
using co_op_engine.Factories;

namespace co_op_engine.GameStates
{
    class StartMenu : GameState
    {
        public StartMenu(Game1 game)
            : base(game)
        {
            var createGameButtonLvl1 = new Button(
                AssetRepository.Instance.DebugGridTexture, 
                "Create Game, Level: Dead Man's Gulch", 
                AssetRepository.Instance.DebugGridTexture,
                AssetRepository.Instance.DebugGridTexture,
                new Point(100,300),
                AssetRepository.Instance.Arial);
            createGameButtonLvl1.OnInteracted += StartServerGameplayLevel1;
            createGameButtonLvl1.OnLeftClick += StartServerGameplayLevel1;
            controlManager.AddControl(createGameButtonLvl1);
            createGameButtonLvl1.Selected = true;

            var createGameButtonLvl2 = new Button(
                AssetRepository.Instance.DebugGridTexture,
                "Create Game, Level: Edge of the Universe",
                AssetRepository.Instance.DebugGridTexture,
                AssetRepository.Instance.DebugGridTexture,
                new Point(100, 400),
                AssetRepository.Instance.Arial);
            createGameButtonLvl2.OnInteracted += StartServerGameplayLevel2;
            createGameButtonLvl2.OnLeftClick += StartServerGameplayLevel2;
            controlManager.AddControl(createGameButtonLvl2);

            var joinGameButton = new Button(
                AssetRepository.Instance.DebugGridTexture,
                "Join Game",
                AssetRepository.Instance.DebugGridTexture,
                AssetRepository.Instance.DebugGridTexture,
                new Point(100, 500),
                AssetRepository.Instance.Arial);
            joinGameButton.OnInteracted += ClientConnectGamePlay;
            joinGameButton.OnLeftClick += ClientConnectGamePlay;
            controlManager.AddControl(joinGameButton);
        }

        private void StartServerGameplayLevel1(object sender, EventArgs e)
        {
            StartWithLevel(
                level: LevelFactory.Instance.GetLevel1()
            );
        }

        private void StartServerGameplayLevel2(object sender, EventArgs e)
        {
            StartWithLevel(
                level: LevelFactory.Instance.GetLevel2()
            );
        }

        private void StartWithLevel(Level level)
        {
            MechanicSingleton.InitializeWithSettings("ServerTesting", 12);

            var network = new NetworkServer();
            network.StartHosting();

            NetCommander.SetNetwork(network);

            var gameplayServer = new GamePlay(GameRef, level);
            GameRef.ChangeGameState(gameplayServer);
        }

        private void ClientConnectGamePlay(object sender, EventArgs e)
        {
            MechanicSingleton.InitializeWithSettings("ClientTesting", 3);

            var network = new NetworkClient();
            network.ConnectToGame("127.0.0.1");

            NetCommander.SetNetwork(network);

            var gameplayClient = new GamePlay(GameRef, LevelFactory.Instance.GetLevel1());
            GameRef.ChangeGameState(gameplayClient);
        }

        public override void LoadContent()
        {
            //load what is needed into assets
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();

            controlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }
    }

    public class LevelSelectEventArgs : EventArgs
    {
        public Level Level;
    }
}
