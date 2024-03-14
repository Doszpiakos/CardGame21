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
    public class Dealer : INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Variables/Properties
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
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Name));
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
                if (dealerHand != null)
                {
                    foreach (var card in dealerHand)
                    {
                        total += card.Value;
                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Total"));
            }
        }

        ObservableCollection<Card> deck;
        public ObservableCollection<Card> Deck
        {
            get
            {
                return deck;
            }
            set
            {
                deck = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Deck));
            }
        }
        ObservableCollection<Card> dealerHand;

        public ObservableCollection<Card> DealerHand
        {
            get
            {
                return dealerHand;
            }
            set
            {
                dealerHand = value;
                int calc = 0;
                foreach (var card in dealerHand)
                {
                    calc += card.Value;
                }
                Total = calc;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, DealerHand));
            }
        }

        bool dealOver;
        public bool DealOver
        {
            get
            {
                return dealOver;
            }

            set
            {
                dealOver = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DealOver"));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void RevealCard()
        {
            DealerHand[1].FaceUp = true;
            DealerHand[1].RevealCard();
            Total = 0;
        }

        public void AddACard(Card card)
        {
            dealerHand.Add(card);
            Total = 0;
        }

        public Dealer(ObservableCollection<Card> deck)
        {
            Deck = deck;
            DealerHand = new ObservableCollection<Card>();
            Name = "Dealer";
        }
    }
}
