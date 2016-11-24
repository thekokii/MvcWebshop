using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateTime { get; set; }
        public bool Shipped { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public string UserInfoId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        
        public void CartToOrder(ICollection<CartLine> cartLines)
        {
            foreach (var item in cartLines)
            {
                OrderLines.Add(new OrderLine { Product = item.Product, Quantity = item.Quantity });
            }
        }
    }

    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}