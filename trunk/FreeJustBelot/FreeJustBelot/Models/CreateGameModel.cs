using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FreeJustBelot.Models
{
    [DataContract]
    public class CreateGameModel
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}