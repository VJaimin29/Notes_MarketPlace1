using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class ChangePassword
    {
        public string Password { get; set; }
        public string newPassword { get; set; }
        
        [Compare("newPassword")]
        public string confirmNewPassword { get; set; }
    }
}