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
using System.Windows;
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
        Player currentPlayer;
        public Player CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPlayer"));
            }
        }

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

        ObservableCollection<Player> playersList;

        public ObservableCollection<Player> PlayersList
        {
            get => playersList;
            set
            {
                playersList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PayersList"));
            }
        }

        int bet = 2;
        public int Bet
        {
            get
            {
                return bet;
            }
            set
            {
                if (CurrentPlayer != null)
                {
                    if (value > CurrentPlayer.Money)
                        bet = CurrentPlayer.Money;
                    if (value < 1)
                        bet = 1;
                    else if (value > 500)
                        bet = 500;
                    else
                        bet = value;
                }
                else
                    bet = 0;
                CurrentPlayer.Bet = bet;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bet"));
            }
        }
        int counter = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public NewGameWindow Window;

        public ICommand StartGameCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public ICommand BetCommand { get; set; }

        public void Reset()
        {
            counter = 0;
            int i = 0;
            while (i < Options.Players.Count && Options.Players[i].Money < 1)
            {
                counter++;
                i++;
            }
            if (i < Options.Players.Count)
                CurrentPlayer = Options.Players[i];
        }
        public NewGameViewModel(MainWindow main)
        {
            PlayersList = Options.Players;
            CurrentPlayer = Options.Players[counter];

            StartGameCommand = new RelayCommand(() =>
            {
                if (CurrentPlayer == null)
                {
                    if (Options.CardLogic == null)
                        Options.CardLogic = new CardLogic(numOfDecks);
                    GameViewModel gameViewModel = new GameViewModel(Window);
                    Game game = new Game(gameViewModel);
                    game.Show();
                    Window.Hide();
                    counter = 0;
                    gameViewModel.FirstDraw();
                }
                else
                    MessageBox.Show("Place your bets!");
            });
            BetCommand = new RelayCommand(() =>
            {
                if (counter < Options.Players.Count)
                {
                    CurrentPlayer.Bet = bet;
                    CurrentPlayer.Money = CurrentPlayer.Money - bet;
                    counter++;
                }
                if (counter < Options.Players.Count)
                    CurrentPlayer = Options.Players[counter];
                else
                    CurrentPlayer = null;
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
