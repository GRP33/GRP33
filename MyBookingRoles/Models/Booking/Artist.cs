using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBookingRoles.Models.Booking
{
    public class Artist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ArtistID { get; set; }

        [Required(ErrorMessage = "First name Required")]
        [Display(Name = "First Name")]
        public string ArtistName { get; set; }
        [Required(ErrorMessage = "Last name required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Select atleast one speciality ")]
        [Display(Name = "Specialization")]
        public Enums.Speciality Speciality { get; set; }

        [Required]
        [Display(Name = "Rate Per Hour (R)")]
        [ScaffoldColumn(true)]
        [DataType(DataType.Currency)]
        public double RatePerHour { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }

        [Display(Name = "New Appointments Disable ?")]
        public Boolean DisableNewBookings { get; set; }
        [Required(ErrorMessage = "Phone number required")]
        [StringLength(10, ErrorMessage = "The {0} must be 10 digits")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNum { get; set; }

        [Display(Name = "Profile Image")]
        [DataType(DataType.ImageUrl)]
        public byte[] ProfilePic { get; set; }

    }
}