using CardGame21.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace CardGame21.Logic
{
    public class CardLogic
    {
        Random randomGen = new Random();

        public ObservableCollection<Card> Cards { get; }

        Suits[] Suits;

        int numOfDecks;

        public CardLogic(int numOfDecks)
        {
            Cards = new ObservableCollection<Card>();

            this.numOfDecks = numOfDecks;

            Suits = new Suits[4];
            Suits[0] = Model.Suits.Diamonds;
            Suits[1] = Model.Suits.Hearts;
            Suits[2] = Model.Suits.Spades;
            Suits[3] = Model.Suits.Clubs;

            GenerateDecks();
        }

        // Generate decks, based on number of decks
        void GenerateDecks()
        {
            for (int i = 0; i < numOfDecks; i++)
            {
                for (int s = 0; s < Suits.Length; s++)
                {
                    for (int j = 1; j < 15; j++)
                    {
                        if (j == 11)
                            j++;
                        Card card = new Card(j, Suits[s], true);
                        Cards.Add(card);
                    }
                }
            }
        }

        // Draw a card from remaining pile
        // Checks if deck is empty
        public Card DrawCard(bool faceUp)
        {
            if (Cards.Count == 0)
            {
                MessageBox.Show("Deck is empty, generating new deck!");
                GenerateDecks();
            }

            int draw = randomGen.Next(0, Cards.Count);

            Card card = Cards[draw];

            Cards.RemoveAt(draw);
            
            if (!faceUp)
                card.FaceUp = false;

            return card;
        }
    }
}
