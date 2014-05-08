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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevTools.GraphicsControls.Boiler;
using DevTools.GraphicsControls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ContentCompiler.ContentCompilation;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ContentBuilder builder;
        ContentManager contentMgr;

        public MainWindow()
        {
            InitializeComponent();
            
            builder = new ContentBuilder();

        }

        public void BuildAllContent()
        {
            builder.Clear();
            builder.Add(@"C:\dev\coop\DevTools\Chains.png", "Chains", "TextureImporter", "TextureProcessor");
            string errors = builder.Build();
        }

        private void LoadContent(object sender, LoadContentArgs e)
        {
            //could bring in the content manager stuff,
            //would look like:
            //ContentManager Content = new ContentManager(this.graphicsTest.graphicsService);
            //Content.Load<Texture2D>("stuff"); etc
            //ContentManager Content = new ContentManager();
            BuildAllContent();
        }

        private SpriteBatch spriteBatch;
        private void graphicsTest_RenderXna_1(object sender, GraphicsDeviceEventArgs e)
        {
            //HERE IS WHERE THE MAGIC HAS FINALLY COME TOGETHER>>>.......F DSLKIFJMH KSLDUHFMLIUDHF
            e.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
        }
    }
}
