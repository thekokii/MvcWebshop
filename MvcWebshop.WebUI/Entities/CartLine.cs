using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.Entities
{

    // There is no need of Cart, because every user gets only one cart
    // Or is there?
    public class CartLine
    {
        public int CartLineId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string UserInfoId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}