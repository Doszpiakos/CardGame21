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
        NewGameWindow newGame;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Options.Players = new ObservableCollection<Player>();
            Left = Options.Left;
            Top = Options.Top;
            Height = Options.Height;
            Width = Options.Width;
            PlayerInput = "NewPlayer";
            NewGameCommand = new RelayCommand(() => 
            {
                if (PlayersList.Count > 0)
                {
                    newGameViewModel = new NewGameViewModel(main);
                    newGame = new NewGameWindow(newGameViewModel);
                    newGame.Show();
                    main.Hide();
                }
                else
                    MessageBox.Show("No players!");
            });
            AddPlayerCommand = new RelayCommand(() =>
            {
                int i = 0;
                while (i < Options.Players.Count && Options.Players[i].Name != PlayerInput)
                    i++;
                if (i == Options.Players.Count)
                    Options.Players.Add(new Player(PlayerInput));
                else
                    MessageBox.Show("Name is taken!");
                PlayerInput = "NewPlayer";
            });

            ExitCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            });

        }
    }
}
