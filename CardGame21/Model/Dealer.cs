using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CardGame21.Model
{
    public class Dealer : INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Variables/Properties
        
        // Dealers total card value
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

        // Current deck in play
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

        // Dealers deck
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

        // Dealers turn finished
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

        // Reveal dealers second card
        public void RevealCard()
        {
            DealerHand[1].FaceUp = true;
            DealerHand[1].RevealCard();
            Total = 0;
        }

        // Add a card to hand
        public void AddACard(Card card)
        {
            dealerHand.Add(card);
            Total = 0;
        }

        public Dealer(ObservableCollection<Card> deck)
        {
            Deck = deck;
            DealerHand = new ObservableCollection<Card>();
        }
    }
}
