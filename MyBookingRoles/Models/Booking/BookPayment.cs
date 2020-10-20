using MyBookingRoles.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Permissions;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyBookingRoles.Models.Booking
{
    public class BookPayment
    {
        [Key]
        public int BookPayymentID { get; set; }
        public int BookingID { get; set; }
        public virtual Bookings Bookings { get; set; }
        public int DiscountID { get; set; }
        public virtual Discount Discount { get; set; }
        public int UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public bool PaymentType { get; set; }
        public double BalanceDue { get; set; }
        public double Amount { get; set; }
        
        public double  MembershipDiscount { get; set; }
        public DateTime PaymetDate { get; set; }
        ///Email Notification///
        ///Please provide all the emails sent to clients in order
        public void SendMail()
        {
            var subject = "Studio Foto45 Bookings ";
            var body = "Dear " + ApplicationUser.UserName + ", <br /><br />Booking for : <b style='color: green'>" + Bookings.Artist.ArtistName +"on "+ Bookings.Date + " Was Successfull!</b><br />Location for event is at -<b>" + Bookings.Location + "</b>-</b><br /> Please Login to <b>Studio Foto45!</b> for your detailed booking.<hr /><b style='color: red'>Please Do not reply to this email.</b>.<br /> Thanks & Regards, <br /><b>Studio Foto45!</b>";

            string fromEmail = System.Configuration.ConfigurationManager.AppSettings["fromEmail"].ToString();
            string fromPassword = System.Configuration.ConfigurationManager.AppSettings["fromPassword"].ToString();

            MailMessage mm = new MailMessage(fromEmail, ApplicationUser.Email);
            mm.Subject = subject;
            mm.Body = body;
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.Timeout = 100000;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            NetworkCredential nc = new NetworkCredential(fromEmail, fromPassword);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;

            smtp.Send(mm);
        }
    }
}