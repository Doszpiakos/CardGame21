using CardGame21.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CardGame21.View
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        NewGameViewModel newGameViewModel;
        public NewGameWindow(NewGameViewModel newGameViewModel)
        {
            InitializeComponent();
            this.newGameViewModel = newGameViewModel;
            this.DataContext = this.newGameViewModel;
            newGameViewModel.Window = this;
        }

        public void ReCalc()
        {
            newGameViewModel.Reset();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
                (sender as TextBox).Text = "1";
        }
    }
}
