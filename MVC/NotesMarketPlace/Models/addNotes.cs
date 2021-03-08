using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Models
{
    public class addNotes
    {
        //Seller Notes
        public int SellerID { get; set; }
        public int Status { get; set; }
        public Nullable<int> ActionedBy { get; set; }
        public string AdminRemarks { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public string Title { get; set; }
        public int Category { get; set; }
        public string DisplayPicture { get; set; }
        public Nullable<int> NoteType { get; set; }
        public Nullable<int> NumberOfPages { get; set; }
        public string Description { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        public bool IsPaid { get; set; }
        public decimal SellingPrice { get; set; }
        public string NotesPreview { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreateedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }

        //Seller Notes Attachments
        public int NoteID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public class NotesCategory<t1 ,t2>
        {
            public t1 ID { get; set; }
            public t2 Name { get; set; }
            public IEnumerable<SelectListItem> Item { get; set; }
        }

        public class NotesTypes<t1, t2>
        {
            public t1 ID { get; set; }
            public t2 Name { get; set; }
            public IEnumerable<SelectListItem> Item3 { get; set; }
        }

        public class Countries<t1 , t2>
        {
            public t1 ID { get; set; }
            public t2 Name { get; set; }
            public IEnumerable<SelectListItem> Item1 { get; set; }
        }
    }

}