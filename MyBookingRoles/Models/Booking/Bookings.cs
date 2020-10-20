using DHTMLX.Scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBookingRoles.Models.Booking
{
    public class Bookings
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DHXJson(Alias = "id")]
        public int BookingID { get; set; }
        [Display(Name = "User")]
        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int CustomerID { get; set; }
       // public CustomerProfile Customer { get; set; }
        public string CustomerName { get; set; }
        [Display(Name = "Artist")]
        public int ArtistID { get; set; }
        public virtual Artist Artist { get; set; }

        [Display(Name = "Location")]
        [Required]
        public string Location { get; set; }

        // p public virtual Location Location { get; set; }
        // [Display(Name = " Select Package")]

        [Required]
        public int PackageId { get; set; }

        public virtual Package Package { get; set; }

        [Required]
        [Display(Name = "Date of Event*")]
        [DHXJson(Alias = "start_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        // [MyEventDateValidation(ErrorMessage="Are you creating an appointment ?")]

        public DateTime Date { get; set; }

        [Display(Name = "Time*")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int CompareTo(Bookings other)
        {
            return this.Date.Date.Add(this.Time.TimeOfDay).CompareTo(other.Date.Date.Add(other.Time.TimeOfDay));
        }
        [Display(Name = "Expected end Time")]
        [DHXJson(Alias = "end_date")]
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd }", ApplyFormatInEditMode = true)]

        public DateTime end { get; set; }
        [Required]
        [Display(Name = "Duration (in hours)*")]
        public int Duration { get; set; }
        [Display(Name = "Select Service Type")]

        [Required]
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }

        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Booking Fee (R)")]
        public double BookingFee { get; set; }

        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Artist Rate Fee (R)")]
        public double ArtistRateFee { get; set; }

        //[ScaffoldColumn(true)]
        //[DataType(DataType.Currency)]
        //[Display(Name = "Location / Venue Fee (R)")]
       // public double LocationVenueFee { get; set; }

        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Package Cost (R)")]
        public double PackageCost { get; set; }

        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Event / Service Fee (R)")]
        public double EventFee { get; set; }

        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Discount (R)")]
        public double Discount { get; set; }

        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Due (R)")]
        public double TotalDue { get; set; }
        public string TimeBlockHelper { get; set; }
        [DHXJson(Alias = "text")]
        public string Status { get; set; }

        //Methods
        public ApplicationDbContext db = new ApplicationDbContext();
        public void MigrateUser(string userName)
        {
            var booking = db.Users.Where(c => c.Id == BookingID.ToString());
            foreach (ApplicationUser item in booking)
            {
                item.UserName = userName;
            }

        //}
        //public string getCName()
        //{
        //    var name = (from a in db.Customers where a.UserID == UserID select a.Name).SingleOrDefault();
        //    return name;
        //}
        ////Calculations

        //public double calcBookingFee()
        //{
        //    return 50;
        //}

        //public double calcArtistRate()
        //{
        //    //var art = from a in db.Artists
        //    //          where a.ArtistID == ArtistID
        //    //          select a.RatePerHour;

        //    var art = db.Artists.Where(a => a.ArtistID == ArtistID)
        //                        .Select(a => a.RatePerHour)
        //                        .Single();

        //    return Convert.ToDouble(art);
        //}
        //public int getCustomerID()
        //{
        //    var id = (from a in db.Customers where a.UserID == UserID select a.CustomerID).SingleOrDefault();
        //    return id;
        //}
        ////public int calcDuration()
        ////{
        ////    int hour = 0;
        ////    hour = (Convert.ToInt16(Date - end) / 60) * 60;
        ////    return hour;
        ////}
        //public double calcArtistFee()
        //{
        //    return calcArtistRate() * calcPackageCost();
        //}

        //public double calcLocationFee()
        //{
        //    var loc = db.Locations.Where(l => l.LocationId == LocationId)
        //                          .Select(l => l.LocationPrice)
        //                          .Single();

        //    return Convert.ToDouble(loc);
        //}

        //public double calcPackageCost()
        //{
        //    //var pac = from p in db.Packages
        //    //          where p.PackageId == PackageId
        //    //          select p.PackagePrice;

        //    var pac = db.Packages.Where(p => p.PackageId == PackageId)
        //                         .Select(p => p.PackagePrice)
        //                         .Single();

        //    return Convert.ToDouble(pac);
        //}

        //public double calcEventFee()
        //{
        //    var even = db.Services.Where(s => s.ServiceId == ServiceId)
        //                          .Select(s => s.ServicePrice)
        //                          .Single();

        //    return Convert.ToDouble(even);
        //}

        //public double calcTotalBeforeDiscout()
        //{
        //    return calcBookingFee() + calcArtistFee() + calcEventFee()
        //          + calcLocationFee() + calcPackageCost();
        //}

        //public double calcDiscount()
        //{
        //    double disc = 0;

        //    if (calcTotalBeforeDiscout() >= 8000)
        //    {
        //        disc = calcTotalBeforeDiscout() * 0.15;
        //    }
        //    else if (calcTotalBeforeDiscout() >= 10000)
        //    {
        //        disc = calcTotalBeforeDiscout() * 2;
        //    }
        //    else
        //    {
        //        disc = 0;
        //    }

        //    return disc;
        //}
        //public double calcToatlDue()
        //{
        //    return calcTotalBeforeDiscout() - calcDiscount();
        //
        }
    }
}