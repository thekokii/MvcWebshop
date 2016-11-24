using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWebshop.WebUI.DAL
{
    public interface ICartLineRepository : IRepository<CartLine>
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<UserInfo> GetUserInfos();
    }
}
