using Microsoft.AspNet.Identity;
using MyBookingRoles.Models;
using MyBookingRoles.Models.Booking;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBookingRoles.Controllers.Booking
{
    public class BookingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        // [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Artist).Include(b => b.Package).Include(b => b.Service);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.ArtistID = new SelectList(db.Artists.Where(x => x.DisableNewBookings == false), "ArtistID", "ArtistName");
            //ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationType");
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageType");
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "ServiceType");

            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            var model = new Bookings
            {
                UserID = User.Identity.GetUserId()
            };
            return View(model);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,UserID,ArtistID,LocationId,PackageId,Date,Time,TimeBlockHelper,Duration,ServiceId,BookingFee,ArtistRateFee,LocationVenueFee,PackageCost,EventFee,Discount,TotalDue,Status")] Bookings bookings)
        {
            bookings.UserID = User.Identity.GetUserId();
            //CHECK TIME
            if (bookings.TimeBlockHelper == "DONT")
                ModelState.AddModelError("", " No bookings available for " + bookings.Date.ToShortDateString());
            else
            {
                //sets Time
                bookings.Time = DateTime.Parse(bookings.TimeBlockHelper);

                //Check workingHours
                DateTime start = bookings.Date.Add(bookings.Time.TimeOfDay);
                DateTime end = (bookings.Date.Add(bookings.Time.TimeOfDay)).AddMinutes(double.Parse(db.Administrations.Find(1).Value));
                if (!(BusinessLogic.IsInWorkingHours(start, end)))
                    ModelState.AddModelError("", " Artist Working Hours are from " + int.Parse(db.Administrations.Find(2).Value) + " to " + int.Parse(db.Administrations.Find(3).Value));

                //Check Booking Clash
                string check = BusinessLogic.ValidateNoAppoinmentClash(bookings);
                if (check != "")
                    ModelState.AddModelError("", check);

                string status = "";

                if (bookings.Status == "")
                {
                    status = "Pending";
                }
                bookings.Status = status;
            }

            if (Session["book"]==null)
            {
                if (ModelState.IsValid)
                {
                   
                    // bookings.Duration = bookings.calcDuration();
                    //bookings.CustomerName = bookings.getCName();
                    //bookings.CustomerID = bookings.getCustomerID();
                    //bookings.BookingFee = bookings.calcBookingFee();
                    //bookings.ArtistRateFee = bookings.calcArtistFee();
                    //bookings.Discount = bookings.calcDiscount();
                    //bookings.PackageCost = bookings.calcPackageCost();
                    //bookings.LocationVenueFee = bookings.calcLocationFee();
                    //bookings.EventFee = bookings.calcEventFee();
                    //bookings.TotalDue = bookings.calcToatlDue();
                    db.Bookings.Add(bookings);
                    Session["book"] = bookings;
                    db.SaveChanges();
                    return RedirectToAction("Succesful", new
                    {
                        Controller = "Bookings",
                        Action = "Succesful"
                    });
                }
            }
            ViewBag.ArtistID = new SelectList(db.Artists.Where(x => x.DisableNewBookings == false), "ArtistID", "ArtistName", bookings.ArtistID);
            //ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationType", bookings.LocationId);
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageType", bookings.PackageId);
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "ServiceType", bookings.ServiceId);
            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            return View(bookings);
        }
        public ActionResult Succesful()
        {
            //  var bookings = db.Bookings.Include(b => b.Artist).Include(b => b.Location).Include(b => b.Package).Include(b => b.Service);
            return View();

        }
        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            ViewBag.Date = bookings.Date.Date;
            ViewBag.ArtistID = new SelectList(db.Artists.Where(x => x.DisableNewBookings == false), "ArtistID", "ArtistName", bookings.ArtistID);
            //ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationType", bookings.LocationId);
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageType", bookings.PackageId);
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "ServiceType", bookings.ServiceId);
            return View(bookings);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "BookingID,UserID,ArtistID,LocationId,PackageId,Date,Time,TimeBlockHelper,Duration,ServiceId,BookingFee,ArtistRateFee,LocationVenueFee,PackageCost,EventFee,Discount,TotalDue,Status")] Bookings bookings)
        {
            // verify time
            if (bookings.TimeBlockHelper == "DONT")
                ModelState.AddModelError("", "No bookings available for " + bookings.Date.ToShortDateString());
            else
            {
                //Set Time
                bookings.Time = DateTime.Parse(bookings.TimeBlockHelper);
                //Check WorkingHours
                DateTime start = new DateTime(bookings.Date.Year, bookings.Date.Month, bookings.Date.Day, bookings.Time.Hour, bookings.Time.Minute, bookings.Time.Second);
                DateTime end = new DateTime(bookings.Date.Year, bookings.Date.Month, bookings.Date.Day, bookings.Time.Hour, bookings.Time.Minute, bookings.Time.Second).AddMinutes(double.Parse(db.Administrations.Find(1).Value));
                if (!BusinessLogic.IsInWorkingHours(start, end))
                    ModelState.AddModelError("", "Artists Working Hours are from " + int.Parse(db.Administrations.Find(2).Value) + " to " + int.Parse(db.Administrations.Find(3).Value));
            }

            if (ModelState.IsValid)
            {
                db.Entry(bookings).State = EntityState.Modified;
                db.Entry(bookings).Property(u => u.UserID).IsModified = false;
                db.Entry(bookings).Property(u => u.Date).IsModified = false;
                db.Entry(bookings).Property(u => u.Time).IsModified = false;
                db.Entry(bookings).Property(u => u.PackageId).IsModified = false;
                db.Entry(bookings).Property(u => u.Location).IsModified = false;
                db.Entry(bookings).Property(u => u.Duration).IsModified = false;
                db.Entry(bookings).Property(u => u.ServiceId).IsModified = false;
                db.Entry(bookings).Property(u => u.TotalDue).IsModified = false;
                db.Entry(bookings).Property(u => u.BookingFee).IsModified = false;
                db.Entry(bookings).Property(u => u.ArtistRateFee).IsModified = false;
               // db.Entry(bookings).Property(u => u.LocationVenueFee).IsModified = false;
                db.Entry(bookings).Property(u => u.Discount).IsModified = false;
                db.Entry(bookings).Property(u => u.PackageCost).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            ViewBag.ArtistID = new SelectList(db.Artists.Where(x => x.DisableNewBookings == false), "ArtistID", "ArtistName", bookings.ArtistID);
          //  ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationType", bookings.LocationId);
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageType", bookings.PackageId);
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "ServiceType", bookings.ServiceId);
            return View(bookings);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookings bookings = db.Bookings.Find(id);
            db.Bookings.Remove(bookings);
            db.SaveChanges();
            if (User.IsInRole("SuperAdmin"))
                return RedirectToAction("Index");
            else if (User.IsInRole("Artist"))
                return RedirectToAction("Details", new { Controller = "Artist", Action = "UpcomingBookings", id = User.Identity.Name });
            else if (User.IsInRole("Client"))
                return RedirectToAction("Details", new { Controller = "RegisterdUsers", Action = "Details" });
            else
                return RedirectToAction("Index", new { Controller = "Home", Action = "Index" });

        }
        [HttpPost]
        public JsonResult GetAvailableAppointments(int artID, DateTime date)
        {
            List<SelectListItem> resultlist = BusinessLogic.AvailableAppointments(artID, date);
            return Json(resultlist);
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