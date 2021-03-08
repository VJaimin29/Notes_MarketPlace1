using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NotesMarketPlace.Models;

namespace NotesMarketPlace.Models
{
    public class Confirm
    {
        //User u = new User();

        [Required(ErrorMessage = "Please enter First Name")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Please enter Email ID")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public String ConfirmPassword { get; set; }
    }
}