using MvcWebshop.WebUI.Entities;
using MvcWebshop.WebUI.DAL;
using MvcWebshop.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MvcWebshop.WebUI.DAL
{
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(ApplicationDbContext context) : base(context)
        {
        }
        public ApplicationDbContext ApplicationDbContext { get { return Context as ApplicationDbContext; } }
    }
}
