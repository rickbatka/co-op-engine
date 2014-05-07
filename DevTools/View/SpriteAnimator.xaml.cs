using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevTools.GraphicsControls;
using DevTools.GraphicsControls.Boiler;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DevTools.View
{
    /// <summary>
    /// Interaction logic for SpriteAnimator.xaml
    /// </summary>
    public partial class SpriteAnimator : Window
    {
        ContentManager contentMgr;
        Texture2D DevTexture;

        public SpriteAnimator()
        {
            InitializeComponent();
        }

        public void LoadContent(object sender, LoadContentArgs e)
        {
            contentMgr = new ContentManager(e.Services, "Content");
            DevTexture = contentMgr.Load<Texture2D>("HealCross.png");
        }

        SpriteBatch spritebatch;
        bool hasInstantiatedSpriteBatch = false;
        private void RenderBox(object sender, GraphicsDeviceEventArgs e)
        {
            e.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
            if (!hasInstantiatedSpriteBatch)
            {
                spritebatch = new SpriteBatch(e.GraphicsDevice);
                hasInstantiatedSpriteBatch = true;
            }

            spritebatch.Begin();

            Draw(spritebatch);

            spritebatch.End();

        }

        private void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(DevTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, 1000, 1000), Microsoft.Xna.Framework.Color.White);
        }
    }
}
