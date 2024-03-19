using CardGame21.Model;
using CardGame21.View;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CardGame21.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Variables/Properties

        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Game Window;
        NewGameWindow previousWindow;

        bool hitEnabled;
        public bool HitEnabled
        {
            get
            {
                return hitEnabled;
            }
            set
            {
                hitEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HitEnabled"));
            }
        }

        bool standEnabled;
        public bool StandEnabled
        {
            get
            {
                return standEnabled;
            }
            set
            {
                standEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StandEnabled"));
            }
        }

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

        ObservableCollection<Info> info;
        public ObservableCollection<Info> Info
        {
            get => info;
            set
            {
                info = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Info));
            }
        }

        ObservableCollection<Player> players;
        public ObservableCollection<Player> Players
        {
            get => players;
            set
            {
                players = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Players"));
            }
        }

        ObservableCollection<Dealer> dealers;
        public ObservableCollection<Dealer> Dealers
        {
            get => dealers;
            set
            {
                dealers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dealers"));
            }
        }

        #endregion

        public GameViewModel(NewGameWindow previousWindow)
        {
            this.previousWindow = previousWindow;
            Players = Options.Players;
            Info = new ObservableCollection<Info>();

            // Hit button
            HitCommand = new RelayCommand(() =>
            {
                if (CurrentPlayer.Total < 21)
                {
                    Card card = Options.CardLogic.DrawCard(true);
                    CurrentPlayer.AddACard(card);
                    Info.Add(new Info(CurrentPlayer.Name + " choose hit, got " + card.Name, Model.Info.MessageColors.Hit));
                }
                HitEnabled = false;
                StandEnabled = false;
                CheckPlayer();
            });
            // Stand button
            StandCommand = new RelayCommand(() =>
            {
                Info.Add(new Info(CurrentPlayer.Name + " choose stand", Model.Info.MessageColors.Stand));
                CurrentPlayer.Color = "CornflowerBlue";
                HitEnabled = false;
                StandEnabled = false;
                NextPlayer();
            });

            dealers = new ObservableCollection<Dealer>();
        }

        // Chooses next player, starts dealers turn
        void NextPlayer()
        {
            if (CurrentPlayer != null)
            {
                int i = 0;
                while (i < Options.Players.Count && CurrentPlayer != Options.Players[i])
                    i++;
                i++;
                if (i < Options.Players.Count)
                {
                    while (i < Options.Players.Count && Options.Players[i].Won)
                        i++;
                    CurrentPlayer = Options.Players[i];
                    Info.Add(new Info(CurrentPlayer.Name + "'s turn", Model.Info.MessageColors.Turn));
                    CurrentPlayer.Color = "Navy";
                    HitEnabled = true;
                    StandEnabled = true;
                }
                else
                {
                    CurrentPlayer = null;
                    int j = 0;
                    while (j < Options.Players.Count && Options.Players[j].CheckedStatus == true)
                    {
                        j++;
                    }
                    if (j < Options.Players.Count)
                        DealerTurn();
                    else
                        GameEnd();
                }
            }
        }

        // Checks players won/lost status
        void CheckPlayer()
        {
            if (Dealers[0].DealOver)
            {
                int total = 0;

                if (Dealers[0].Total < 22)
                    total = Dealers[0].Total;

                foreach (var player in Options.Players)
                {
                    if (!player.CheckedStatus)
                    {
                        if (player.Total < 22 && player.Total > total)
                        {
                            player.Won = true;
                            player.CheckedStatus = true;
                            Info.Add(new Info(player.Name + " has won!", Model.Info.MessageColors.Won));
                            MessageBox.Show(player.Name + " has won!");
                        }
                        else
                        {
                            player.CheckedStatus = true;
                            Info.Add(new Info(player.Name + " has busted!", Model.Info.MessageColors.Bust));
                            MessageBox.Show(player.Name + " has busted!");
                        }
                    }
                }
            }
            else
            {
                if (CurrentPlayer.Total > 21)
                {
                    //LOSE
                    CurrentPlayer.CheckedStatus = true;
                    Info.Add(new Info(CurrentPlayer.Name + " has busted!", Model.Info.MessageColors.Bust));
                    MessageBox.Show(CurrentPlayer.Name + " has busted!");
                    NextPlayer();
                }
                else if (CurrentPlayer.Total == 21)
                {
                    //WIN
                    Info.Add(new Info(CurrentPlayer.Name + " has won!", Model.Info.MessageColors.Won));
                    MessageBox.Show(CurrentPlayer.Name + " has won!");
                    CurrentPlayer.Won = true;
                    CurrentPlayer.CheckedStatus = true;
                    NextPlayer();
                }
                else
                {
                    HitEnabled = true;
                    StandEnabled = true;
                }
            }

            AllowUIToUpdate();
        }

        // Game over, resets players
        void GameEnd()
        {
            MessageBox.Show("GAME_ENDED");

            // Reset players
            foreach (var player in Options.Players)
            {
                if (player.Won)
                    player.Money += player.Bet * 2;
                player.Won = false;
                player.CheckedStatus = false;
                player.Color = "CornflowerBlue";
                player.Total = 0;
            }
            Window.Hide();
            Info = new ObservableCollection<Info>();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Info"));
            Dealers.RemoveAt(0);

            // Window position
            previousWindow.Left = Options.Left;
            previousWindow.Top = Options.Top;
            previousWindow.Height = Options.Height;
            previousWindow.Width = Options.Width;

            previousWindow.ReCalc();
        }

        private static void AllowUIToUpdate()
        {
            DispatcherFrame frame = new();
            // DispatcherPriority set to Input, the highest priority
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                Thread.Sleep(100); // Stop all processes to make sure the UI update is perform
                return null;
            }), null);
            Dispatcher.PushFrame(frame);
            // DispatcherPriority set to Input, the highest priority
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Input, new Action(delegate { }));
        }

        void DealerTurn()
        {
            Info.Add(new Info("Dealers turn!", Model.Info.MessageColors.Turn));
            Dealers[0].RevealCard();

            AllowUIToUpdate();

            while (Dealers[0].Total < 17)
            {
                Dealers[0].AddACard(Options.CardLogic.DrawCard(true));
                AllowUIToUpdate();
            }

            // Dealer bust
            if (Dealers[0].Total > 21)
            {
                Info.Add(new Info("Dealer busted!", Model.Info.MessageColors.Bust));
                MessageBox.Show("Dealer busted!");
                Dealers[0].Total = 0;
            }
            Dealers[0].DealOver = true;
            CheckPlayer();
            GameEnd();
        }

        public void FirstDraw()
        {
            Dealers.Add(new Dealer(Options.CardLogic.Cards));

            HitEnabled = false;
            StandEnabled = false;

            CurrentPlayer = Options.Players[0];
            AllowUIToUpdate();

            foreach (var player in Options.Players)
            {
                player.Won = false;
                AllowUIToUpdate();
                player.AddACard(Options.CardLogic.DrawCard(true));
                AllowUIToUpdate();
                player.Calc();
            }

            Dealers[0].AddACard(Options.CardLogic.DrawCard(true));

            foreach (var player in Options.Players)
            {
                AllowUIToUpdate();
                player.AddACard(Options.CardLogic.DrawCard(true));
                AllowUIToUpdate();
                player.Calc();
            }

            Dealers[0].AddACard(Options.CardLogic.DrawCard(false));

            AllowUIToUpdate();

            foreach (var player in Options.Players)
            {
                if (player.Total == 21)
                {
                    player.Won = true;
                    player.CheckedStatus = true;
                    MessageBox.Show(player.Name + " has won!");
                }
                else
                {
                    player.Calc();
                }
            }

            int j = 0;
            while (j < Options.Players.Count && Options.Players[j].Won)
                j++;

            if (j < Options.Players.Count)
            {
                CurrentPlayer = Options.Players[j];
                Info.Add(new Info(CurrentPlayer.Name + "'s turn", Model.Info.MessageColors.Turn));
                CurrentPlayer.Color = "Navy";
                HitEnabled = true;
                StandEnabled = true;
            }
            else
                GameEnd();
        }
    }
}
