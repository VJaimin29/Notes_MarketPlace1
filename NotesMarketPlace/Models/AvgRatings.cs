using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class AvgRatings
    {
        public SellerNote Note { get; set; }
        public int Total { get; set; }
        public NotesCategory category { get; set; }
        public User user { get; set; }
        public User approver { get; set; }
    }
}