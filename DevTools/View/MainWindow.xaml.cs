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
using DevTools.ViewModel;

namespace DevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConsoleViewModel VM
        {
            get { return (ConsoleViewModel)this.DataContext; }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.ConsoleViewModel();
        }

        public void ConnectClient(object sender, RoutedEventArgs args)
        {
            VM.Connect();
        }

        public void CloseClient(object sender, RoutedEventArgs e)
        {
            VM.Disconnect();
        }

        public void StartServer(object sender, RoutedEventArgs e)
        {
            VM.Host();
        }

        public void StopServer(object sender, RoutedEventArgs e)
        {
            VM.StopHost();
        }

        public void CheckForConfirmInInput(object sender, RoutedEventArgs e)
        {
            var input = (KeyEventArgs)e;
            var textbox = (TextBox)sender;

            if (input.Key == Key.Enter)
            {
                VM.ExecuteCommand(textbox.Text);
                textbox.Text = "";
            }
        }
    }
}
