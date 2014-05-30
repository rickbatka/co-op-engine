extern alias xnaFrameworkAlias;
extern alias xnaGraphicsAlias;
extern alias monoFrameworkAlias;
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
using DevTools.ViewModel;
using xnaFrameworkAlias.Microsoft.Xna.Framework.Content;
using xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics;

namespace DevTools.View
{
    /// <summary>
    /// Interaction logic for ParticlePlayground.xaml
    /// </summary>
    public partial class ParticlePlayground : Window
    {
        ParticlePlaygroundViewModel VM
        {
            get { return this.DataContext as ParticlePlaygroundViewModel; }
            set { this.DataContext = value; }
        }

        SpriteBatch spriteBatch;
        monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.SpriteBatch monoSpriteBatch;

        public ParticlePlayground()
        {
            InitializeComponent();

            VM = new ParticlePlaygroundViewModel();

            //this.Loaded += FinishedLoading;
        }

        public void RenderParticleEngine(object sender, GraphicsDeviceEventArgs e)
        {
            //todo testing 
            e.GraphicsDevice.Clear(xnaFrameworkAlias.Microsoft.Xna.Framework.Color.White);

            VM.UpdateAndDraw();
        }

        public void LoadContent(object sender, LoadContentArgs e)
        {
            VM.LoadContent(new ContentManager(e.Services, "Content"), e.GraphicsDevice);
            VM.monoDevice = e.MonoDevice;
            spriteBatch = new SpriteBatch(e.GraphicsDevice);
            monoSpriteBatch = new monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.SpriteBatch(e.MonoDevice);

            var tex = new monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.Texture2D(e.MonoDevice, 1, 1);
            tex.SetData<monoFrameworkAlias.Microsoft.Xna.Framework.Color>(new monoFrameworkAlias.Microsoft.Xna.Framework.Color[] { monoFrameworkAlias.Microsoft.Xna.Framework.Color.White });
            VM.spriteBatch = monoSpriteBatch;
            VM.PlainTexture = tex;
        }
    }
}
