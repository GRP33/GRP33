using MyBookingRoles.Models.Booking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookingRoles.Models.RateService
{
    public class Feedback
    {

        public int FeedbackID { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        [Display(Name = "Previous Booking")]
        public int BookingID { get; set; }
        public string Answer { get; set; }
        public string Comment { get; set; }
        public string ArtistName { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Bookings Booking { get; set; }

        public ApplicationDbContext db = new ApplicationDbContext();

        public string GetARtistName()
        {
            var name = (from a in db.Bookings
                        where a.BookingID == BookingID
                        select a.Artist.ArtistName).SingleOrDefault();
            return name;
        }
    }
}