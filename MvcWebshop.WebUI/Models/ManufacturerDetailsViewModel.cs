using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace MvcWebshop.WebUI.Models
{
    public class ManufacturerDetailsViewModel
    {
        public Manufacturer Manufacturer { get; set; }
        public PagedList.IPagedList<BeerProduct> Products { get; set; }
    }
}