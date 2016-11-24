using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.DAL
{
    public class BeerProductRepository : Repository<BeerProduct>, IBeerProductRepository
    {
        public BeerProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Manufacturer> GetManufacturers()
        {
            return ApplicationDbContext.Manufacturers;
        }

        public ApplicationDbContext ApplicationDbContext { get { return Context as ApplicationDbContext; } }
    }
}