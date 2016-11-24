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
using PagedList;

namespace MvcWebshop.WebUI.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [Authorize(Roles = ("Admin"))]
        public ActionResult Index(string searchString, int? page, int? pageSize)
        {
            var orders = db.Orders.AsQueryable();
            
            if (!String.IsNullOrEmpty(searchString))
            {

                orders = orders.Where(p => p.UserInfo.ApplicationUser.UserName.Contains(searchString)
                                       || p.OrderLines.Sum(ol => ol.Quantity * ol.Product.Price).ToString().Contains(searchString)
                                       || p.OrderId.ToString().Contains(searchString)
                                       // Add style here
                                       );

                ViewBag.SearchString = searchString;
            }

            ViewBag.PageSize = pageSize;

            //return View(products.ToList());

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("OrderAdminPartial", orders.ToList().OrderBy(p => p.OrderId).ToPagedList(page ?? 1, pageSize ?? 10))
                : View(orders.ToList().OrderBy(p => p.OrderId).ToPagedList(page ?? 1, pageSize ?? 10));
            
            //return View(orders.ToList());
        }

        [Authorize]
        public ActionResult List()
        {
            var userInfoId = User.Identity.GetUserId();
            var orders = db.Orders.Where(o => o.UserInfoId == userInfoId);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return RedirectToAction("List", "orders");
            }

            if (User.Identity.GetUserId() == order.UserInfo.UserInfoId || User.IsInRole("Admin"))
            {
                return View(order);
            }
            else
            {
                return RedirectToAction("List", "orders");
            }
        }

        /*
        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserInfoId = new SelectList(db.UserInfos, "UserInfoId", "UserInfoId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,DateTime,Shipped,UserInfoId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserInfoId = new SelectList(db.UserInfos, "UserInfoId", "UserInfoId", order.UserInfoId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserInfoId = new SelectList(db.UserInfos, "UserInfoId", "UserInfoId", order.UserInfoId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,DateTime,Shipped,UserInfoId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserInfoId = new SelectList(db.UserInfos, "UserInfoId", "UserInfoId", order.UserInfoId);
            return View(order);
        }
        */

        // GET: Orders/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.Shipped)
            {
                return RedirectToAction("List", "orders");
            }
            
            if (User.Identity.GetUserId() == order.UserInfo.UserInfoId || User.IsInRole("Admin"))
            {
                return View(order);
            } else
            {
                return RedirectToAction("List", "orders");
            }
        }

        // POST: Orders/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);

            if (order.Shipped)
            {
                return RedirectToAction("List", "orders");
            }

            if (User.Identity.GetUserId() == order.UserInfo.UserInfoId || User.IsInRole("Admin"))
            {
                db.Orders.Remove(order);
                db.SaveChanges();
                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index");
                } else
                {
                    return RedirectToAction("List", "orders");
                }
            }
            else
            {
                return RedirectToAction("List", "orders");
            }
        }


        // GET: Orders/Complete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            if (order.Shipped)
            {
                return RedirectToAction("Index");
            }
            
            return View(order);
        }

        // POST: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);

            if (order.Shipped)
            {
                return RedirectToAction("Index");
            }

            order.Shipped = true;

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "admin/orders", null);
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
