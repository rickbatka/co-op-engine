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
        private double windowsScalingOffset = 1.0;
        private bool isDragging = false;
        private Point downPosition;

        public SpriteAnimator()
        {
            InitializeComponent();
            DebugEventView.Visibility = System.Windows.Visibility.Collapsed;

            VM = new SpriteAnimatorViewModel();

            this.Loaded += FinishedLoading;
        }

        void FinishedLoading(object sender, RoutedEventArgs e)
        {
            //windows scaling issues...
            Matrix screenMatrix = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            double dx = screenMatrix.M11;
            double dy = screenMatrix.M22;

            if (dx != dy)
            {
                throw new Exception("Run environment not conducive for graphics");
            }
            else
            {
                windowsScalingOffset = dx;
            }
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

        private void GraphicsBoxMouseLeftDown(object sender, HwndMouseEventArgs e)
        {
            downPosition = e.Position;
            isDragging = true;
        }

        private void GraphicsBoxMouseLeftUp(object sender, HwndMouseEventArgs e)
        {
            if (isDragging)
            {
                VM.CurrentSelection = new Microsoft.Xna.Framework.Rectangle(
                    (int)(downPosition.X * windowsScalingOffset),
                    (int)(downPosition.Y * windowsScalingOffset),
                    (int)((-downPosition.X + e.Position.X) * windowsScalingOffset),
                    (int)((-downPosition.Y + e.Position.Y) * windowsScalingOffset)
                );
            }
            isDragging = false;
        }

        private void GraphicsBoxMouseMove(object sender, HwndMouseEventArgs e)
        {
            if (isDragging)
            {
                VM.CurrentSelection = new Microsoft.Xna.Framework.Rectangle(
                    (int)(downPosition.X * windowsScalingOffset),
                    (int)(downPosition.Y * windowsScalingOffset),
                    (int)((-downPosition.X + e.Position.X) * windowsScalingOffset),
                    (int)((-downPosition.Y + e.Position.Y) * windowsScalingOffset)
                );
            }
        }

        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxSelectionKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);
            expression.UpdateSource();
        }
    }
}
