using System.Collections.Generic;

namespace MvcWebshop.WebUI.Entities
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}