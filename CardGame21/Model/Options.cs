using CardGame21.Logic;
using System.Collections.ObjectModel;

namespace CardGame21.Model
{
    public static class Options
    {
        // Collection of current players
        public static ObservableCollection<Player> Players { get; set; }

        // Number of decks in play
        public static int NumOfDecks { get; set; }

        // Cardlogic
        public static CardLogic CardLogic { get; set; }

        // Window left side
        public static double Left { get; set; } = 570;

        // Window top side
        public static double Top { get; set; } = 270;

        // Window height
        public static double Height { get; set; } = 550;

        // Window width
        public static double Width { get; set; } = 780;

    }
}
