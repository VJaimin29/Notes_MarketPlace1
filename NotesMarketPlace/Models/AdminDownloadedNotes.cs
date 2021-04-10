using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class AdminDownloadedNotes
    {
        public Download dwn { get; set; }
        public User buyer { get; set; }
        public User seller { get; set; }
        public NotesCategory nc { get; set; }
    }
}