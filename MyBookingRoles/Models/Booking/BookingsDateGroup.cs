using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookingRoles.Models.Booking
{
    public class BookingsDateGroup
    {

        [Display(Name = "Bookings Date ")]
        [DataType(DataType.Date)]
        public DateTime? BookingsDate { get; set; }
        public int CustomerCount { get; set; }

    }
}