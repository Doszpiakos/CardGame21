using CardGame21.Model;
using CardGame21.View;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CardGame21.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Variables/Properties

        // Window left side
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

        // Window top side
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

        // Window height
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

        // Window width
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

        // Enables start button for UI
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

        // Player name input from UI
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

        NewGameViewModel newGameViewModel;
        NewGameWindow newGame;

        public ICommand NewGameCommand { get; set; }
        public ICommand AddPlayerCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MainWindow main;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public MainWindowViewModel()
        {
            Options.Players = new ObservableCollection<Player>(); // Creating players list

            // Setting window location info to Options
            Left = Options.Left;
            Top = Options.Top;
            Height = Options.Height;
            Width = Options.Width;
            PlayerInput = "NewPlayer";

            // Start game options menu
            NewGameCommand = new RelayCommand(() =>
            {
                if (Options.Players.Count > 0)
                {
                    newGameViewModel = new NewGameViewModel(main);
                    newGame = new NewGameWindow(newGameViewModel);
                    newGame.Show();
                    main.Hide();
                }
                else
                    MessageBox.Show("No players!");
            });

            // Adds new player to players list
            AddPlayerCommand = new RelayCommand(() =>
            {
                // Checks if player has a name
                if (PlayerInput != "")
                {
                    int i = 0;
                    while (i < Options.Players.Count && Options.Players[i].Name != PlayerInput)
                        i++;
                    if (i == Options.Players.Count)
                        Options.Players.Add(new Player(PlayerInput));
                    else
                        MessageBox.Show("Name is taken!");
                    PlayerInput = "NewPlayer";
                    StartEnabled = true;
                }
            });

            // Exit command for closing application
            ExitCommand = new RelayCommand(() =>
            {
                System.Windows.Application.Current.Shutdown();
            });

        }
    }
}
