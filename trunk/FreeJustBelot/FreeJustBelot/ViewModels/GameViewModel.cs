using FreeJustBelot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.ViewModels
{
    public class GameViewModel :BindableBase
    {
        public List<string> Cards { get; set; }

        public GameViewModel()
        {
            this.Cards = new List<string>();
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.Cards.Add("Assets/Club_10.svg");
            this.OnPropertyChanged("Cards");
        }
    }
}
