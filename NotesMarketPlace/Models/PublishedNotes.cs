using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class PublishedNotes
    {
        public SellerNote SellerNoteDetails { get; set; }
        public NotesCategory CategoryDetails { get; set; }
        public User myUser { get; set; }
        public User approver { get; set; }
        public Download dwd { get; set; }
        public int Total { get; set; }

    }
}