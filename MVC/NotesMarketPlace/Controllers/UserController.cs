using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NotesMarketPlace.Controllers
{
    public class UserController : Controller
    {
        NotesMarketPlaceEntities2 nmp = new NotesMarketPlaceEntities2();
        // GET: User

        // [HttpPost]
        // [ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult Users()
        {
            return View();   
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Users([Bind(Exclude = "Profile_Picture")] UserProfileView up1 )
        {
    
            var data = nmp.Users.Where(x => x.EmailID == @User.Identity.Name).SingleOrDefault();

            data.First_Name = up1.First_Name;
            data.Last_Name = up1.Last_Name;
            nmp.SaveChanges();

            UserProfile up = new UserProfile();
            up.UserID = data.ID;
            up.SecondaryEmailAddress = up1.SecondaryEmailAddress;
            // up.DOB = up1.DOB;
            // up.Gender = up1.Gender;
            up.Phone_number_Country_Code = up1.Phone_number_Country_Code;
            up.Phone_Number = up1.Phone_Number;

            // for profile page upload
            //up.Profile_Picture = up1.Profile_Picture;
            byte[] imageData = null;
            HttpPostedFileBase objFiles = Request.Files["Profile_Picture"];
            using (var binaryReader = new BinaryReader(objFiles.InputStream))
            {
                imageData = binaryReader.ReadBytes(objFiles.ContentLength);
            }
            up.Profile_Picture = imageData;

            up.Address_Line1 = up1.Address_Line1;
            up.Address_Line2 = up1.Address_Line2;
            up.City = up1.City;
            up.State = up1.State;
            up.ZipCode = up1.ZipCode;
            up.Country = up1.Country;
            up.University = up1.University;
            up.College = up1.College;
            up.CreatedDate = DateTime.Now;
            up.ModifiedDate = DateTime.Now;
            up.CreatedBy = data.ID;
            up.ModifiedBy = up1.ID;
            nmp.UserProfiles.Add(up);
            nmp.SaveChanges();
            return RedirectToAction("Users");
        }

        //USER SIGn UP FOR THE FIRST TIME
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addUsers(Confirm u1)
        {

            if (!ModelState.IsValid)
            {
                return View("SignUp");
            }

            else
            {
                try
                { 
                User us = new User();


                    if (us.EmailID == u1.EmailID)
                    {
                        ViewBag.A = "User Email already exist";
                        return View("SignUp");
                    }

                us.First_Name = u1.First_Name;
                us.Last_Name = u1.Last_Name;
                us.EmailID = u1.EmailID;
                us.Password = u1.Password;
                us.RoleID = 1;
                us.IsEmailVerified = false;
                us.CreatedDate = DateTime.Now;
                us.CreatedBy = 1;
                us.ModifiedDate = DateTime.Now;
                us.ModifiedBy = 1;
                us.IsActive = true;

                nmp.Users.Add(us);
                nmp.SaveChanges();

                    //For sending the mail

                    var sendermail = new MailAddress("jaiminenter@gmail.com", "Jaimin Vyas");
                    var receivermail = new MailAddress(us.EmailID, "Sent To");

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(sendermail.Address,"Purvim1812")
                    };

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/User/HTMLEmailTemplate.cshtml")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Name}", us.First_Name);


                    using (var message = new MailMessage(sendermail, receivermail)
                    {
                        Subject = "Email Verification",
                })
                    {
                        message.Body = body;
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                    }

                    return View("SignUp");
                }

                catch (Exception e)
                {
                    // string message = Convert.ToString(e.Message);
                    ViewBag.Error = "Sign Up Failed Sorry";

                }

                return RedirectToAction("finalLogin");
            }
        }

        public ActionResult HTMLEmailTemplate(User u1)
        {
            u1.IsEmailVerified = true;
            nmp.Users.Add(u1);
            nmp.SaveChanges();
            return RedirectToAction("finalLogin");
        }

        //LOGIN OF USER MEMBER
        [HttpPost]
        public ActionResult finalLogin(User u1)
        {
            var us = nmp.Users.Where(x => x.EmailID == u1.EmailID && x.Password == u1.Password).Count();
            if (us != 0)
            {
                FormsAuthentication.SetAuthCookie(u1.EmailID, false);
                return RedirectToAction("SearchNotes","Front");
            }

            ModelState.AddModelError("", "Invalid Username Or Password");
            return View();
        }

        public ActionResult finalLogin()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult addNotes()
        {
            var item = nmp.NotesCategories.ToList();
            ViewBag.data = item;
            var item1 = nmp.Countries.ToList();
            ViewBag.data1 = item1;
            var item2 = nmp.NotesTypes.ToList();
            ViewBag.data2 = item2;

            return View();
        }

        [HttpPost]
        public ActionResult addNotes(addNotes ad1)
        {
            SellerNote sn = new SellerNote();
            var data = nmp.Users.Where(x => x.EmailID == @User.Identity.Name).SingleOrDefault();
            sn.SellerID = data.ID;
            sn.Status = 1;
            sn.AdminRemarks = "Not Done";
            sn.PublishedDate = DateTime.Now;
            sn.Title = ad1.Title;

            //For dropdown list from select Notes Category
            
            sn.Category = ad1.Category;
            sn.DisplayPicture = ad1.DisplayPicture;

            //For Seller Notes Attachments
            SellerNotesAttachement sna = new SellerNotesAttachement();
            sna.FilePath = ad1.FilePath;
            sna.FileName = ad1.FilePath;
            sna.NoteID = sn.SellerID;
            sna.IsActive = true;

            sn.NoteType = ad1.NoteType;
            sn.NumberOfPages = ad1.NumberOfPages;
            sn.Description = ad1.Description;
            sn.UniversityName = ad1.UniversityName;
            sn.Country = ad1.Country;
            sn.Course = ad1.Course;
            sn.CourseCode = ad1.CourseCode;
            sn.Professor = ad1.Professor;
            sn.IsPaid = ad1.IsPaid;
            if(sn.IsPaid)
            {
                sn.SellingPrice = ad1.SellingPrice;
            }
            else { sn.SellingPrice = 0; }

            sn.NotesPreview = ad1.NotesPreview;
            sn.IsPaid = ad1.IsPaid;
            nmp.SellerNotes.Add(sn);
            nmp.SaveChanges();
            
            return RedirectToAction("dashboard");

        }

        public ActionResult dashboard()
        {
            return View();
        }

        public ActionResult buyerRequest()
        {
            return View();
        }
    }

}