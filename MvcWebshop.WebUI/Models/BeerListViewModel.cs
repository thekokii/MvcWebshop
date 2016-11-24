using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.Models
{
    public class BeerListViewModel
    {
        public IEnumerable<BeerProduct> BeerProducts { get; set; }
        //public PagingInfo PagingInfo { get; set; }
    }
}