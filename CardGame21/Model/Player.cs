using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CardGame21.Model
{
    public class Player : INotifyCollectionChanged, INotifyPropertyChanged
    {
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
            }
        }

        public Player(string name)
        {
            Name = name;
            Cards = new ObservableCollection<Card>();
            Money = 1000;
            Won = false;
            Color = "CornflowerBlue";
        }

        public void AddACard(Card card)
        {
            Cards.Add(card);
            Total = Calc();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
