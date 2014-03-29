using co_op_engine.Collections;
using co_op_engine.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public class AssetRepository
    {
        public static AssetRepository Instance;
        private Game1 gameRef;

        public SpriteFont Arial;

        public Texture2D DebugGridTexture;
        public Texture2D PlainWhiteTexture;
        public Texture2D TowerTexture;
        public Texture2D ArrowTexture;
        public Texture2D HeroTexture;
        public Texture2D SwordTexture;
        public Texture2D AxeTexture;
        public Texture2D MaceTexture;

        string[] heroAnimationData;
        public AnimationSet HeroAnimations { get { return AnimationSet.BuildFromAsset(heroAnimationData); } }
        string[] swordAnimationData;
        public AnimationSet SwordAnimations { get { return AnimationSet.BuildFromAsset(swordAnimationData); } }
        string[] axeAnimationData;
        public AnimationSet AxeAnimations { get { return AnimationSet.BuildFromAsset(axeAnimationData); } }
        string[] maceAnimationData;
        public AnimationSet MaceAnimations { get { return AnimationSet.BuildFromAsset(maceAnimationData); } }
        string[] towerAnimationData;
        public AnimationSet TowerAnimations { get { return AnimationSet.BuildFromAsset(towerAnimationData); } }

        public void TempSetWindowText(string text)
        {
            gameRef.Window.Title = text;
        }

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
            Arial = gameRef.Content.Load<SpriteFont>("Arial");

            DebugGridTexture = gameRef.Content.Load<Texture2D>("grid");
            PlainWhiteTexture = gameRef.Content.Load<Texture2D>("pixel");
            PlainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            ArrowTexture = gameRef.Content.Load<Texture2D>("arrow");
            TowerTexture = gameRef.Content.Load<Texture2D>("tower");
            HeroTexture = gameRef.Content.Load<Texture2D>("HeroNoArms");
            SwordTexture = gameRef.Content.Load<Texture2D>("Sword");
            AxeTexture = gameRef.Content.Load<Texture2D>("Axe");
            MaceTexture = gameRef.Content.Load<Texture2D>("Mace");

            heroAnimationData = File.ReadAllLines("content/HeroNoArmsData.txt");
            swordAnimationData = File.ReadAllLines("content/SwordData.txt");
            axeAnimationData = File.ReadAllLines("content/AxeData.txt");
            maceAnimationData = File.ReadAllLines("content/MaceData.txt");
            towerAnimationData = File.ReadAllLines("content/TowerData.txt");
        }
    }
}
