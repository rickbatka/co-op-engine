using co_op_engine.Collections;
using co_op_engine.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Content
{
    class AssetRepository
    {
        public static AssetRepository Instance;
        private Game1 gameRef;

        public Texture2D DebugGridTexture;
        public Texture2D PlainWhiteTexture;
        public Texture2D TowerTexture;
        public Texture2D ArrowTexture;
        public Texture2D HeroTexture;
        public Texture2D SwordTexture;

        public AnimationSet HeroAnimations;
        public AnimationSet SwordAnimations;

        private AssetRepository(Game1 gameRef)
        {
            this.gameRef = gameRef;
        }

        public static void Initialize(Game1 gameRef)
        {
            Instance = new AssetRepository(gameRef);
            Instance.LoadContent();
        }

        private void LoadContent()
        {
            DebugGridTexture = gameRef.Content.Load<Texture2D>("grid");
            PlainWhiteTexture = gameRef.Content.Load<Texture2D>("pixel");
            PlainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            ArrowTexture = gameRef.Content.Load<Texture2D>("arrow");
            TowerTexture = gameRef.Content.Load<Texture2D>("tower");
            HeroTexture = gameRef.Content.Load<Texture2D>("HeroNoArms");
            SwordTexture = gameRef.Content.Load<Texture2D>("Sword");

            HeroAnimations = AnimationSet.BuildFromAsset("content/HeroNoArmsData.txt");
            SwordAnimations = AnimationSet.BuildFromAsset("content/SwordData.txt");
        }
    }
}
