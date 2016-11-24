using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MvcWebshop.WebUI.Models;

namespace MvcWebshop.WebUI.DAL
{
    public class BeerReviewRepository : Repository<BeerReview>, IBeerReviewRepository
    {
        public BeerReviewRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<BeerProduct> GetProducts()
        {
            return ApplicationDbContext.Products;
        }

        public ApplicationDbContext ApplicationDbContext { get { return Context as ApplicationDbContext; } }

    }
}