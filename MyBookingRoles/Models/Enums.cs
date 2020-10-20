using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookingRoles.Models
{
    public class Enums
    {
        public enum PrintingObject
        {
            T_shirt,
            Cap,
            Cup,
            Plate,
            Bag,
            [Display(Name = "Pillow Case")]
            Pillow_Case,
            [Display(Name = "Book Covers")]
            Book_Cover,

        }
        public enum Colors
        {
            Black,
            White,
            Blue,
            Red,
            Green,
            Grey,
            Yellow,
            Variaty,
            [Display(Name = "Mixed Random Colors")]
            Mixed_Random_Colours,
        }
        public enum Sex
        {
            Male, Female
        }
        public enum Country
        {
            [Display(Name = "South Africa")]
            South_Africa,
            Botswana,
            Namibia,
            Zimbabwe,
            Mozambique,
            Swaziland,
            Lesotho
        }
        public enum Coverage
        {
            Weddings,
            Big_Events,
            Editing,
            Graduations,
            [Display(Name = "Baby Showers")]
            Baby_Showers,
            Parties,
            [Display(Name = "Studio Shoots")]
            Studio_Shoots,
            [Display(Name = "Stage Perfomance")]
            Stage_Perfomance
        }
        public enum Speciality
        {
            [Display(Name = "Video and Photography")]
            Video_And_Photography,
            Photography,
            Videography
        }
        public enum Subscription
        {
            Gold,
            Platinum,
            Silver,
            None
        }
        public enum Notificationsetting
        {
            Email,
            SMS,
            None
        }

        public enum Status
        {
            Pending,
            Approved,
            Cancelled,
            Completed
        }
    }
}