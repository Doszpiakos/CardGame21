﻿using CardGame21.Logic;
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

namespace CardGame21.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Game Window;

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

        int counter = 0;

        CardLogic cardLogic;
        GameLogic gameLogic;

        NewGameWindow previousWindow;

        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }

        void NextPlayer()
        {
            int i = 0;
            while (i < playersList.Count && playersList[i].Total != 21)
            {
                i++;
            }
            if (i < playersList.Count)
                CurrentPlayer = playersList[i];
            else
                CurrentPlayer = null;
        }

        void GameEnd()
        {
            MessageBox.Show("GAME_ENDED");
            Window.Hide();
            previousWindow.Show();
        }

        public GameViewModel(NewGameWindow previousWindow, CardLogic cardLogic)
        {
            this.previousWindow = previousWindow;
            this.cardLogic = cardLogic;

            HitCommand = new RelayCommand(() =>
            {
                if (playersList[counter].Total < 21)
                    playersList[counter].AddACard(cardLogic.DrawCard(true));

                if (playersList[counter].Total > 21)
                {
                    //LOSE
                    MessageBox.Show(CurrentPlayer.Name + " has busted!");
                    counter++;
                    if (counter < playersList.Count)
                        CurrentPlayer = playersList[counter];
                    else
                        GameEnd();
                }
                else if (playersList[counter].Total == 21)
                {
                    //WIN
                    MessageBox.Show(CurrentPlayer.Name + " has won!");
                    counter++;
                    if (counter < playersList.Count)
                        CurrentPlayer = playersList[counter];
                    else
                        GameEnd();
                }
            });

            StandCommand = new RelayCommand(() =>
            {
                if (counter < playersList.Count - 1)
                {
                    counter++;
                    CurrentPlayer = playersList[counter];
                }
                else
                {
                    Dealers[0].RevealCard();

                    while (Dealers[0].Total < 17)
                    {
                        Dealers[0].AddACard(cardLogic.DrawCard(true));
                    }

                    if (Dealers[0].Total > 21)
                    {
                        MessageBox.Show("Dealer busted!");
                    }
                    else
                    {
                        foreach (var player in playersList)
                        {
                            if (Dealers[0].Total < player.Total)
                                MessageBox.Show(player.Name + " has won!");
                            else
                                MessageBox.Show(player.Name + " has lost!");
                        }
                    }
                    GameEnd();
                }
            });

            playersList = new ObservableCollection<Player>();
            dealers = new ObservableCollection<Dealer>();
        }

        public void FirstDraw()
        {
            playersList.Add(new Player("John"));
            playersList.Add(new Player("TESTER"));

            Dealers.Add(new Dealer(cardLogic.Cards));

            CurrentPlayer = playersList[0];

            foreach (var player in playersList)
            {
                player.AddACard(cardLogic.DrawCard(true));
                playersList[counter].Calc();
            }

            Dealers[0].AddACard(cardLogic.DrawCard(true));
            
            foreach (var player in playersList)
            {
                player.AddACard(cardLogic.DrawCard(true));
                playersList[counter].Calc();
            }

            Dealers[0].AddACard(cardLogic.DrawCard(false));


            if (playersList[0].Total > 21)
                playersList[0].Calc();
            if (playersList[1].Total > 21)
                playersList[1].Calc();

            foreach (var player in playersList)
            {
                if (player.Total == 21)
                {
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

            /* DRAW CARD TESTING
            for (int i = 0; i < 44; i++)
            {
                cardLogic.DrawCard(true);
            }
            */
        }
    }
}