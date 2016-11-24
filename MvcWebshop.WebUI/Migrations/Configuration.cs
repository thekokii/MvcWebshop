namespace MvcWebshop.WebUI.Migrations
{
    using Entities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcWebshop.WebUI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcWebshop.WebUI.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //context.Roles.AddOrUpdate(new IdentityRole { Name = "Admin" });
            //context.SaveChanges();

            var admin = context.Roles.SingleOrDefault(m => m.Name == "Admin");
            var user = context.Users.SingleOrDefault(m => m.UserName == "admin@beer.com");
            user.Roles.Add(new IdentityUserRole { RoleId = admin.Id });

            //context.Manufacturers.AddOrUpdate(m => m.Name,
            //new Manufacturer { Name = "BrewDog", Description = "Scottish big dog." },
            //new Manufacturer { Name = "MONYO", Description = "The most popular hungarian craft beer brand." },
            //new Manufacturer { Name = "Dreher", Description = "Hungarian piss beer manufacturer." }
            //);

            //context.Products.AddOrUpdate(p => p.Name,
            //new BeerProduct { ManufacturerId = 1, Name = "Punk IPA", Price = 790, ABV = 5.6M, Quantity = 50 },
            //new BeerProduct { ManufacturerId = 2, Name = "Flying Rabbit", Price = 590, ABV = 6.5M, Quantity = 80 },
            //new BeerProduct { ManufacturerId = 3, Name = "Arany Ászok", Price = 180, ABV = 4.5M, Quantity = 300 }
            //);
        }
    }
}
