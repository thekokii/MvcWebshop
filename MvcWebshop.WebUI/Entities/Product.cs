using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWebshop.WebUI.Entities
{
    public abstract class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ImgPath { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        //public virtual ICollection<Review> Reviews { get; set; }
    }

    public class BeerProduct : Product
    {
        [Range(0, 99)]
        public decimal? ABV { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<BeerReview> BeerReviews { get; set; }

        public decimal Rating
        {
            get
            {
                if (BeerReviews != null)
                {
                    return Math.Round(BeerReviews.Sum(br => br.Calculated) / (decimal)BeerReviews.Count, 2);
                }
                else return 0;
            }
        }
    }
}
