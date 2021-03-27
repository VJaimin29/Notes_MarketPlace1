using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Models
{
    public class ContactUs
    {
        public string First_Name { get; set; }
        public string EmailID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}