using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class DashBoard
    {
        public SellerNote SellerNoteDetails{get; set;}
        public NotesCategory CategoryDetails { get; set; }
        public ReferenceData ReferenceDetails { get; set; }
        public int Totalsold { get; set; }
    }
}