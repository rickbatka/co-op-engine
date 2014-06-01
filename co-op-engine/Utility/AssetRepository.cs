using co_op_engine.Collections;
using co_op_engine.GameStates;
using co_op_engine.World;
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
        public Texture2D SoftPixelTexture;
        public Texture2D TowerTexture;
        public Texture2D ArrowTexture;
        public Texture2D HeroTexture;
        public Texture2D SwordTexture;
        public Texture2D AxeTexture;
        public Texture2D MaceTexture;
        public Texture2D DebugFillTexture;
        public Texture2D Circle;
        public Texture2D HealBeam;
        public Texture2D FuzzyLazer;
        public Texture2D Slime;
        public BackgroundTile BushesTile;

        string[] heroAnimationData;
        public AnimationSet HeroAnimations(float scale){ return AnimationSet.BuildFromAsset(heroAnimationData, scale); } 
        string[] swordAnimationData;
        public AnimationSet SwordAnimations(float scale){ return AnimationSet.BuildFromAsset(swordAnimationData, scale); }
        string[] axeAnimationData;
        public AnimationSet AxeAnimations(float scale){ return AnimationSet.BuildFromAsset(axeAnimationData, scale); }
        string[] maceAnimationData;
        public AnimationSet MaceAnimations(float scale){ return AnimationSet.BuildFromAsset(maceAnimationData, scale); }
        string[] towerAnimationData;
        public AnimationSet TowerAnimations(float scale){ return AnimationSet.BuildFromAsset(towerAnimationData, scale); }
        string[] arrowAnimationData;
        public AnimationSet ArrowAnimations(float scale){ return AnimationSet.BuildFromAsset(arrowAnimationData, scale); }
        string[] slimeAnimationData;
        public AnimationSet SlimeAnimations(float scale){ return AnimationSet.BuildFromAsset(slimeAnimationData, scale); }

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


            DebugFillTexture = new Texture2D(gameRef.GraphicsDevice, 1, 1);
            DebugFillTexture.SetData<Color>(new[] { Color.White });
            
            DebugGridTexture = gameRef.Content.Load<Texture2D>("grid");
            PlainWhiteTexture = gameRef.Content.Load<Texture2D>("pixel");
            PlainWhiteTexture.SetData<Color>(new Color[] { Color.White });
            SoftPixelTexture = gameRef.Content.Load<Texture2D>("softpixel");
            SoftPixelTexture.SetData<Color>(new Color[] { Color.White });
            ArrowTexture = gameRef.Content.Load<Texture2D>("arrow");
            TowerTexture = gameRef.Content.Load<Texture2D>("tower");
            HeroTexture = gameRef.Content.Load<Texture2D>("HeroNoArms");
            SwordTexture = gameRef.Content.Load<Texture2D>("Sword");
            AxeTexture = gameRef.Content.Load<Texture2D>("Axe");
            MaceTexture = gameRef.Content.Load<Texture2D>("Mace");
            Circle = gameRef.Content.Load<Texture2D>("circle");
            HealBeam = gameRef.Content.Load<Texture2D>("HealZap");
            FuzzyLazer = gameRef.Content.Load<Texture2D>("FuzzyLazer");
            Slime = gameRef.Content.Load<Texture2D>("slime");
            BushesTile = new BackgroundTile(gameRef.Content.Load<Texture2D>("bushes"), 450, 450);

            heroAnimationData = File.ReadAllLines("content/HeroNoArmsData.txt");
            swordAnimationData = File.ReadAllLines("content/SwordData.txt");
            axeAnimationData = File.ReadAllLines("content/AxeData.txt");
            maceAnimationData = File.ReadAllLines("content/MaceData.txt");
            towerAnimationData = File.ReadAllLines("content/TowerData.txt");
            arrowAnimationData = File.ReadAllLines("content/arrowData.txt");
            slimeAnimationData = File.ReadAllLines("content/slimeData.txt");
        }
    }
}
