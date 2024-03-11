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
        int counter = 0;

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

        void NextPlayer()
        {
            int i = counter;
            while (i < playersList.Count && playersList[i].Total >= 21)
            {
                i++;
            }
            if (i < playersList.Count)
            {
                CurrentPlayer.Color = "CornflowerBlue";
                CurrentPlayer = playersList[i];
                counter = i;
                Info.Add(new Info(CurrentPlayer.Name + "'s turn", Model.Info.MessageColors.Turn));
                CurrentPlayer.Color = "Navy";
            }
            else
                CurrentPlayer = null;
        }

        void CheckOver()
        {
            int i = 0;
            while (i < playersList.Count && playersList[i].Total >= 21)
                i++;
            if (i < playersList.Count)
                DealerTurn();
            else
                GameEnd();
        }

        void GameEnd()
        {
            foreach (var player in playersList)
            {
                if (player.Won)
                    player.Money += player.Bet * 2;
            }

            MessageBox.Show("GAME_ENDED");
            ClearPlayers();
            Window.Hide();
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

            HitCommand = new RelayCommand(() =>
            {
                if (playersList[counter].Total < 21)
                {
                    Card card = cardLogic.DrawCard(true);
                    playersList[counter].AddACard(card);
                    Info.Add(new Info(CurrentPlayer.Name + " choose hit, got " + card.Name, Model.Info.MessageColors.Hit));

                }
                if (playersList[counter].Total > 21)
                {
                    //LOSE
                    MessageBox.Show(CurrentPlayer.Name + " has busted!");
                    Info.Add(new Info(CurrentPlayer.Name + " has busted!", Model.Info.MessageColors.Bust));
                    counter++;
                    if (counter < playersList.Count)
                    {
                        CurrentPlayer = playersList[counter];
                        Info.Add(new Info(CurrentPlayer.Name + "'s turn!", Model.Info.MessageColors.Turn));
                    }
                    else
                        CheckOver();
                }
                else if (playersList[counter].Total == 21)
                {
                    //WIN
                    MessageBox.Show(CurrentPlayer.Name + " has won!");
                    Info.Add(new Info(CurrentPlayer.Name + " has won!", Model.Info.MessageColors.Won));
                    CurrentPlayer.Won = true;
                    counter++;
                    if (counter < playersList.Count)
                        CurrentPlayer = playersList[counter];
                    else
                        CheckOver();
                }
            });

            StandCommand = new RelayCommand(() =>
            {
                Info.Add(new Info(CurrentPlayer.Name + " choose stand", Model.Info.MessageColors.Stand));
                CurrentPlayer.Color = "CornflowerBlue";
                counter++;
                NextPlayer();
                if (CurrentPlayer == null)
                {
                    DealerTurn();
                }
            });

            playersList = Options.Players;
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
                foreach (var player in playersList)
                {
                    if (!player.Won && player.Total < 22)
                    {
                        player.Won = true;
                        MessageBox.Show(player.Name + " has won!");
                    }
                }
            }
            else
            {
                foreach (var player in playersList)
                {
                    if (Dealers[0].Total < player.Total && player.Total < 22)
                    {
                        if (!player.Won)
                        {
                            player.Won = true;
                            MessageBox.Show(player.Name + " has won!");
                        }
                    }
                    else if (Dealers[0].Total >= player.Total && player.Total < 22)
                        MessageBox.Show(player.Name + " has lost!");
                }
            }
            GameEnd();
        }

        void ClearPlayers()
        {
            foreach (var player in Options.Players)
            {
                player.Cards = new ObservableCollection<Card>();
            }
        }

        public void FirstDraw()
        {
            CurrentPlayer = playersList[0];
            AllowUIToUpdate();
            Dealers.Add(new Dealer(cardLogic.Cards));
            AllowUIToUpdate();

            foreach (var player in playersList)
            {
                player.Won = false;
                AllowUIToUpdate();
                player.AddACard(cardLogic.DrawCard(true));
                AllowUIToUpdate();
                playersList[counter].Calc();
            }

            Dealers[0].AddACard(cardLogic.DrawCard(true));

            foreach (var player in playersList)
            {
                AllowUIToUpdate();
                player.AddACard(cardLogic.DrawCard(true));
                AllowUIToUpdate();
                playersList[counter].Calc();
            }

            Dealers[0].AddACard(cardLogic.DrawCard(false));
            AllowUIToUpdate();

            foreach (var player in playersList)
            {
                if (player.Total == 21)
                {
                    CurrentPlayer.Won = true;
                    MessageBox.Show(player.Name + " has won!");
                    NextPlayer();
                }
                else
                {
                    player.Calc();
                }
            }

            if (CurrentPlayer == null)
                GameEnd();
            else
            {
                Info.Add(new Info(CurrentPlayer.Name + "'s turn", Model.Info.MessageColors.Turn));
                CurrentPlayer.Color = "Navy";
            }
        }
    }
}
