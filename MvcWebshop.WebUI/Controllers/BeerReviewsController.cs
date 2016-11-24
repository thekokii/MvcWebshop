using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.Models;
using Microsoft.AspNet.Identity;
using MvcWebshop.WebUI.DAL;
using PagedList;

namespace MvcWebshop.WebUI.Controllers
{
    public class BeerReviewsController : Controller
    {
        //private Repository<BeerReview> repository = new Repository<BeerReview>(new ApplicationDbContext());
        private IBeerReviewRepository repository = new BeerReviewRepository(new ApplicationDbContext());
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BeerReviews
        /*
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var reviews = repository.GetAll();
            //var reviews = db.Reviews.Include(b => b.BeerProduct);
            return View(reviews.ToList());
        }
        */

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string searchString, int? page, int? pageSize)
        {
            var beerReviews = repository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                // Only exact matches.. no multiple words
                // TODO

                beerReviews = beerReviews.AsQueryable().Where(p => p.UserInfo.ApplicationUser.UserName.Contains(searchString)
                                       || p.Text.Contains(searchString)
                                       || p.BeerProduct.Name.Contains(searchString)
                                       || p.BeerProduct.Manufacturer.Name.Contains(searchString)
                                       || p.ReviewId.ToString().Contains(searchString)
                                       || p.BeerProduct.ProductId.ToString().Contains(searchString)
                                       || p.BeerProduct.Manufacturer.ManufacturerId.ToString().Contains(searchString)
                                       // Add style here
                                       );

                ViewBag.SearchString = searchString;
            }

            ViewBag.PageSize = pageSize;

            //return View(products.ToList());

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ReviewAdminPartial", beerReviews.ToList().OrderBy(p => p.ReviewId).ToPagedList(page ?? 1, pageSize ?? 8))
                : View(beerReviews.ToList().OrderBy(p => p.ReviewId).ToPagedList(page ?? 1, pageSize ?? 8));
        }

        // GET: BeerReviews/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BeerReview beerReview = repository.Get(id);
            //BeerReview beerReview = db.Reviews.Find(id);

            if (beerReview == null)
            {
                return HttpNotFound();
            }
            return View(beerReview);
        }

        // GET: BeerReviews/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
        //    return View();
        //}

        [Authorize]
        public ActionResult Create(int productId, string returnUrl)
        {
            // Create ViewModel
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ProductId = productId;
            ViewBag.ProductName = repository.GetProducts().FirstOrDefault(p => p.ProductId == productId).Name;

            //BeerReviewCreateViewModel brcvm = new BeerReviewCreateViewModel();
            //brcvm.BeerProduct = repository.GetProducts().FirstOrDefault(p => p.ProductId == productId);
            //brcvm.ReturnUrl = returnUrl;

            return View();
        }


        // POST: BeerReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewId,Aroma,Appearance,Taste,Palate,Overall,ProductId,Text")] BeerReview beerReview, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                beerReview.UserInfoId = User.Identity.GetUserId();
                beerReview.DateTime = DateTime.Now;

                repository.Add(beerReview);
                repository.Save();
                //db.Reviews.Add(beerReview);
                //db.SaveChanges();

                //return RedirectToAction("Index");
                return Redirect(returnUrl);
            }

            //ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", beerReview.ProductId);
            ViewBag.ProductId = beerReview.ProductId;
            return View(beerReview);
        }

        // GET: BeerReviews/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BeerReview beerReview = repository.Get(id);
            //BeerReview beerReview = db.Reviews.Find(id);

            if (beerReview == null)
            {
                return HttpNotFound();
            }

            if(User.Identity.GetUserId() == beerReview.UserInfo.UserInfoId || User.IsInRole("Admin")) { 
            
            ViewBag.ProductId = beerReview.ProductId;
            //ViewBag.ProductId = new SelectList(repository.GetProducts(), "ProductId", "Name", beerReview.ProductId);
            //ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", beerReview.ProductId);
            return View(beerReview);
            } else
            {
                return RedirectToAction("Details", "BeerProducts", new { id = beerReview.ProductId });
            }
        }

        // POST: BeerReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ReviewId,Aroma,Appearance,Taste,Palate,Overall,ProductId,Text,UserInfoId")] BeerReview beerReview)
        {
            if (ModelState.IsValid)
            {
                beerReview.DateTime = DateTime.Now;

                repository.Update(beerReview);
                repository.Save();
                //db.Entry(beerReview).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "BeerProducts", new { id = beerReview.ProductId });
            }

            ViewBag.ProductId = new SelectList(repository.GetProducts(), "ProductId", "Name", beerReview.ProductId);
            //ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", beerReview.ProductId);
            return View(beerReview);
        }

        // GET: BeerReviews/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BeerReview beerReview = repository.Get(id);
            //BeerReview beerReview = db.Reviews.Find(id);

            if (beerReview == null)
            {
                return HttpNotFound();
            }
            return View(beerReview);
        }

        // POST: BeerReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            repository.Save();
            //BeerReview beerReview = db.Reviews.Find(id);
            //db.Reviews.Remove(beerReview);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose(disposing);
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}
