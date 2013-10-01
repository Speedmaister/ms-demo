using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeJustBelot.Models
{
    public class GameModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int PlayersWaiting { get; set; }
    }
}