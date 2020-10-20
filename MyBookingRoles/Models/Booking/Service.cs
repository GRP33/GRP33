using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookingRoles.Models.Booking
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Display(Name = "Service")]
        public string ServiceType { get; set; }

        [Display(Name = "Price (R)")]
        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        public double ServicePrice { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }

    }
}