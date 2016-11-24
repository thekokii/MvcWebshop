using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.Models
{
    public class BeerReviewCreateViewModel
    {
        public BeerProduct BeerProduct { get; set; }
        public BeerReview BeerReview { get; set; }
        public string ReturnUrl { get; set; }
    }
}