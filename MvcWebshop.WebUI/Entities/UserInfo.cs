using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.Entities
{
    public class UserInfo
    {
        [Key, ForeignKey("ApplicationUser")]
        public string UserInfoId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<CartLine> CartLines { get; set; }
        public virtual ICollection<BeerReview> BeerReviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}