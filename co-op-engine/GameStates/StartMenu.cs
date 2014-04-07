using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.UIElements;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;

namespace co_op_engine.GameStates
{
    class StartMenu : GameState
    {
        public StartMenu(Game1 game)
            : base(game)
        {
            var createGameButton = new Button(
                AssetRepository.Instance.DebugGridTexture, 
                "Create Game", 
                AssetRepository.Instance.DebugGridTexture,
                AssetRepository.Instance.DebugGridTexture,
                new Point(100,300),
                AssetRepository.Instance.Arial);
            createGameButton.OnInteracted += StartServerGameplay;
            createGameButton.OnLeftClick += StartServerGameplay;
            controlManager.AddControl(createGameButton);
            createGameButton.Selected = true;

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

        private void StartServerGameplay(object sender, EventArgs e)
        {
            MechanicSingleton.InitializeWithSettings("ServerTesting", 12);

            var network = new NetworkServer();
            network.StartHosting();

            NetCommander.SetNetwork(network);

            var gameplayServer = new GamePlay(GameRef);
            GameRef.ChangeGameState(gameplayServer);
        }

        private void ClientConnectGamePlay(object sender, EventArgs e)
        {
            MechanicSingleton.InitializeWithSettings("ClientTesting", 3);

            var network = new NetworkClient();
            network.ConnectToGame("127.0.0.1");

            NetCommander.SetNetwork(network);

            var gameplayClient = new GamePlay(GameRef);
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
}
