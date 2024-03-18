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
        #region Variables/Properties

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

        bool betEnabled;
        public bool BetEnabled
        {
            get
            {
                return betEnabled;
            }
            set
            {
                betEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BetEnabled"));
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

        int bet = 1;
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
                    if (value > 500)
                        value = 500;
                    if (value > CurrentPlayer.Money)
                        bet = CurrentPlayer.Money;
                    else if (value < 1)
                        bet = 1;
                    else
                        bet = value;
                    CurrentPlayer.Bet = bet;
                    bet = 1;
                }
                else
                    bet = 1;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bet"));
            }
        }

        bool startEnabled;
        public bool StartEnabled
        {
            get
            {
                return startEnabled;
            }
            set
            {
                startEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartEnabled"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NewGameWindow Window;

        public ICommand StartGameCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public ICommand BetCommand { get; set; }

        #endregion

        public void Reset()
        {
            int i = 0;
            Bet = 1;

            while (i < Options.Players.Count)
            {
                if (Options.Players[i].Money == 0)
                {
                    MessageBox.Show(Options.Players[i].Name + " has no money left!");
                    Options.Players.RemoveAt(i);
                }
                i++;
            }

            if (Options.Players.Count > 0)
            {
                foreach (var player in Options.Players)
                {
                    player.Bet = 0;
                    for (int j = player.Cards.Count - 1; j > -1; j--)
                    {
                        player.Cards.RemoveAt(j);
                    }
                    player.Total = 0;
                }
                CurrentPlayer = Options.Players[0];
                CurrentPlayer.Color = "Navy";
                StartEnabled = false;
                BetEnabled = true;
                this.Window.Show();
            }
            else
            {
                MessageBox.Show("No more players left!");
                BackCommand.Execute(null);
            }

        }

        void NextPlayer()
        {
            // Find currentplayer in the list
            int i = 0;
            while (i < Options.Players.Count && Options.Players[i] != CurrentPlayer)
            {
                i++;
            }

            // Check if there is a next player
            i++;
            if (i < Options.Players.Count)
            {
                CurrentPlayer = Options.Players[i];
                CurrentPlayer.Color = "Navy";
                BetEnabled = true;
            }
            else
            {
                CurrentPlayer.Color = "CornflowerBlue";
                CurrentPlayer = null;
                StartEnabled = true;
                BetEnabled = false;
            }
        }

        public NewGameViewModel(MainWindow main)
        {
            PlayersList = Options.Players;
            CurrentPlayer = Options.Players[0];
            CurrentPlayer.Color = "Navy";
            BetEnabled = true;
            GameViewModel gameViewModel = null;
            Game game = null;

            StartGameCommand = new RelayCommand(() =>
            {
                if (Options.CardLogic == null)
                    Options.CardLogic = new CardLogic(numOfDecks);
                if (gameViewModel == null)
                    gameViewModel = new GameViewModel(Window);
                if (game == null)
                    game = new Game(gameViewModel);
                game.Show();
                Window.Hide();
                gameViewModel.FirstDraw();
            });
            BetCommand = new RelayCommand(() =>
            {
                CurrentPlayer.Bet = bet;
                CurrentPlayer.Money = CurrentPlayer.Money - bet;
                CurrentPlayer.Color = "CornflowerBlue";
                NextPlayer();
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
                foreach (var player in Options.Players)
                {
                    if (player.Bet != 0)
                    {
                        player.Money += player.Bet;
                    }
                    player.Bet = 0;
                }
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
