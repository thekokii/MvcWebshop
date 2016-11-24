using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWebshop.Domain.Entities.Reviews
{
    public class BeerReview : Review
    {
        [Range(1,10)]
        public int Aroma { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual Products.BeerProduct BeerProduct { get; set; }
    }
}
