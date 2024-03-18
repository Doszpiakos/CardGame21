using System.Windows;
using System.Windows.Controls;
using CardGame21.ViewModel;

namespace CardGame21.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel main = new MainWindowViewModel();
            main.main = this;
            this.DataContext = main;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Text = "";
        }
    }
}