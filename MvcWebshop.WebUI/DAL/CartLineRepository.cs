using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MvcWebshop.WebUI.Models;
using MvcWebshop.WebUI.Entities;

namespace MvcWebshop.WebUI.DAL
{
    public class CartLineRepository : Repository<CartLine>, ICartLineRepository
    {
        public CartLineRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetProducts()
        {
            return ApplicationDbContext.Products;
        }
        public IEnumerable<UserInfo> GetUserInfos()
        {
            return ApplicationDbContext.UserInfos;
        }

        public ApplicationDbContext ApplicationDbContext { get { return Context as ApplicationDbContext; } }

    }
}