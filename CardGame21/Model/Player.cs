using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CardGame21.Model
{
    public class Player : INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Variables/Properties

        // Current/Selected player color change
        string color;
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
            }
        }

        // Player name
        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        // Player total card value
        int total = 0;
        public int Total
        {
            get
            {
                return total;
            }

            set
            {
                total = 0;
                foreach (var card in cards)
                {
                    total += card.Value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Total"));
            }
        }

        // Player current hand
        ObservableCollection<Card> cards;
        public ObservableCollection<Card> Cards
        {
            get
            {
                return cards;
            }
            set
            {
                cards = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Cards));
            }
        }

        // Player current bet
        int bet;
        public int Bet
        {
            get
            {
                return bet;
            }
            set
            {
                bet = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bet"));
            }
        }

        // Player current money
        int money;
        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Money"));
            }
        }

        // Player won this round
        bool won;
        public bool Won
        {
            get
            {
                return won;
            }
            set
            {
                won = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Won"));
            }
        }

        // Player won/lost already logged
        bool checkedStatus;
        public bool CheckedStatus
        {
            get
            {
                return checkedStatus;
            }
            set
            {
                checkedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheckedStatus"));
                ChangeColor();
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public Player(string name)
        {
            Name = name;
            Cards = new ObservableCollection<Card>();
            Money = 500;
            Won = false;
            Color = "CornflowerBlue";
        }

        void ChangeColor()
        {
            if (won)
                Color = "Green";
            else
                Color = "Red";
        }

        // Checks and changes "ace" values in hand
        public int Calc()
        {
            int calc = 0;
            int aceCounter = 0;

            foreach (var card in cards)
            {
                if (card.Value == 11)
                    aceCounter++;
                calc += card.Value;
            }

            if (calc > 21 && aceCounter > 0)
            {
                int i = 0;

                while (calc > 21 && i < cards.Count)
                {
                    if (cards[i].Value == 11)
                    {
                        aceCounter--;
                        cards[i].Value = 1;
                        calc -= 10;
                    }
                    i++;
                }
            }

            return calc;
        }

        // Add a card to hand
        public void AddACard(Card card)
        {
            Cards.Add(card);
            Total = Calc();
        }
    }
}
