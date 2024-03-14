using CardGame21.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
