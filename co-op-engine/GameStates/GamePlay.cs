using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using co_op_engine.Collections;
using co_op_engine.Components;
using co_op_engine.Factories;
using co_op_engine.ServiceProviders;
using co_op_engine.Utility;
using co_op_engine.World.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.GameStates
{
    class GamePlay : GameState
    {
        private Texture2D plainWhiteTexture;
        private Texture2D towerTexture;
        private Texture2D arrowTexture;
        private Texture2D knightTexture;

        ObjectContainer container;
        //List<GameObject> Players = new List<GameObject>();
        //List<GameObject> Enemies = new List<GameObject>();
        //List<GameObject> Towers = new List<GameObject>();

        //GameObject devPlayerObject;
        //ElasticQuadTree tree;

        public static Texture2D DEBUG_GRID_TEXTURE;

        public GamePlay(Game1 game)
            : base(game)
        {
            container = new ObjectContainer(GameRef.screenRectangle);
            Camera.Instantiate(GameRef.screenRectangle);
        }

        public override void LoadContent()
        {

            DEBUG_GRID_TEXTURE = GameRef.Content.Load<Texture2D>("grid");

            plainWhiteTexture = GameRef.Content.Load<Texture2D>("pixel");
            plainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            arrowTexture = GameRef.Content.Load<Texture2D>("arrow");
            towerTexture = GameRef.Content.Load<Texture2D>("tower");
            knightTexture = GameRef.Content.Load<Texture2D>("ww");

            var animations = AnimationSet.BuildFromAsset("content/ww.txt");

            ///////////////////////////////////////////////////////////

            //@TODO move to level setup
            PlayerFactory.GetPlayer(container, knightTexture, animations);

            //Players.Add(PlayerFactory.GetPlayer(this, tree, knightTexture, new TileSheet(animation)));

            ////@TODO EnemyFactory
            //var devEnemy = new GameObject();
            ////devEnemy.SetBrain(new StepFollow(devEnemy));
            //devEnemy.SetBrain(new DoNothingBrain(devEnemy));
            //devEnemy.SetupDevTempComponents(arrowTexture, tree);
            //Enemies.Add(devEnemy);

            TowerFactory.GetDoNothingTower(container, towerTexture, plainWhiteTexture);

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
            container.DebugDraw(GameRef.spriteBatch, DEBUG_GRID_TEXTURE);

            GameRef.spriteBatch.End();
        }

        public List<GameObject> GetPlayers()
        {
            return container.GetPlayers();
        }
    }
}
