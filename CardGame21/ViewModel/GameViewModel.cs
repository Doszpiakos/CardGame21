using CardGame21.Logic;
using CardGame21.Model;
using CardGame21.View;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;

namespace CardGame21.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public Game Window;
        CardLogic cardLogic;
        NewGameWindow previousWindow;

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

        // Chooses next player, starts dealers turn
        void NextPlayer()
        {
            if (CurrentPlayer != null)
            {
                CurrentPlayer.Color = "CornflowerBlue";
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
                }
                else
                {
                    CurrentPlayer = null;
                    DealerTurn();
                }
            }
        }

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
                            MessageBox.Show(player.Name + " has won!");
                            Info.Add(new Info(player.Name + " has won!", Model.Info.MessageColors.Won));
                        }
                        else
                        {
                            player.CheckedStatus = true;
                            MessageBox.Show(player.Name + " has busted!");
                            Info.Add(new Info(player.Name + " has busted!", Model.Info.MessageColors.Bust));
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
                    MessageBox.Show(CurrentPlayer.Name + " has busted!");
                    Info.Add(new Info(CurrentPlayer.Name + " has busted!", Model.Info.MessageColors.Bust));
                    NextPlayer();
                }
                else if (CurrentPlayer.Total == 21)
                {
                    //WIN
                    MessageBox.Show(CurrentPlayer.Name + " has won!");
                    Info.Add(new Info(CurrentPlayer.Name + " has won!", Model.Info.MessageColors.Won));
                    CurrentPlayer.Won = true;
                    CurrentPlayer.CheckedStatus = true;
                    NextPlayer();
                }
            }
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
                player.Cards = new ObservableCollection<Card>();
                player.Total = 0;
            }
            Window.Hide();

            // Window position
            previousWindow.Left = Options.Left;
            previousWindow.Top = Options.Top;
            previousWindow.Height = Options.Height;
            previousWindow.Width = Options.Width;

            previousWindow.ReCalc();
            previousWindow.Show();
        }

        public GameViewModel(NewGameWindow previousWindow)
        {
            this.previousWindow = previousWindow;
            this.cardLogic = Options.CardLogic;
            Info = new ObservableCollection<Info>();
            Players = Options.Players;

            // Hit button
            HitCommand = new RelayCommand(() =>
            {
                if (CurrentPlayer.Total < 21)
                {
                    Card card = cardLogic.DrawCard(true);
                    CurrentPlayer.AddACard(card);
                    Info.Add(new Info(CurrentPlayer.Name + " choose hit, got " + card.Name, Model.Info.MessageColors.Hit));
                }
                CheckPlayer();
            });
            // Stand button
            StandCommand = new RelayCommand(() =>
            {
                Info.Add(new Info(CurrentPlayer.Name + " choose stand", Model.Info.MessageColors.Stand));
                CurrentPlayer.Color = "CornflowerBlue";
                NextPlayer();
            });

            dealers = new ObservableCollection<Dealer>();
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
                Dealers[0].AddACard(cardLogic.DrawCard(true));
                AllowUIToUpdate();
            }

            // Dealer bust
            if (Dealers[0].Total > 21)
            {
                MessageBox.Show("Dealer busted!");
                Dealers[0].Total = 0;
            }
            Dealers[0].DealOver = true;
            CheckPlayer();
            GameEnd();
        }

        public void FirstDraw()
        {
            CurrentPlayer = Options.Players[0];
            AllowUIToUpdate();
            Dealers.Add(new Dealer(cardLogic.Cards));
            AllowUIToUpdate();

            foreach (var player in Options.Players)
            {
                player.Won = false;
                AllowUIToUpdate();
                player.AddACard(cardLogic.DrawCard(true));
                AllowUIToUpdate();
                CurrentPlayer.Calc();
            }

            Dealers[0].AddACard(cardLogic.DrawCard(true));

            foreach (var player in Options.Players)
            {
                AllowUIToUpdate();
                player.AddACard(cardLogic.DrawCard(true));
                AllowUIToUpdate();
                CurrentPlayer.Calc();
            }

            Dealers[0].AddACard(cardLogic.DrawCard(false));
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

            int i = 0;
            while (i < Options.Players.Count && Options.Players[i].Won)
            {
                i++;
            }

            if (i < Options.Players.Count)
            {
                CurrentPlayer = Options.Players[i];
                Info.Add(new Info(CurrentPlayer.Name + "'s turn", Model.Info.MessageColors.Turn));
                CurrentPlayer.Color = "Navy";
            }
            else
                GameEnd();
        }
    }
}
