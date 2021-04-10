using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        NotesMarketPlaceEntities nmp = new NotesMarketPlaceEntities();

        [HttpGet]
        public ActionResult addCategory(int n)
        {
            NotesCategory n1 = nmp.NotesCategories.Where(x => x.ID == n).FirstOrDefault();
             return View(n1);
        }

        [HttpPost]
        public ActionResult addCategory1(NotesCategory nc)
        {
            NotesCategory nc1 = new NotesCategory();
            nc1.Name = nc.Name;
            nc1.Description = nc.Description;
            nc1.IsActive = true;
            nmp.NotesCategories.Add(nc1);
            nmp.SaveChanges();

            return View();
        }

        public ActionResult addType(int nt)
        {
            NotesType nt1 = nmp.NotesTypes.Where(x => x.ID == nt).FirstOrDefault();

            return View(nt1);
        }

        [HttpPost]
        public ActionResult addType(NotesType nt)
        {
            NotesType nt1 = new NotesType();
            nt1.Name = nt.Name;
            nt1.Description = nt.Description;
            nt1.IsActive = true;
            nmp.NotesTypes.Add(nt1);
            nmp.SaveChanges();

            return View();
        }

        public ActionResult addCountry(int x)
        {
            Country c1 = nmp.Countries.Where(c => c.ID == x).FirstOrDefault();

            return View(c1);
        }

        [HttpPost]
        public ActionResult addCountry(Country cn)
        {
            Country cn1 = new Country();
            cn1.Name = cn.Name;
            cn1.CountryCode = cn.CountryCode;
            cn1.IsActive = true;
            nmp.Countries.Add(cn1);
            nmp.SaveChanges();

            return View();
        }

        [HttpGet]
        public ActionResult addAdmin(User u)
        {
            //User u1 = new User();
            //u1.First_Name = u.First_Name;
            return View(u);
        }

        public ActionResult adminDashboard()
        {
            List<SellerNote> dw = nmp.SellerNotes.GroupBy(x => x.ID).Select(x => x.FirstOrDefault()).ToList();
            List<User> usr = nmp.Users.ToList();
            List<NotesCategory> ncat = nmp.NotesCategories.ToList();
            var multiTable = from d in dw
                             where d.Status == 3
                             join u in usr on d.SellerID equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             join n in ncat on d.Category equals n.ID into Temp2
                             from n in Temp2.DefaultIfEmpty()
                             let avgratings = (from downloads in nmp.Downloads
                                               where downloads.NoteID == d.ID
                                               group downloads by downloads.NoteID into grp
                                               select new AvgRatings
                                               {
                                                   Total = grp.Count()
                                               })
                             select new PublishedNotes { SellerNoteDetails = d, 
                                 myUser = u,
                                 CategoryDetails =n,
                                 Total = avgratings.Select(x => x.Total).FirstOrDefault()
                             };

            return View(multiTable);
        }

        [HttpGet]
        public ActionResult notesUnderReview()
        {
            List<SellerNote> dw = nmp.SellerNotes.ToList(); 
            List<User> usr = nmp.Users.ToList();
            List<NotesCategory> nc = nmp.NotesCategories.ToList();
            var multiTable = from d in dw
                             join u in usr on d.SellerID equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             join s in nc on d.Category equals s.ID into TempT2
                             from s in TempT2.DefaultIfEmpty()
                             select new PublishedNotes { SellerNoteDetails = d , myUser = u , CategoryDetails = s};

            return View(multiTable);
        }

        public ActionResult publishedNotes()
        {
              List<SellerNote> dw = nmp.SellerNotes.ToList();
              List<User> usr = nmp.Users.ToList();
              List<User> usr1 = nmp.Users.ToList();
              List<NotesCategory> nc = nmp.NotesCategories.ToList();
              List<Download> down = nmp.Downloads.ToList();

            var multiTable = from d in dw
                             join u in usr on d.SellerID equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             join s in nc on d.Category equals s.ID into TempT2
                             from s in TempT2.DefaultIfEmpty()
                             join u1 in usr1 on d.ActionedBy equals u1.ID into TempT3
                             from u1 in TempT3.DefaultIfEmpty()
                             let avgratings = (from downloads in nmp.Downloads
                                               where downloads.NoteID == d.ID
                                               group downloads by downloads.NoteID into grp
                                               select new AvgRatings
                                               {
                                                   Total = grp.Count()
                                               })
                             select new PublishedNotes { 
                                 SellerNoteDetails = d,
                                 myUser = u,
                                 Total = avgratings.Select(a => a.Total).FirstOrDefault(),
                                 CategoryDetails = s,
                                 approver = u1
                             };

              return View(multiTable);
        }

        public ActionResult rejectedNotes()
        {
            List<SellerNote> dw = nmp.SellerNotes.ToList();
            List<User> usr = nmp.Users.ToList();
            List<User> usr1 = nmp.Users.ToList();
            List<NotesCategory> nc = nmp.NotesCategories.ToList();
            List<SellerNotesReportedIssue> rp = nmp.SellerNotesReportedIssues.ToList();
        
            var multiTable = from d in dw
                             where d.Status == 4
                             join u in usr on d.SellerID equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             join s in nc on d.Category equals s.ID into TempT2
                             from s in TempT2.DefaultIfEmpty()
                             join q in rp on d.ID equals q.NoteID into TempT3
                             from q in TempT3.DefaultIfEmpty()
                             join a in usr1 on d.ActionedBy equals a.ID into TempT4
                             from a in TempT4.DefaultIfEmpty()
                             select new RejectedNotes { SellerNoteDetails = d, myUser = u, CategoryDetails = s , remark = q , actionBy = a};

            return View(multiTable);
        }

        public ActionResult underReview (int x,int status)
        {
            SellerNotesReportedIssue sn = nmp.SellerNotesReportedIssues.Where(a => a.NoteID == x).FirstOrDefault();
            SellerNote sn1 = nmp.SellerNotes.Where(a => a.ID == x).FirstOrDefault();
            sn1.Status = status + 2;
            nmp.SaveChanges();

            return RedirectToAction("notesUnderReview");
        }

        public ActionResult spamNotes()
        {
            return View();
        }

        public ActionResult manageAdmin()
        {
            List<UserRole> u1 = nmp.UserRoles.ToList();
            List<User> u2 = nmp.Users.Where(x => x.RoleID == 2).ToList();
            List<UserProfile> u3 = nmp.UserProfiles.ToList();

            var multiTable = from d in u2
                             join u in u1 on d.RoleID equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             join c in u3 on d.ID equals c.UserID into Temp2
                             from c in Temp2.DefaultIfEmpty()
                             select new manageAdmin { us = d, usr = u , uspr=c};

            return View(multiTable);
        }

        public ActionResult addA(int x)
        {
            var i = nmp.Users.Where(c => c.ID == x).FirstOrDefault();
           
            return RedirectToAction("addAdmin", i);
        }

        public ActionResult delA(int x)
        {
            User u = nmp.Users.Where(c => c.ID == x).FirstOrDefault();
            u.IsActive = false;
            nmp.SaveChanges();

            return RedirectToAction("manageAdmin");
        }

        public ActionResult manageCategory()
        {
            List<NotesCategory> nc = nmp.NotesCategories.ToList();

            return View(nc);
        }

        public ActionResult delCat(int x)
        {
            NotesCategory n = nmp.NotesCategories.Where(c => c.ID == x).FirstOrDefault();
            n.IsActive = false;
            nmp.SaveChanges();

            return RedirectToAction("manageCategory");
        }

        public ActionResult manageCountry()
        {
            List<Country> cn = nmp.Countries.ToList();

            return View(cn);
        }

        public ActionResult delCntry(int x)
        {
            Country c = nmp.Countries.Where(a => a.ID == x).FirstOrDefault();
            c.IsActive = false;
            nmp.SaveChanges();

            return RedirectToAction("manageCountry");
        }

        public ActionResult manageType()
        {
            List<NotesType> nt = nmp.NotesTypes.ToList();

            return View(nt);
        }

        public ActionResult manageSystem()
        {
            return View();
        }

        public ActionResult downloadedNotes()
        {
            List<Download> s = nmp.Downloads.ToList();
            List<User> byr = nmp.Users.ToList();
            List<User> sl = nmp.Users.ToList();
            List<NotesCategory> ncat = nmp.NotesCategories.ToList();

            var multiTable = from d in s
                             join u1 in byr on d.Downloader equals u1.ID into Temp1
                             from u1 in Temp1.DefaultIfEmpty()
                             join u2 in sl on d.Seller equals u2.ID into Temp2
                             from u2 in Temp2.DefaultIfEmpty()
                             select new AdminDownloadedNotes { dwn = d, buyer = u1, seller = u2  };

            return View(multiTable);
        }

        public ActionResult memberPage()
        {
            List<User> us = nmp.Users.ToList();

            var multiTable = from u in us
                             let avgratings1 = (from Note in nmp.SellerNotes
                                                where Note.SellerID == u.ID                                                group Note by Note.SellerID into grp
                                                select new AvgRatings
                                                {
                                                    Total = grp.Count()
                                                })
                                let avgratings2 = (from downloads in nmp.Downloads
                                                   where downloads.Downloader == u.ID 
                                                   group downloads by downloads.Downloader into grp
                                                   select new AvgRatings
                                                   {
                                                       Total = grp.Count()
                                                   })
                                let avgratings3 = (from SellerNote in nmp.SellerNotes
                                                   where SellerNote.SellerID == u.ID & SellerNote.Status == 3
                                                   group SellerNote by SellerNote.SellerID into grp
                                                   select new AvgRatings
                                                   {
                                                       Total = grp.Count()
                                                   })
                             select new memberpage
                             {
                                 muser = u,
                                 Total1 = avgratings1.Select( x => x.Total).FirstOrDefault(),
                                 Total2 = avgratings2.Select( x => x.Total).FirstOrDefault(),
                                 Total3 = avgratings3.Select( x=> x.Total).FirstOrDefault()
                             };

            return View(multiTable);
        }

        public ActionResult delMember(int x)
        {
            User u = nmp.Users.Where(a => a.ID == x).FirstOrDefault();
            nmp.Users.Remove(u);
            nmp.SaveChanges();

            return RedirectToAction("memberPage");
        }

        [HttpGet]
        public ActionResult memberDetails(int x)
        {
            User up = nmp.Users.Where(a => a.ID == x).FirstOrDefault();
            UserProfile up1 = nmp.UserProfiles.Where(a => a.UserID == x).FirstOrDefault();
            ViewBag.D1 = up;
            ViewBag.D2 = up1;
            List<SellerNote> sn = nmp.SellerNotes.Where(a => a.SellerID == x).ToList();
            List<User> u1 = nmp.Users.Where(a => a.ID == x).ToList();
            var MultiTable = from u in u1
                             join s in sn on u.ID equals s.SellerID into Temp1
                             from s in Temp1.DefaultIfEmpty()
                             let avgratings3 = (from Download in nmp.Downloads
                                                where Download.Downloader == u.ID
                                                group Download by Download.Downloader into grp
                                                select new AvgRatings
                                                {
                                                    Total = grp.Count()
                                                })
                            /* let avgratings2 = (from Download in nmp.Downloads
                                                where Download.Downloader == u.ID
                                                group Download by Download.PurchasePrice into grp
                                                select new AvgRatingsD
                                                {
                                                    Total = grp.Sum()
                                                })*/
                             select new memberDetails
                             {
                                 snote = s,
                                 DownloadedNotes = avgratings3.Select(a => a.Total).FirstOrDefault(),
                                 //EarnedMoney = avgratings2.Select(a => a.Total).FirstOrDefault()
                             };
          
            return View(MultiTable);
        }
    }
}