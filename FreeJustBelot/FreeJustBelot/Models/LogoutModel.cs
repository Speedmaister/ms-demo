using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FreeJustBelot.Models
{
    [DataContract]
    public class LogoutModel
    {
        [DataMember(Name="sessionKey")]
        public string SessionKey { get; set; }
    }
}