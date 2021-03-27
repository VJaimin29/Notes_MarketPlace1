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
        NotesMarketPlaceEntities8 nmp = new NotesMarketPlaceEntities8();

        public ActionResult addCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addCategory(NotesCategory nc)
        {
            NotesCategory nc1 = new NotesCategory();
            nc1.Name = nc.Name;
            nc1.Description = nc.Description;
            nc1.IsActive = true;
            nmp.NotesCategories.Add(nc1);
            nmp.SaveChanges();

            return View();
        }

        public ActionResult addType()
        {
            return View();
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

        public ActionResult addCountry()
        {
            return View();
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

        public ActionResult addAdmin()
        {
            return View();
        }

        public ActionResult adminDashboard()
        {
            List<Download> dw = nmp.Downloads.GroupBy(x => x.NoteID).Select(x => x.FirstOrDefault()).ToList();
            List<User> usr = nmp.Users.ToList();
            var multiTable = from d in dw
                             join u in usr on d.Downloader equals u.ID into Temp1
                             from u in Temp1.DefaultIfEmpty()
                             select new buyerRequest { downloads = d, users = u };

            return View(multiTable);
        }
    }
}