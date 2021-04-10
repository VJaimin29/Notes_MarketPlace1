using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class RejectedNotes
    {
        public SellerNote SellerNoteDetails { get; set; }
        public NotesCategory CategoryDetails { get; set; }
        public User myUser { get; set; }
        public User actionBy { get; set; }
        public SellerNotesReportedIssue remark { get; set; }
    }
}