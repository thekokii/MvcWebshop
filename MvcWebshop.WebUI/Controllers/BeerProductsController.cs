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
using PagedList;
using MvcWebshop.WebUI.DAL;
using System.IO;

namespace MvcWebshop.WebUI.Controllers
{
    public class BeerProductsController : Controller
    {
        private IBeerProductRepository repository = new BeerProductRepository(new ApplicationDbContext());
        private ApplicationDbContext db = new ApplicationDbContext();
        //public int pageSize = 2;

        // GET: BeerProducts
        /*
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //var products = db.Products.Include(b => b.Manufacturer);
            var products = repository.GetAll();
            return View(products.ToList());
        }
        */

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string searchString, int? page, int? pageSize)
        {
            var products = repository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                // Only exact matches.. no multiple words
                // TODO

                products = products.AsQueryable().Where(p => p.Name.Contains(searchString)
                                       || p.Manufacturer.Name.Contains(searchString)
                                       || p.Description.Contains(searchString)
                                       || p.ProductId.ToString().Contains(searchString)
                                       // Add style here
                                       );
                
                ViewBag.SearchString = searchString;
            }

            ViewBag.PageSize = pageSize;

            //return View(products.ToList());

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ProductAdminPartial", products.ToList().OrderBy(p => p.ProductId).ToPagedList(page ?? 1, pageSize ?? 8))
                : View(products.ToList().OrderBy(p => p.ProductId).ToPagedList(page ?? 1, pageSize ?? 8));
        }

        public ActionResult List(bool? descending, string orderBy, string searchString, int? page, int? pageSize, int? manufacturerId)
        {
            var products = repository.GetAll();
            //var products = db.Products.Include(b => b.Manufacturer);

            if (!String.IsNullOrEmpty(searchString))
            {
                // Only exact matches.. no multiple words
                // TODO

                products = products.AsQueryable().Where(p => p.Name.Contains(searchString)
                                       || p.Manufacturer.Name.Contains(searchString)
                                       || p.Description.Contains(searchString)
                                       // Add style here
                                       );

                ViewBag.SearchString = searchString;
            }

            if (manufacturerId != null)
            {
                products = products.Where(p => p.ManufacturerId == manufacturerId);
                if(products.Any()) { 
                    ViewBag.ManufacturerName = products.FirstOrDefault().Manufacturer.Name;
                }
            }

            switch (orderBy)
            {
                case "name":            products = products.OrderBy(p => p.Name);
                    break;
                case "manufacturer":    products = products.OrderBy(p => p.Manufacturer.Name);
                    break;
                case "price":           products = products.OrderBy(p => p.Price);
                    break;
                case "abv":             products = products.OrderBy(p => p.ABV);
                    break;
                default:    products = products.OrderBy(p => p.Name); break;
            }

            if (descending ?? false)
            {
                products = products.Reverse();
            }
            
            //products = products.OrderBy(p => p.Name);

            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.PageSize = pageSize;
            ViewBag.Descending = descending;
            ViewBag.OrderBy = orderBy;
            ViewBag.DescendingDDL = new SelectList(new List<string> { "ascending", "descending" }, "descending", "Ascending");

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ProductListPartial", products.ToList().ToPagedList(page ?? 1, pageSize ?? 8))
                : View(products.ToList().ToPagedList(page ?? 1, pageSize ?? 8));
        }


        // GET: BeerProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //BeerProduct beerProduct = db.Products.Include(b => b.BeerReviews)
            //                                     .Where(b => b.ProductId == id)
            //                                     .FirstOrDefault();

            BeerProduct beerProduct = repository.Get(id);
            
            if (beerProduct == null)
            {
                return HttpNotFound();
            }

            //BeerProduct beerProduct = db.Products.Find(id);
            beerProduct.BeerReviews = beerProduct.BeerReviews.OrderByDescending(br => br.DateTime).ToList();

            return View(beerProduct);
        }

        // GET: BeerProducts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CommonImgs = Directory.EnumerateFiles(Server.MapPath("~/Content/Images/Common/beers"))
                              .Select(fn => "~/Content/Images/Common/beers/" + Path.GetFileName(fn));

            ViewBag.ManufacturerId = new SelectList(repository.GetManufacturers(), "ManufacturerId", "Name");
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "Name");
            return View();
        }

        // POST: BeerProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductId,ManufacturerId,Name,Quantity,Price,Description,ABV")] BeerProduct beerProduct, HttpPostedFileBase file, string imgPath)
        {
            if (ModelState.IsValid)
            {
                string _imgPath;

                repository.Add(beerProduct);
                repository.Save();

                if (file != null)
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + beerProduct.ManufacturerId.ToString());

                    file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + beerProduct.ManufacturerId.ToString() + "/" + beerProduct.ProductId + Path.GetExtension(file.FileName));

                    _imgPath = "/Content/Images/" + beerProduct.ManufacturerId.ToString() + "/" + beerProduct.ProductId + Path.GetExtension(file.FileName);

                }
                else if (imgPath != null)
                {
                    _imgPath = imgPath;
                }
                else
                {
                    _imgPath = "/Content/beer.png";
                }

                beerProduct.ImgPath = _imgPath;

                repository.Update(beerProduct);
                repository.Save();

                //db.Products.Add(beerProduct);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManufacturerId = new SelectList(repository.GetManufacturers(), "ManufacturerId", "Name", beerProduct.ManufacturerId);
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "Name", beerProduct.ManufacturerId);
            return View(beerProduct);
        }

        // GET: BeerProducts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeerProduct beerProduct = repository.Get(id);
            //BeerProduct beerProduct = db.Products.Find(id);
            if (beerProduct == null)
            {
                return HttpNotFound();
            }

            ViewBag.CommonImgs = Directory.EnumerateFiles(Server.MapPath("~/Content/Images/Common/beers"))
                              .Select(fn => "~/Content/Images/Common/beers/" + Path.GetFileName(fn));

            ViewBag.ManufacturerId = new SelectList(repository.GetManufacturers(), "ManufacturerId", "Name", beerProduct.ManufacturerId);
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "Name", beerProduct.ManufacturerId);
            return View(beerProduct);
        }

        // POST: BeerProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductId,ManufacturerId,Name,Quantity,Price,Description,ImgPath,ABV")] BeerProduct beerProduct, HttpPostedFileBase file, string imgPathHtml)
        {
            if (ModelState.IsValid)
            {
                string _imgPath;

                if (file != null)
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + beerProduct.ManufacturerId.ToString());

                    file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + beerProduct.ManufacturerId.ToString() + "/" + beerProduct.ProductId + Path.GetExtension(file.FileName));

                    _imgPath = "/Content/Images/" + beerProduct.ManufacturerId.ToString() + "/" + beerProduct.ProductId + Path.GetExtension(file.FileName);

                }
                else if (imgPathHtml != "")
                {
                    _imgPath = imgPathHtml;
                }
                else
                {
                    _imgPath = beerProduct.ImgPath;
                }

                beerProduct.ImgPath = _imgPath;

                repository.Update(beerProduct);
                repository.Save();
                //db.Entry(beerProduct).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManufacturerId = new SelectList(repository.GetManufacturers(), "ManufacturerId", "Name", beerProduct.ManufacturerId);
            //ViewBag.ManufacturerId = new SelectList(db.Manufacturers, "ManufacturerId", "Name", beerProduct.ManufacturerId);
            return View(beerProduct);
        }

        // GET: BeerProducts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeerProduct beerProduct = repository.Get(id);
            //BeerProduct beerProduct = db.Products.Find(id);
            if (beerProduct == null)
            {
                return HttpNotFound();
            }
            return View(beerProduct);
        }

        // POST: BeerProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            repository.Save();
            //BeerProduct beerProduct = db.Products.Find(id);
            //db.Products.Remove(beerProduct);
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
