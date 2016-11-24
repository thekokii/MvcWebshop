using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcWebshop.Domain.Entities.Reviews;
using System.ComponentModel.DataAnnotations;

namespace MvcWebshop.Domain.Entities.Products
{
    public class BeerProduct : Product
    {
        [Range(1,99)]
        public int ABV { get; set; }
        public virtual ICollection<Reviews.BeerReview> BeerReviews { get; set; }
    }
}
