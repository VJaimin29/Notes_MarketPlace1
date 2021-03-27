using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class MyRejectedNotes
    {
        public Download downloads { get; set; }
        public SellerNote currentNote { get; set; }
    }
}