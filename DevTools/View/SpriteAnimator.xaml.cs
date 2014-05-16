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
using DevTools.ViewModel;

namespace DevTools.View
{
    /// <summary>
    /// Interaction logic for SpriteAnimator.xaml
    /// </summary>
    public partial class SpriteAnimator : Window
    {
        SpriteAnimatorViewModel VM
        {
            get { return this.DataContext as SpriteAnimatorViewModel; }
            set { this.DataContext = value; }
        }
        SpriteBatch spriteBatch;

        public SpriteAnimator()
        {
            InitializeComponent();
            VM = new SpriteAnimatorViewModel(graphicsTest);
        }

        public void LoadContent(object sender, LoadContentArgs e)
        {
            VM.LoadContent(new ContentManager(e.Services, "Content"), e.GraphicsDevice);
            spriteBatch = new SpriteBatch(e.GraphicsDevice);
        }

        private void RenderBox(object sender, GraphicsDeviceEventArgs e)
        {
            e.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            spriteBatch.Begin();
            VM.Draw(spriteBatch);

            if (PhysBoxCheckBox.IsChecked.Value)
            {
                VM.DrawPhysics(spriteBatch);
            }
            if (DamageDotsCheckBox.IsChecked.Value)
            {
                VM.DrawDamageDots(spriteBatch);
            }
            if (SelectionCheckBox.IsChecked.Value)
            {
                VM.DrawSelection(spriteBatch);
            }

            spriteBatch.End();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //open
            System.Windows.Forms.OpenFileDialog opener = new System.Windows.Forms.OpenFileDialog();

            opener.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            opener.Filter = "Image files (*.jpg, *.bmp, *.png) | *.jpg; *.bmp; *.png";//"Content files| *.jpg; *.bmp; *.png | All Files (*.*) | *.*";

            if (opener.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                VM.OpenFilePair(opener.FileName);
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VM.RefreshCurrentContent();
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            VM.Pause();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            VM.Play();
        }

        private double hackyScale = .66;
        private bool isDragging = false;
        private Point downPosition;
        private void graphicsTest_HwndLButtonDown_1(object sender, HwndMouseEventArgs e)
        {
            downPosition = e.Position;
            isDragging = true;
        }

        private void graphicsTest_HwndLButtonUp_1(object sender, HwndMouseEventArgs e)
        {
            if (isDragging)
            {
                VM.CurrentSelection = new Microsoft.Xna.Framework.Rectangle(
                    (int)(downPosition.X * hackyScale),
                    (int)(downPosition.Y * hackyScale),
                    (int)((-downPosition.X + e.Position.X) * hackyScale),
                    (int)((-downPosition.Y + e.Position.Y) * hackyScale)
                );
            }
            isDragging = false;
        }

        private void graphicsTest_HwndMouseMove_1(object sender, HwndMouseEventArgs e)
        {
            if (isDragging)
            {
                VM.CurrentSelection = new Microsoft.Xna.Framework.Rectangle(
                    (int)(downPosition.X * hackyScale),
                    (int)(downPosition.Y * hackyScale),
                    (int)((-downPosition.X + e.Position.X) * hackyScale),
                    (int)((-downPosition.Y + e.Position.Y) * hackyScale)
                );
            }
        }
    }
}
