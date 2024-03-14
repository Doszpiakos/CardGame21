using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame21.Model
{
    public class Info : INotifyPropertyChanged
    {
        public enum MessageColors { Hit, Stand, Bust, Won, Turn }

        #region Variables/Properties

        string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public Info(string message, MessageColors messageColor)
        {
            Message = message;
            string color = "";
            switch (messageColor)
            {
                case MessageColors.Hit:
                    color = "Maroon";
                    break;
                case MessageColors.Stand:
                    color = "Navy";
                    break;
                case MessageColors.Bust:
                    color = "Red";
                    break;
                case MessageColors.Won:
                    color = "Green";
                    break;
                case MessageColors.Turn:
                    color = "Blue";
                    break;
            }
            Color = color;
        }
    }
}
