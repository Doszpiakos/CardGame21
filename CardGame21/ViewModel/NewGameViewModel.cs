using CardGame21.Logic;
using CardGame21.Model;
using CardGame21.View;
using CommunityToolkit.Mvvm.Input;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CardGame21.ViewModel
{
    public class NewGameViewModel : INotifyCollectionChanged
    {
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
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, NumOfDecks));
            }
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public NewGameWindow Window;

        CardLogic cardLogic;

        public ICommand StartGameCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public NewGameViewModel(MainWindow main)
        {
            CardLogic cardLogic = new CardLogic(NumOfDecks);

            StartGameCommand = new RelayCommand(() =>
            {
                GameViewModel gameViewModel = new GameViewModel(Window, cardLogic);
                Game game = new Game(gameViewModel);
                game.Show();
                Window.Hide();
                gameViewModel.FirstDraw();
            });

            PlusCommand = new RelayCommand(() =>
            {
                if (NumOfDecks < 10)
                    NumOfDecks++;
            });

            MinusCommand = new RelayCommand(() =>
            {
                if (NumOfDecks > 1)
                    NumOfDecks--;
            });

            BackCommand = new RelayCommand(() =>
            {
                main.Show();
                Window.Close();
            });
        }
    }
}
