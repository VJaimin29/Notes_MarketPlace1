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
        NotesMarketPlaceEntities8 nmp = new NotesMarketPlaceEntities8();
        // GET: User

        // [HttpPost]
        // [ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult Users()
        {
            String email = Session["Email"].ToString();
            var data = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            UserProfileView upv = new UserProfileView();
            upv.First_Name = data.First_Name;
            upv.Last_Name = data.Last_Name;
           
            return View(upv);   
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
            var us = nmp.Users.Where(x => x.EmailID == u1.EmailID && x.Password == u1.Password).FirstOrDefault();
            if (us != null)
            {
                Session["Email"] = us.EmailID.ToString();
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
        public ActionResult addNotes(SellerNote sn)
        {
            var item = nmp.NotesCategories.ToList();
            ViewBag.data = item;
            var item1 = nmp.Countries.ToList();
            ViewBag.data1 = item1;
            var item2 = nmp.NotesTypes.ToList();
            ViewBag.data2 = item2;
            addNotes ad1 = new addNotes();
            ad1.Title = sn.Title;

            return View(ad1);
        }

        [HttpPost]
        public ActionResult addNotes(addNotes ad1, HttpPostedFileBase file1 , HttpPostedFileBase file2,string Save,string Publish)
        {
            SellerNote sn = new SellerNote();
            var data = nmp.Users.Where(x => x.EmailID == @User.Identity.Name).SingleOrDefault();
            sn.SellerID = data.ID;

            if(!string.IsNullOrEmpty(Save))
            {
                sn.Status = 1;

            }

            if (!string.IsNullOrEmpty(Publish))
            {
                sn.Status = 2;
            }

            sn.ActionedBy = data.ID;
            sn.AdminRemarks = "Not Done";
            sn.PublishedDate = DateTime.Now;
            sn.Title = ad1.Title;
            
            //For dropdown list from select Notes Category
            sn.Category = ad1.Category;

            //TO add image of display picture
            string ImageName = System.IO.Path.GetFileName(file1.FileName);
            string physicalPath = Server.MapPath("~/img/" + ImageName);
            file1.SaveAs(physicalPath);
            sn.DisplayPicture = "img/" + ImageName;
            
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
            sn.NotesPreview = "notes/" + System.IO.Path.GetFileName(file2.FileName);

            nmp.SellerNotes.Add(sn);

            nmp.SaveChanges();

            //For Seller Notes Attachments
            SellerNotesAttachement sna = new SellerNotesAttachement();
            sna.FileName = System.IO.Path.GetFileName(file2.FileName);
            sna.FilePath = Server.MapPath("~/notes/" + sna.FileName);
            file2.SaveAs(sna.FilePath);
            sna.NoteID = sn.ID;
            sna.IsActive = true;
            nmp.SellerNotesAttachements.Add(sna);
            nmp.SaveChanges();

            return RedirectToAction("dashboard");

        }

        public ActionResult dashboard()
        {
            string email = Session["Email"].ToString();
            var usr = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            ViewBag.Data1 = usr;
           
            List<SellerNote> notes = nmp.SellerNotes.ToList();
            List<NotesCategory> catn = nmp.NotesCategories.ToList();
            List<ReferenceData> statusn = nmp.ReferenceDatas.ToList();
            var multipletable = from c in notes
                                join nc in catn on c.Category equals nc.ID into TempT1
                                from nc in TempT1.DefaultIfEmpty()
                                join sn in statusn on c.Status equals sn.ID into TempT2
                                from sn in TempT2.DefaultIfEmpty()
                                select new DashBoard { SellerNoteDetails = c, CategoryDetails = nc, ReferenceDetails = sn };


            return View(multipletable);
        }

        public ActionResult buyerRequest()
        {
            string email = Session["Email"].ToString();
            var usr1 = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            ViewBag.Data1 = usr1;


            List<Download> dw = nmp.Downloads.ToList();
            List<User> usr = nmp.Users.ToList();
            var multiTable = from d in dw
                             join u in usr on d.Downloader equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             select new buyerRequest { downloads=d , users = u};

            return View(multiTable);
        }

        [HttpGet]
        public ActionResult NotesDetails(int noteID)
        {
            //Seller Note object
            string email = Session["Email"].ToString();
            var usr = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            var sln = nmp.SellerNotes.Where(x => x.ID == noteID).SingleOrDefault();
            var slna = nmp.SellerNotesAttachements.Where(x => x.NoteID == noteID).SingleOrDefault();
            var nc = nmp.NotesCategories.Where(x => x.ID == sln.Category).SingleOrDefault();
            var cntry = nmp.Countries.Where(x => x.ID == sln.Country).SingleOrDefault();
            var susr = nmp.Users.Where(x => x.ID == sln.SellerID).FirstOrDefault();
            ViewBag.Data1 = sln;
            ViewBag.Data2 = slna;
            ViewBag.ncat = nc;
            ViewBag.cntry = cntry;
            ViewBag.usr1 = usr;
            ViewBag.usr2 = susr;

            return View();
        }

        //For the functionality to Download the notew from various prospects
        public FileResult Download(string fn)
        {
            string FilePath = Server.MapPath("~/notes/");
            string fullpath = Path.Combine(FilePath,fn);
            return File(fullpath , "application/pdf" , fn);
        }

        public FileResult Downloadfm(int fn)
        {
            var sd = nmp.SellerNotesAttachements.Where(x => x.NoteID == fn).FirstOrDefault();
            string FilePath = Server.MapPath("~/notes/");
            string fullpath = Path.Combine(FilePath, sd.FileName);
            return File(fullpath, "application/pdf", sd.FileName);
        }

        public ActionResult BuyerRequest1(int noteID,int seller)
        {
            string email = Session["Email"].ToString();
            var usr = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            var nt = nmp.SellerNotes.Where(x => x.ID == noteID).FirstOrDefault();
            var susr = nmp.Users.Where(x => x.ID == seller).FirstOrDefault();
            Download dw = new Download();
            dw.NoteID = nt.ID;
            dw.Downloader = usr.ID;
            dw.Seller = susr.ID;
            dw.IsSellerHasAllowedDownload = false;
            dw.IsAttachmentDownloaded = false;
            dw.IsPaid = false;
            dw.PurchasePrice = nt.SellingPrice;
            dw.NoteTitle = nt.Title;
            // For category
            var ncat = nmp.NotesCategories.Where(x => x.ID == nt.Category).FirstOrDefault();
            dw.NoteCategory = ncat.Name;
            nmp.Downloads.Add(dw);
            nmp.SaveChanges();

            return RedirectToAction("SearchNotes" , "Front");
        }

        public ActionResult MyDownloads()
        {
            string email = Session["Email"].ToString();
            var usr1 = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            ViewBag.Data1 = usr1;


            List<Download> dw = nmp.Downloads.ToList();
            List<User> usr = nmp.Users.ToList();
            var multiTable = from d in dw
                             join u in usr on d.Downloader equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             select new MyDownloads { downloads = d, users = u };

            return View(multiTable);
        }

        public ActionResult allowDownload(int bID)
        {
            var Dw = nmp.Downloads.Where(x => x.Downloader == bID).FirstOrDefault();
            Dw.IsSellerHasAllowedDownload = true;
            nmp.SaveChanges();

            return RedirectToAction("buyerRequest");
        }

        public ActionResult MyRejectedNotes()
        {
            string email = Session["Email"].ToString();
            var usr1 = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            ViewBag.Data1 = usr1;

            List<Download> dw = nmp.Downloads.ToList();
            List<SellerNote> usr = nmp.SellerNotes.ToList();
            var multiTable = from d in dw
                             join u in usr on d.NoteID equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             select new MyRejectedNotes { downloads = d, currentNote = u };

            return View(multiTable);
        }

        //Resetting the Password
        public ActionResult forgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult forgotPassword(User u)
        {

            if (nmp.Users.Any(x => x.EmailID == u.EmailID))
            {
                var sln = nmp.Users.Where(x => x.EmailID == u.EmailID).FirstOrDefault();
                var sendermail = new MailAddress("jaiminenter@gmail.com", "Jaimin Vyas");
                var receivermail = new MailAddress(u.EmailID, "Sent To");

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
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/User/HTMLEmailTemplate.cshtml")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{Name}", sln.Password);


                using (var message = new MailMessage(sendermail, receivermail)
                {
                    Subject = "Email Verification",
                })
                {
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
                return RedirectToAction("finalLogin", "User");
            }

            else
            {
                ModelState.AddModelError("", "Invalid Username ");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword u)
        {
            string email = Session["Email"].ToString();
            var usr = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();

            if (usr.Password == u.Password && u.newPassword == u.newPassword)
            {
                usr.Password = u.newPassword;
                nmp.SaveChanges();

                return RedirectToAction("finalLogin");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Old Password ");
            }

            return View();
        }

        public ActionResult MySoldNotes()
        {
            string email = Session["Email"].ToString();
            var usr1 = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            ViewBag.Data1 = usr1;

            List<Download> dw = nmp.Downloads.ToList();
            List<User> usr = nmp.Users.ToList();
            var multiTable = from d in dw
                             join u in usr on d.Downloader equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             select new buyerRequest { downloads = d, users = u };

            return View(multiTable);
        }


        public ActionResult Clone(int ntID)
        {
            var adn = nmp.SellerNotes.Where(x => x.ID == ntID).FirstOrDefault();
     
            return RedirectToAction("addNotes",adn);
        }

        public ActionResult noteReview(int star,string comments)
        {
            string email = Session["Email"].ToString();
            var usr = nmp.Users.Where(x => x.EmailID == email).FirstOrDefault();
            SellerNotesReview snr = new SellerNotesReview();
            snr.NoteID = 5;
            snr.ReviewByID = usr.ID;
            snr.Ratings = star;
            snr.Comments = comments;
            snr.AgainstDownloadsID = 1;
            snr.IsActive = true;
            nmp.SellerNotesReviews.Add(snr);
            nmp.SaveChanges();

            return RedirectToAction("MyDownloads");
        }
    }
}