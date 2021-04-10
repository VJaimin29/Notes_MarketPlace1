using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class AdminRejectedNotes
    {
        public SellerNote SellerNoteDetails { get; set; }
        public NotesCategory CategoryDetails { get; set; }
        public User myUser { get; set; }
        public SellerNotesReportedIssue noteIssue{ get; set;}
    }
}