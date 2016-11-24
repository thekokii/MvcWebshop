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
    public class ManufacturersController : Controller
    {
        //private static ApplicationDbContext db = new ApplicationDbContext();
        //private IManufacturerRepository repository = new ManufacturerRepository(db);
        //private IBeerProductRepository beerRepository = new BeerProductRepository(db);
        private IManufacturerRepository repository = new ManufacturerRepository(new ApplicationDbContext());
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Manufacturers
        /*
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(repository.GetAll().ToList());
            //return View(db.Manufacturers.ToList());
        }
        */

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string searchString, int? page, int? pageSize)
        {
            var manufacturers = repository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                // Only exact matches.. no multiple words
                // TODO

                manufacturers = manufacturers.AsQueryable().Where(p => p.Name.Contains(searchString)
                                       || p.Description.Contains(searchString)
                                       || p.ManufacturerId.ToString().Contains(searchString)
                                       // Add style here
                                       );

                ViewBag.SearchString = searchString;
            }

            ViewBag.PageSize = pageSize;

            //return View(products.ToList());

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ManufacturerAdminPartial", manufacturers.ToList().OrderBy(p => p.ManufacturerId).ToPagedList(page ?? 1, pageSize ?? 2))
                : View(manufacturers.ToList().OrderBy(p => p.ManufacturerId).ToPagedList(page ?? 1, pageSize ?? 2));
        }

        public ActionResult List(string searchString, int? page)
        {
            var manufacturers = repository.GetAll();
            //var manufacturers = db.Manufacturers;


            if (!String.IsNullOrEmpty(searchString))
            {
                // Only exact matches.. no multiple words
                // TODO

                manufacturers = manufacturers.AsQueryable().Where(p => p.Name.Contains(searchString)
                                       || p.Description.Contains(searchString)
                                       || p.ManufacturerId.ToString().Contains(searchString)
                                       // Add style here
                                       );

                ViewBag.SearchString = searchString;
            }

            int pageSize = 4;
            //int pageNumber = (page ?? 1);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ManufacturerListPartial", manufacturers.ToList().OrderBy(p => p.Name).ToPagedList(page ?? 1, pageSize))
                : View(manufacturers.ToList().OrderBy(p => p.Name).ToPagedList(page ?? 1, pageSize));
        }

        // GET: Manufacturers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manufacturer manufacturer = repository.Get(id);
            //Manufacturer manufacturer = db.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return HttpNotFound();
            }

            //var beerProducts = beerRepository.GetAll().Where(b => b.ManufacturerId == id);

            //ManufacturerDetailsViewModel mdvm = new ManufacturerDetailsViewModel
            //{
            //    Manufacturer = manufacturer,
            //    Products = beerProducts.ToList().OrderBy(p => p.Name).ToPagedList(1, 4)
            //};

            ViewBag.ManufacturerId = manufacturer.ManufacturerId;
            ViewBag.PageSize = 4;

            //return View(mdvm);
            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CommonImgs = Directory.EnumerateFiles(Server.MapPath("~/Content/Images/Common/breweries"))
                              .Select(fn => "~/Content/Images/Common/breweries/" + Path.GetFileName(fn));

            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ManufacturerId,Name,Description,ImgPath")] Manufacturer manufacturer, HttpPostedFileBase file, string imgPath)
        {
            if (ModelState.IsValid)
            {
                string _imgPath;

                if (file != null)
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + manufacturer.ManufacturerId.ToString());

                    file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + manufacturer.ManufacturerId.ToString() + Path.GetExtension(file.FileName));

                    _imgPath = "/Content/Images/" + manufacturer.ManufacturerId.ToString() + Path.GetExtension(file.FileName);

                }
                else if (imgPath != null)
                {
                    _imgPath = imgPath;
                }
                else
                {
                    _imgPath = "/Content/beer.png";
                }

                manufacturer.ImgPath = _imgPath;

                repository.Add(manufacturer);
                repository.Save();
                //db.Manufacturers.Add(manufacturer);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manufacturer manufacturer = repository.Get(id);
            //Manufacturer manufacturer = db.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CommonImgs = Directory.EnumerateFiles(Server.MapPath("~/Content/Images/Common/breweries"))
                              .Select(fn => "~/Content/Images/Common/breweries/" + Path.GetFileName(fn));

            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ManufacturerId,Name,Description,ImgPath")] Manufacturer manufacturer, HttpPostedFileBase file, string imgPathHtml)
        {
           
            if (ModelState.IsValid)
            {
                string _imgPath;

                if (file != null)
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + manufacturer.ManufacturerId.ToString());

                    file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + manufacturer.ManufacturerId.ToString() + Path.GetExtension(file.FileName));

                    _imgPath = "/Content/Images/" + manufacturer.ManufacturerId.ToString() + Path.GetExtension(file.FileName);

                }
                else if (imgPathHtml != "")
                {
                    _imgPath = imgPathHtml;
                }
                else
                {
                    _imgPath = "/Content/beer.png";
                }

                manufacturer.ImgPath = _imgPath;

                repository.Update(manufacturer);
                repository.Save();
                //db.Entry(manufacturer).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Manufacturer manufacturer = repository.Get(id);
            //Manufacturer manufacturer = db.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            repository.Save();
            //Manufacturer manufacturer = db.Manufacturers.Find(id);
            //db.Manufacturers.Remove(manufacturer);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose(disposing);
            base.Dispose(disposing);
        }
    }
}
