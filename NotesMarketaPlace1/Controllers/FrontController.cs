
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class FrontController : Controller
    {
        // GET: Front

        NotesMarketPlaceEntities8 nmp = new NotesMarketPlaceEntities8();


        
        public ActionResult FAQs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchNotes()
        {
            List<SellerNote> notes = nmp.SellerNotes.ToList();
            return View(notes);
        }

        public ActionResult ContactUs()
        {

            if(!string.IsNullOrEmpty(Session["Email"] as string))
            {
                String email = Session["Email"].ToString();
                var data = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
                ContactUs cu = new ContactUs();
                cu.First_Name = data.First_Name;
                cu.EmailID = data.EmailID;

                return View(cu);
            }

            return View();
            
        }

        public ActionResult ContactUs1(ContactUs cntus)
        {
            var sendermail = new MailAddress("jaiminenter@gmail.com", "Jaimin Vyas");
            var receivermail = new MailAddress(cntus.EmailID, "Sent To");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sendermail.Address, "Purvim1812")
            };

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/User/HTMLContactUsEmail.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", cntus.First_Name);
            body = body.Replace("{Description}", cntus.Description);

            using (var message = new MailMessage(sendermail, receivermail)
            {
                Subject = cntus.Subject,
            })
            {
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }

            return RedirectToAction("ContactUs");

        }

    }
}