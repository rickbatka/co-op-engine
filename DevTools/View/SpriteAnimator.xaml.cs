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
using System.Windows.Shapes;
using DevTools.GraphicsControls;
using DevTools.GraphicsControls.Boiler;
using DevTools.ViewModel;
using xnaFrameworkAlias.Microsoft.Xna.Framework.Content;
using xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics;

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
        xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;
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
            spriteBatch = new xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics.SpriteBatch(e.GraphicsDevice);
        }

        private void RenderBox(object sender, GraphicsDeviceEventArgs e)
        {
            if (!VM.FileIsOutOfDate)
            {

                e.GraphicsDevice.Clear(xnaFrameworkAlias.Microsoft.Xna.Framework.Color.CornflowerBlue);

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
            else
            {
                VM.RefreshCurrentContent.Execute(null);
                VM.FileIsOutOfDate = false;
            }
        }

        private void GraphicsBoxMouseLeftDown(object sender, HwndMouseEventArgs e)
        {
            if (VM.GridSize > 1)
            {
                downPosition = RoundToNearestGridPoint(e.Position);
            }
            else
            {
                downPosition = e.Position;
            }

            isDragging = true;
        }

        private void GraphicsBoxMouseLeftUp(object sender, HwndMouseEventArgs e)
        {
            Point position = e.Position;

            if (VM.GridSize > 1)
            {
                position = RoundToNearestGridPoint(e.Position);
            }

            if (isDragging)
            {
                VM.CurrentSelection = new xnaFrameworkAlias.Microsoft.Xna.Framework.Rectangle(
                    (int)(downPosition.X * windowsScalingOffset),
                    (int)(downPosition.Y * windowsScalingOffset),
                    (int)((-downPosition.X + position.X) * windowsScalingOffset),
                    (int)((-downPosition.Y + position.Y) * windowsScalingOffset)
                );
            }
            isDragging = false;
        }

        private void GraphicsBoxMouseMove(object sender, HwndMouseEventArgs e)
        {
            Point position = e.Position;

            if (VM.GridSize > 1)
            {
                position = RoundToNearestGridPoint(e.Position);
            }

            if (isDragging)
            {
                VM.CurrentSelection = new xnaFrameworkAlias.Microsoft.Xna.Framework.Rectangle(
                    (int)(downPosition.X * windowsScalingOffset),
                    (int)(downPosition.Y * windowsScalingOffset),
                    (int)((-downPosition.X + position.X) * windowsScalingOffset),
                    (int)((-downPosition.Y + position.Y) * windowsScalingOffset)
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

        private void TextBoxGridSizeKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);
            expression.UpdateSource();
        }

        private Point RoundToNearestGridPoint(Point input)
        {
            int grid = VM.GridSize;
            
            double x = Math.Round(input.X / grid) * grid;
            double y = Math.Round(input.Y / grid) * grid;

            return new Point(x, y);
        }
    }
}
