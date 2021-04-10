using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class MyDownloads
    {
        public Download downloads { get; set; }
        public User users { get; set; }
        //This is for Notes review 
        public int ratings { get; set; }
        public string comments { get; set; }
    }
}