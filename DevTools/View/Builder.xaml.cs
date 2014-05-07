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
using DevTools.ViewModel;

namespace DevTools.View
{
    /// <summary>
    /// Interaction logic for Builder.xaml
    /// </summary>
    public partial class Builder : Window
    {
        private BuilderViewModel VM
        {
            get { return this.DataContext as BuilderViewModel; }
        }

        public Builder()
        {
            this.DataContext = new BuilderViewModel();
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VM.BuildAssets();
        }
    }
}
