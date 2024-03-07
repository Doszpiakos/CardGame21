using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame21.Model
{
    public static class Options
    {
        public static ObservableCollection<Player> Players { get; set; }

        public static int Decks { get; set; }

        public static int DealerRule { get; set; }

    }
}
