using Microsoft.AspNet.Identity;
using MyBookingRoles.Models;
using MyBookingRoles.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookingRoles.Controllers
{
    [Authorize(Roles = "Customer")]
    public class MyUserController : Controller
    {
        private ApplicationDbContext context;

        public MyUserController()
        {
            context = new ApplicationDbContext();
        }

        // Index
        public ActionResult Index()
        {
            return View();
        }

        // UserProfile
        public ActionResult UserProfile()
        {
            return View();
        }
        public ActionResult getBookings()
        {
            var id = User.Identity.GetUserId().ToString();
            var book = context.Bookings.Where(a => a.ApplicationUser.Id == id).ToList();
            ViewBag.Book = id;
            return View(book);
        }

        //Customer List
        public ActionResult customerOrders()
        {
            
            var id = User.Identity.GetUserName().ToString();
            var mm = context.Orders.Where(v => v.CustomerEmail == id).ToList();
            ViewBag.User = id;
            return View(mm);
        }

        public ActionResult RatingSerciveIndex()
        {
            var usr = User.Identity.GetUserName().ToString();
            var myrate_Services = context.Rate_Services.Where(b => b.Rate_ServiceUser == usr).ToList();
            return View(myrate_Services);
        }

        public ActionResult DeleteUser(string id)
        {
            var usr = context.Users.Find(id);
            return View(usr);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUserConfirm(string id)
        {
            var usr = context.Users.Find(id);
            context.Users.Remove(usr);
            context.SaveChanges();
            return RedirectToAction("Login", "Account");
        }





















        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}