using Microsoft.AspNet.Identity;
using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.AjaxHelpers;
using MvcWebshop.WebUI.DAL;
using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebshop.WebUI.Controllers
{
    public class CartLinesController : Controller
    {
        private ICartLineRepository repository = new CartLineRepository(new ApplicationDbContext());
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CartLines
        [Authorize]
        public ActionResult Index()
        {
            var userInfoId = User.Identity.GetUserId();
            var cartLines = repository.GetAll().Where(c => c.UserInfoId == userInfoId);
            //var cartLines = db.CartLines.Where(c => c.UserInfoId == userInfoId);

            return View(cartLines.ToList());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult AddToCart(int id)
        {
            var product = repository.GetProducts().FirstOrDefault(p => p.ProductId == id);
            //var product = db.Products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                var userInfoId = User.Identity.GetUserId();
                var cartLine = repository.GetAll().SingleOrDefault(c => c.UserInfoId == userInfoId
                                                            && c.ProductId == product.ProductId);

                //var userInfoId = User.Identity.GetUserId();
                //var cartLine = db.CartLines.SingleOrDefault(c => c.UserInfoId == userInfoId
                //                                            && c.ProductId == product.ProductId);

                if (cartLine == null)
                {
                    //db.CartLines.Add(new CartLine
                    repository.Add(new CartLine
                    {
                        UserInfoId = userInfoId,
                        Product = product,
                        Quantity = 1
                    });
                }
                else
                {
                    cartLine.Quantity++;
                }

                repository.Save();
                //db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [AjaxAuthorize]
        [Authorize]
        [HttpPost]
        public ActionResult AddToCartAJAX(int id, int quantity)
        {
            if (!Request.IsAuthenticated && Request.IsAjaxRequest())
            {
                Response.StatusCode = 401;
                return Content("");
            }

            var product = repository.GetProducts().FirstOrDefault(p => p.ProductId == id);
            //var product = db.Products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                var userInfoId = User.Identity.GetUserId();
                var cartLine = repository.GetAll().SingleOrDefault(c => c.UserInfoId == userInfoId
                                                                     && c.ProductId == product.ProductId);
                //var cartLine = db.CartLines.SingleOrDefault(c => c.UserInfoId == userInfoId
                //                                            && c.ProductId == product.ProductId);

                if (cartLine == null)
                {
                    //db.CartLines.Add(new CartLine
                    repository.Add(new CartLine
                    {
                        UserInfoId = userInfoId,
                        Product = product,
                        Quantity = quantity
                        //Quantity = 1
                    });
                }
                else
                {
                    cartLine.Quantity += quantity;
                    //cartLine.Quantity++;
                }

                repository.Save();
                //db.SaveChanges();
            }

            var result = repository.GetAll().Where(c => c.UserInfoId == User.Identity.GetUserId()).Sum(c => c.Quantity);

            return Json(result);
            //return Json(id);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult RemoveFromCart(int id)
        {
            var product = repository.GetProducts().FirstOrDefault(p => p.ProductId == id);
            //var product = db.Products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                var userInfoId = User.Identity.GetUserId();
                var cartLine = repository.GetAll().SingleOrDefault(c => c.UserInfoId == userInfoId
                                                                     && c.ProductId == product.ProductId);

                //var user = db.UserInfos.Find(User.Identity.GetUserId());
                //var cartLine = user.CartLines.SingleOrDefault(c => c.ProductId == product.ProductId);

                //var userInfoId = User.Identity.GetUserId();
                //var cartLine = db.CartLines.SingleOrDefault(c => c.UserInfoId == userInfoId
                //                                            && c.ProductId == product.ProductId);

                if (cartLine != null)
                {
                    repository.Delete(cartLine.CartLineId);
                    repository.Save();
                    //db.CartLines.Remove(cartLine);
                    //db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [AjaxAuthorize]
        [Authorize]
        [HttpPost]
        public ActionResult RemoveFromCartAJAX(int id)
        {
            var product = repository.GetProducts().FirstOrDefault(p => p.ProductId == id);
            //var product = db.Products.FirstOrDefault(p => p.ProductId == id);
            int quantity = 0;

            if (product != null)
            {
                var userInfoId = User.Identity.GetUserId();
                var cartLine = repository.GetAll().SingleOrDefault(c => c.UserInfoId == userInfoId
                                                                     && c.ProductId == product.ProductId);

                quantity = cartLine.Quantity;

                //var user = db.UserInfos.Find(User.Identity.GetUserId());
                //var cartLine = user.CartLines.SingleOrDefault(c => c.ProductId == product.ProductId);

                //var userInfoId = User.Identity.GetUserId();
                //var cartLine = db.CartLines.SingleOrDefault(c => c.UserInfoId == userInfoId
                //                                            && c.ProductId == product.ProductId);

                if (cartLine != null)
                {
                    repository.Delete(cartLine.CartLineId);
                    repository.Save();
                    //db.CartLines.Remove(cartLine);
                    //db.SaveChanges();
                }
            }

            var result = new { id = id, quantity = quantity };
            
            return Json(result, JsonRequestBehavior.AllowGet);
            //Send some message?
        }

        [Authorize]
        public ActionResult Checkout()
        {
            if(GetUser().CartLines.Count() == 0) { return RedirectToAction("Index", "cart", null); }

            Order order = new Order();
            order.UserInfo = GetUser();

            return View(order);
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult Checkout(Order order)
        {
            var user = GetUser();

            order.DateTime = DateTime.Now;
            order.UserInfo = user;
            order.OrderLines = new List<OrderLine>();
            order.CartToOrder(order.UserInfo.CartLines);

            user.Orders.Add(order);

            var toRemoveCartLines = repository.GetAll().Where(cl => cl.UserInfoId == user.UserInfoId);
            //var toRemoveCartLines = db.CartLines.Where(cl => cl.UserInfoId == user.UserInfoId);

            foreach (var cartLine in toRemoveCartLines)
            {
                repository.Delete(cartLine.CartLineId);
                //db.CartLines.Remove(cartLine);
            }

            repository.Save();
            //db.SaveChanges();

            return RedirectToAction("Index");
        }

        public PartialViewResult Summary()
        {
            return PartialView(GetUser());
        }

        // Do with binder
        private UserInfo GetUser()
        {
            return repository.GetUserInfos().FirstOrDefault(u => u.UserInfoId == User.Identity.GetUserId());
            //return db.UserInfos.Find(User.Identity.GetUserId());
        }


    }
}