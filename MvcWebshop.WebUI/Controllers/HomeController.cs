using MvcWebshop.WebUI.DAL;
using MvcWebshop.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebshop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBeerProductRepository repository = new BeerProductRepository(new ApplicationDbContext());

        public ActionResult Index()
        {
            var products = repository.GetAll().OrderByDescending(p => p.ABV).Take(3);

            return View(products.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}