using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BuiThiDieuNguyet.Models;
using Microsoft.AspNet.Identity;

namespace BuiThiDieuNguyet.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        public ActionResult Index()
        {
            var movies = db.Movies.Include(m => m.GenreObj);
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.GenreID = new SelectList(db.Genres, "ID", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Picture = new byte[movie.PictureUpload.ContentLength];
                movie.PictureUpload.InputStream.Read(movie.Picture, 0,
                movie.PictureUpload.ContentLength);
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreID = new SelectList(db.Genres, "ID", "Name", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreID = new SelectList(db.Genres, "ID", "Name", movie.GenreID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Summary,ReleaseDate,Genre,Price,Rated,Picture,GenreID")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreID = new SelectList(db.Genres, "ID", "Name", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
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

        public ActionResult AddToCart(int id)
        {
            //Kiem tra Id movie ton tai hay khong
            var movie = db.Movies.Where(x => x.ID == id).FirstOrDefault();
            if (movie == null)
            {
                return RedirectToAction("Index");
            }
            var hoaDon = this.Session["HoaDon"] as HoaDon;
            if (hoaDon == null)
            {
                hoaDon = new HoaDon();
                hoaDon.NgayLap = DateTime.Now;
                hoaDon.ChiTietHoaDons = new List<ChiTietHoaDon>();
                this.Session["HoaDon"] = hoaDon;
                db.HoaDons.Add(hoaDon);
            }
            //Kiem tra don hang da co truoc do
            var chiTietHoaDon = hoaDon.ChiTietHoaDons.Where(x => x.MovieObj.ID ==
            id).FirstOrDefault();
            if (chiTietHoaDon == null)
            {
                chiTietHoaDon = new ChiTietHoaDon();
                chiTietHoaDon.MaMovie = id;
                chiTietHoaDon.MovieObj = movie;
                chiTietHoaDon.HoaDonObj = hoaDon;
                chiTietHoaDon.SoLuong = 1;
                hoaDon.ChiTietHoaDons.Add(chiTietHoaDon);
            }
            else
            {
                chiTietHoaDon.SoLuong++;
            }
            db.SaveChanges();
            return View(hoaDon);
        }

        public ActionResult RemoveFromCart(int maMovies)
        {
            var hoaDon = this.Session["HoaDon"] as HoaDon;
            var chiTietHoaDon = hoaDon.ChiTietHoaDons.Where(x => x.MovieObj.ID ==
            maMovies).FirstOrDefault();
            hoaDon.ChiTietHoaDons.Remove(chiTietHoaDon);
            return View("AddToCart", hoaDon);
        }

        public PartialViewResult Summary()
        {
            var hoaDon = this.Session["HoaDon"] as HoaDon;
            if (hoaDon == null)
            {
                return null;
            }
            return PartialView(hoaDon);
        }

        public ActionResult Checkout()
        {
            return View(new ShippingDetail());
        }
        [HttpPost]
        public ActionResult Checkout(ShippingDetail detail)
        {
            var hoaDon = this.Session["HoaDon"] as HoaDon;
            if (hoaDon.ChiTietHoaDons.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("---")
                .AppendLine("Items:");
                foreach (var hoaDonChiTiet in hoaDon.ChiTietHoaDons)
                {
                    var subtotal = hoaDonChiTiet.MovieObj.Price * hoaDonChiTiet.SoLuong;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", hoaDonChiTiet.SoLuong,
                    hoaDonChiTiet.MovieObj.Title,
                    subtotal);
                }
                body.AppendFormat("Total order value: {0:c}", hoaDon.TongTien)
                .AppendLine("---")
                .AppendLine("Ship to:")
                .AppendLine(detail.Name)
                .AppendLine(detail.Address)
                .AppendLine(detail.Mobile.ToString());

                EmailServiceNew.SendEmail(new IdentityMessage()
                {
                    Destination = detail.Email,
                    Subject = "New order submitted!",
                    Body = body.ToString()
                });
                this.Session["HoaDon"] = null;
                return View("CheckoutCompleted");
            }
            else
            {
                return View(new ShippingDetail());
            }
        }
    }
}
