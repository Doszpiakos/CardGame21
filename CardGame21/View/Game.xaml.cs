using CardGame21.ViewModel;
using System.Windows;

namespace CardGame21.View
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public Game(GameViewModel gameViewModel)
        {
            InitializeComponent();
            this.DataContext = gameViewModel;
            gameViewModel.Window = this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
