using Microsoft.AspNet.Identity;
using MyBookingRoles.Models;
using MyBookingRoles.Models.RateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBookingRoles.Controllers
{
    public class FeedbacksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "SuperAdmin,Artist")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "UserID");
            var model = new Feedback
            {
                UserId = User.Identity.GetUserName()
            };
            return View(model);
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,UserId,BookingID,Answer,Comment,ArtistName")] Feedback feedback)
        {
            feedback.UserId = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                feedback.ArtistName = feedback.GetARtistName();
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("SuccessMessage");
            }

            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "UserID", feedback.BookingID);
            return View(feedback);
        }
        public ActionResult SuccessMessage()
        {
            return View();
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}