using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.Models
{
    public class RoomModel
    {
        public List<string> Players { get; set; }

        public RoomModel()
        {
            this.Players = new List<string>();
        }
    }
}
