﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Networking;
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
            controlManager.controls.Add(createGameButton);

            var joinGameButton = new Button(
                AssetRepository.Instance.DebugGridTexture,
                "Join Game",
                AssetRepository.Instance.DebugGridTexture,
                AssetRepository.Instance.DebugGridTexture,
                new Point(100, 500),
                AssetRepository.Instance.Arial);
            joinGameButton.OnInteracted += ClientConnectGamePlay;
            controlManager.controls.Add(joinGameButton);
        }

        private void StartServerGameplay(object sender, EventArgs e)
        {
            var network = new NetworkServer();
            network.StartHosting();

            var gameplayServer = new GamePlay(GameRef,network);
            GameRef.ChangeGameState(gameplayServer);
        }

        private void ClientConnectGamePlay(object sender, EventArgs e)
        {
            var network = new NetworkClient();
            network.ConnectToGame("127.0.0.1");

            var gameplayClient = new GamePlay(GameRef, network);
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
