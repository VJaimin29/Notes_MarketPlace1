using NotesMarketPlace.Context;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class FrontController : Controller
    {
        // GET: Front

        NotesMarketPlaceEntities2 nmp = new NotesMarketPlaceEntities2();

        public ActionResult FAQs()
        {
            return View();
        }

        public ActionResult SearchNotes()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }

        //Resetting the Password
        public ActionResult forgotPassword()
        {
            return View();
        }

        public ActionResult resetPassword(User u1)
        {
            var us = nmp.Users.Where(x => x.EmailID == u1.EmailID).Count();

            if(us == 0)
            {
                return HttpNotFound();
            }

            else
            {
                return RedirectToRoute("SignUp" , "User");
            }

        }

    }
}