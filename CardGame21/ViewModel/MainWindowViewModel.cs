using CardGame21.Logic;
using CardGame21.Model;
using CardGame21.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardGame21.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand NewGameCommand { get; set; }
        public ICommand AddPlayerCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MainWindow main;

        string playerInput;
        public string PlayerInput
        {
            get { return playerInput; }
            set
            {
                playerInput = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayerInput"));
            }
        }

        public ObservableCollection<Player> PlayersList
        {
            get => Options.Players;
            set
            {
                Options.Players = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PayersList"));
            }
        }

        NewGameViewModel newGameViewModel;
        GameLogic gameLogic;
        NewGameWindow newGame;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Options.Players = new ObservableCollection<Player>();
            PlayerInput = "NewPlayer";
            NewGameCommand = new RelayCommand(() => 
            {
                newGameViewModel = new NewGameViewModel(main);
                newGame = new NewGameWindow(newGameViewModel);
                newGame.Show();
                main.Hide();
            });
            AddPlayerCommand = new RelayCommand(() =>
            {
                Options.Players.Add(new Player(PlayerInput));
            });

            ExitCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            });

        }
    }
}
