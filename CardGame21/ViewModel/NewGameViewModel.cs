using CardGame21.Logic;
using CardGame21.Model;
using CardGame21.View;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CardGame21.ViewModel
{
    public class NewGameViewModel : INotifyPropertyChanged
    {
        public static double Left
        {
            get
            {
                return Options.Left;
            }
            set
            {
                Options.Left = value;
            }
        }
        public static double Top
        {
            get
            {
                return Options.Top;
            }
            set
            {
                Options.Top = value;
            }
        }
        public static double Height
        {
            get
            {
                return Options.Height;
            }
            set
            {
                Options.Height = value;
            }
        }
        public static double Width
        {
            get
            {
                return Options.Width;
            }
            set
            {
                Options.Width = value;
            }
        }

        ObservableCollection<Player> players = new ObservableCollection<Player>();

        int numOfDecks = 1;
        public int NumOfDecks
        {
            get
            {
                return numOfDecks;
            }
            set
            {
                numOfDecks = value;
                Options.NumOfDecks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumOfDecks"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NewGameWindow Window;

        public ICommand StartGameCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public NewGameViewModel(MainWindow main)
        {
            StartGameCommand = new RelayCommand(() =>
            {
                if (Options.CardLogic == null)
                    Options.CardLogic = new CardLogic(numOfDecks);
                GameViewModel gameViewModel = new GameViewModel(Window);
                Game game = new Game(gameViewModel);
                game.Show();
                Window.Hide();
                gameViewModel.FirstDraw();
            });

            PlusCommand = new RelayCommand(() =>
            {
                if (NumOfDecks < 10)
                    NumOfDecks++;
                Options.CardLogic = null;
            });

            MinusCommand = new RelayCommand(() =>
            {
                if (NumOfDecks > 1)
                    NumOfDecks--;
                Options.CardLogic = null;
            });

            BackCommand = new RelayCommand(() =>
            {
                main.Left = Options.Left;
                main.Top = Options.Top;
                main.Height = Options.Height;
                main.Width = Options.Width;
                main.Show();
                Window.Hide();
            });
        }
    }
}
