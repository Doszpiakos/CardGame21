using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CardGame21.Model
{
    public enum Suits { Hearts, Diamonds, Clubs, Spades, Blank }

    public class Card : INotifyPropertyChanged
    {
        #region Variables/Properties

        // Card background image
        ImageBrush suitImage;
        public ImageBrush SuitImage
        {
            get
            {
                if (FaceUp)
                    return suitImage;
                return new ImageBrush(new BitmapImage(new Uri("Images/Blank.png", UriKind.Relative)));
            }
            set
            {
                value = suitImage;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SuitImage"));
            }
        }

        // Card value
        int value;
        public int Value
        {
            get
            {
                if (FaceUp)
                    return value;
                return 0;
            }
            set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        // Card is revealed
        bool faceUp;
        public bool FaceUp
        {
            get
            {
                return faceUp;
            }
            set
            {
                faceUp = value;
            }
        }

        // Card suit
        Suits suit;
        public Suits Suit
        {
            get
            {
                if (FaceUp)
                    return suit;
                return Suits.Blank;
            }
            set
            {
                suit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Suit"));
            }
        }

        // Card name
        string name;
        public string Name
        {
            get
            {
                if(FaceUp)
                    return name;
                return "Blank";
            }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public Card(int value, Suits suit, bool faceUp)
        {
            AddName(value);
            if (value == 1)
                Value = 11;
            else if (value > 11)
                Value = 10;
            else
                Value = value;
            Suit = suit;
            SetColor(suit);
            FaceUp = faceUp;
        }

        // Reveals dealers second card
        public void RevealCard()
        {
            Value = value;
            Suit = suit;
            SuitImage = suitImage;
            Name = name;
        }

        // Sets card image
        void SetColor(Suits suit)
        {
            switch (suit)
            {
                case Suits.Hearts:
                    suitImage = new ImageBrush(new BitmapImage(new Uri("Images/Heart.png", UriKind.Relative)));
                    break;
                case Suits.Diamonds:
                    suitImage = new ImageBrush(new BitmapImage(new Uri("Images/Diamond.png", UriKind.Relative)));
                    break;
                case Suits.Clubs:
                    suitImage = new ImageBrush(new BitmapImage(new Uri("Images/Club.png", UriKind.Relative)));
                    break;
                case Suits.Spades:
                    suitImage = new ImageBrush(new BitmapImage(new Uri("Images/Spade.png", UriKind.Relative)));
                    break;
            }
        }

        // Sets card name by value
        void AddName(int value)
        {
            switch (value)
            {
                case 1:
                case 11:
                    Name = "Ace";
                    break;
                case 2:
                    Name = "Two";
                    break;
                case 3:
                    Name = "Three";
                    break;
                case 4:
                    Name = "Four";
                    break;
                case 5:
                    Name = "Five";
                    break;
                case 6:
                    Name = "Six";
                    break;
                case 7:
                    Name = "Seven";
                    break;
                case 8:
                    Name = "Eight";
                    break;
                case 9:
                    Name = "Nine";
                    break;
                case 10:
                    Name = "Ten";
                    break;
                case 12:
                    Name = "Jack";
                    break;
                case 13:
                    Name = "Queen";
                    break;
                case 14:
                    Name = "King";
                    break;
            }
        }
    }
}
