using MyBookingRoles.Models;
using MyBookingRoles.Models.Booking;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBookingRoles.Controllers.Booking
{
    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ArtistList(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var artist = db.Artists.OrderBy(x => x.ArtistName);

            //  return View(products.ToList());
            return View(artist.ToPagedList(pageNumber, pageSize));

        }
        // GET: Artists
        // GET: Booking
        public ActionResult Index(string artCoverage, string searchstring)
        {
            var coverList = new List<Enums.Speciality>();
            coverList = Enum.GetValues(typeof(Enums.Speciality))
                .Cast<Enums.Speciality>().ToList();
            ViewBag.artCoverage = new SelectList(coverList);
            var artists = from d in db.Artists
                          select d;

            if (!string.IsNullOrEmpty(searchstring))
            {
                artists = artists.Where(s => s.ArtistName.Contains(searchstring));

            }
            if (!string.IsNullOrEmpty(artCoverage))
            {
                artists = artists.Where(x => x.Speciality.ToString() == artCoverage);
            }
            return View(artists);
        }

        // Availability
        public ActionResult Availability(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return View("Error");
            }
            Bookings test = new Bookings
            {
                ArtistID = id
            };
            ViewBag.TimeBlockHelper = new SelectList(String.Empty);
            ViewBag.ArtistName = artist.ArtistName;
            return View(test);
        }
        //GET:/Artist/UpcomingBookings/5
        [Authorize(Roles = "Admin, Artist")]
        public ActionResult UpcomingBookings(string id, string SearchString)
        {
            int n;
            bool isInt = int.TryParse(id, out n);
            if (!isInt)
            {
                var user = db.Users.First(x => x.UserName == id);
               // var model = new EditUserViewModel(user);
                Artist artist = db.Artists.First(x => x.ArtistName == user.UserName);
                if (artist == null)
                {
                    return View("Error");
                }

                if (!String.IsNullOrEmpty(SearchString))
                {
                    artist.Bookings = artist.Bookings.Where(s => s.ApplicationUser.UserName.ToUpper().Contains(SearchString.ToUpper())).ToList();

                }
                // artist.Bookings.Sort();
                return View(artist);
            }
            else
            {
                if (!User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Artist artist = db.Artists.Find(n);
                if (artist == null)
                {
                    return View("Error");
                }
                // artist.Bookings.Take(10);
                return View(artist);
            }
        }

        //GET:/ Artist/History
        [Authorize(Roles = "Admin, Artist")]
        public ActionResult History(string id, string SearchString)
        {
            int n;
            bool isInt = int.TryParse(id, out n);
            if (!isInt)
            {
                var user = db.Users.First(u => u.UserName == id);
               // var model = new EditUserViewModel(user);
                Artist artist = db.Artists.First(u => u.ArtistName == user.UserName);
                if (artist == null)
                {
                    return View("Error");
                }
                if (!String.IsNullOrEmpty(SearchString))
                {
                    artist.Bookings = artist.Bookings.Where(s => s.ApplicationUser.UserName.ToUpper().Contains(SearchString.ToUpper())).ToList();
                }
                // artist.Bookings.Sort();
                return View(artist);
            }
            else
            {
                if (!User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Artist artist = db.Artists.Find(n);
                if (artist == null)
                {
                    return View("Error");
                }
                artist.Bookings.Take(15);
                return View(artist);
            }
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artist artist, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    artist.ProfilePic = new byte[file.ContentLength];
                    file.InputStream.Read(artist.ProfilePic, 0, file.ContentLength);

                }
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID,ArtistName,LastName,Speciality,RatePerHour,DisableNewBookings,PhoneNum,ProfilePic")] Artist artist,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file !=null)
                {
                    artist.ProfilePic = new byte[file.ContentLength];
                    file.InputStream.Read(artist.ProfilePic, 0, file.ContentLength);

                }
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
            db.SaveChanges();
            return RedirectToAction("Index");
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