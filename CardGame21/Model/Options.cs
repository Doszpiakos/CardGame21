using CardGame21.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CardGame21.Model
{
    public static class Options
    {
        public static ObservableCollection<Player> Players { get; set; }

        public static int NumOfDecks { get; set; }

        public static CardLogic CardLogic { get; set; }

        public static double Left { get; set; } = 570;

        public static double Top { get; set; } = 270;

        public static double Height { get; set; } = 550;

        public static double Width { get; set; } = 780;

    }
}
