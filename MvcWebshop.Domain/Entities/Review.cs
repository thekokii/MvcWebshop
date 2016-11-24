using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWebshop.Domain.Entities
{
    public abstract class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
        //public virtual Product Product { get; set; }
    }

    public class BeerReview : Review
    {
        [Range(1,10)]
        public int Aroma { get; set; }
        //[Range(1, 5)]
        public int Appearance { get; set; }
        //[Range(1, 10)]
        public int Taste { get; set; }
        //[Range(1, 5)]
        public int Palate { get; set; }
        //[Range(1, 20)]
        public int Overall { get; set; }
        public virtual BeerProduct BeerProduct { get; set; }
    }
}
