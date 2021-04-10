using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class memberDetails
    {
        
        public UserProfile profile { get; set; }
        public SellerNote snote { get; set; }
        public Download dnote { get; set; }
        public int DownloadedNotes { get; set; }
        public int EarnedMoney { get; set; }
    }
}