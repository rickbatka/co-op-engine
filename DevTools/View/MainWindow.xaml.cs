extern alias xnaFrameworkAlias;
extern alias xnaGraphicsAlias;
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
using ContentCompiler.ContentCompilation;
using DevTools.View;
using xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics;
using xnaFrameworkAlias.Microsoft.Xna.Framework.Content;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void MenuHelpAboutClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version:" + "<pending versioning feature>");
        }

        public void MenuFileExitClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ButtonSpriteAnimatorPressed(object sender, RoutedEventArgs e)
        {
            SpriteAnimator spriteAnimator = new SpriteAnimator();
            spriteAnimator.Show();
        }

        public void ButtonContentCompilerPressed(object sender, RoutedEventArgs e)
        {
            Builder builder = new Builder();
            builder.Show();
        }
    }
}
