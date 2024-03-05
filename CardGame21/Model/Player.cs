﻿using System;
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
        public Player(string name)
        {
            Name = name;
            Cards = new ObservableCollection<Card>();
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
