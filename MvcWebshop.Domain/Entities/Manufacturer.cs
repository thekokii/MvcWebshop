using System.Collections.Generic;

namespace MvcWebshop.Domain.Entities
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}